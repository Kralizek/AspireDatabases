using Microsoft.EntityFrameworkCore;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddNpgsqlDbContext<TodoItemDbContext>("pgdb", configureDbContextOptions: o => o.UseSnakeCaseNamingConvention());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var todos = app.MapGroup("todos");

todos.MapGet("/", async (TodoItemDbContext db, CancellationToken cancellationToken) => await db.Items.ToListAsync(cancellationToken));

app.MapDefaultEndpoints();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoItemDbContext>();

    await db.Database.EnsureCreatedAsync();
}

app.Run();
