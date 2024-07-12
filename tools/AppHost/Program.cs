var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("Password");

var cosmos = builder.AddAzureCosmosDB("cosmos")
                    .RunAsEmulator()
                    .AddDatabase("cosmosdb");

builder.AddProject<Projects.CosmosDbWeb>("cosmosWeb")
       .WithReference(cosmos);

var mssql = builder.AddSqlServer("mssql", password)
                   .WithDataVolume("aspire-tests-mssql")
                   .AddDatabase("mssqldb");

builder.AddProject<Projects.MsSqlWeb>("mssqlWeb")
       .WithReference(mssql);

var npgsql = builder.AddPostgres("pg", password: password)
                    .WithDataVolume("aspire-tests-pg")
                    .AddDatabase("pgdb");

builder.AddProject<Projects.PostgreSqlWeb>("pgWeb")
       .WithReference(npgsql);

builder.Build().Run();
