using food_delivery.Services;
using food_delivery.Models;
using food_delivery.Producers;
using food_delivery.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<FoodDeliveryDatabaseSettings>(builder.Configuration.GetSection("FoodDeliveryDatabase"));
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddSingleton<MerchantsService>();
builder.Services.AddSingleton<MerchantMenuProducer>();
builder.Services.AddSingleton<IMerchantMenuService, MerchantMenuService>();
builder.Services.AddSignalR();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    builder.Services.AddCors();
    app.UseCors(options => options
        .WithOrigins(
            "http://172.19.0.2:30010",
            "http://localhost:3000"
            )
        // .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .SetIsOriginAllowed((host) => true));

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<OrdersHub>("/hubs/orders");
});

app.UseHttpsRedirection();

app.UseAuthorization();


app.Run();
