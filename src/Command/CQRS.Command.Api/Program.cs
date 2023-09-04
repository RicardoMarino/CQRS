using CQRS.Command.Infrastructure.Configurations;
using CQRS.Command.Infrastructure.Repositories;
using CQRS.Command.Infrastructure.Stores;
using CQRS.Core.Domain;
using CQRS.Core.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
// Add Configurations
builder.Services.Configure<MongoConfiguration>(builder.Configuration.GetSection(nameof(MongoConfiguration)));

// Add services injection
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventStore, EventStore>();

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();