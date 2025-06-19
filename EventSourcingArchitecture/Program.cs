using OnlineOrderSystem.Commands;
using OnlineOrderSystem.EventStore;
using OnlineOrderSystem.EventBus;
using OnlineOrderSystem.Queries;
using OnlineOrderSystem.ReadModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register Event Store
builder.Services.AddSingleton<IEventStore, InMemoryEventStore>();

// Register Event Bus
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();

// Register Command Handlers
builder.Services.AddScoped<PlaceOrderCommandHandler>();
builder.Services.AddScoped<UpdateOrderCommandHandler>();
builder.Services.AddScoped<CancelOrderCommandHandler>();

// Register Query Handlers
builder.Services.AddScoped<GetOrderByIdHandler>();

// Register Read Model Repository
builder.Services.AddSingleton<IOrderReadModelRepository, InMemoryReadModelRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Online Order System API V1");
        // Force HTTP for development
        c.DefaultModelsExpandDepth(-1);
    });
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
