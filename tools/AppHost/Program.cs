var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("Password");

var cosmos = builder.AddAzureCosmosDB("cosmos")
                    .AddDatabase("cosmosdb")
                    .RunAsEmulator(emulator =>
                    {
                        emulator.WithVolume("aspire-tests-cosmosdb", "/tmp/cosmos/appdata")
                                .WithEnvironment("AZURE_COSMOS_EMULATOR_ENABLE_DATA_PERSISTENCE", "true");
                    });

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
