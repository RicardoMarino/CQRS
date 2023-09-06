using Confluent.Kafka;
using CQRS.Command.Api.Commands;
using CQRS.Command.Domain.Aggregates;
using CQRS.Command.Infrastructure.Configurations;
using CQRS.Command.Infrastructure.Dispatchers;
using CQRS.Command.Infrastructure.Handlers;
using CQRS.Command.Infrastructure.Producers;
using CQRS.Command.Infrastructure.Repositories;
using CQRS.Command.Infrastructure.Stores;
using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;

var builder = WebApplication.CreateBuilder(args);
// Add Configurations
builder.Services.Configure<MongoConfiguration>(builder.Configuration.GetSection(nameof(MongoConfiguration)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));

// Add services injection
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventProducer, EventProducer>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();

// Add services command handler 
var commandHandlers = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var dispatcher = new CommandDispatcher();
dispatcher.RegisterHandler<NewPostCommand>(commandHandlers.HandlerAsync);
dispatcher.RegisterHandler<EditCommentCommand>(commandHandlers.HandlerAsync);
dispatcher.RegisterHandler<LikePostCommand>(commandHandlers.HandlerAsync);
dispatcher.RegisterHandler<AddCommentCommand>(commandHandlers.HandlerAsync);
dispatcher.RegisterHandler<EditCommentCommand>(commandHandlers.HandlerAsync);
dispatcher.RegisterHandler<RemoveCommentCommand>(commandHandlers.HandlerAsync);
dispatcher.RegisterHandler<DeletePostCommand>(commandHandlers.HandlerAsync);
builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);

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