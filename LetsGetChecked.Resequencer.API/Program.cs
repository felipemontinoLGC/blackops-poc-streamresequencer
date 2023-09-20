using LetsGetChecked.Resequencer.API.HostedServices;
using LetsGetChecked.Resequencer.API.Interfaces;
using LetsGetChecked.Resequencer.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<IShardProcessorService, ShardProcessorService>();
builder.Services.AddTransient<IResequencerService, ResequencerService>();
builder.Services.AddHostedService<ResequencerHostedService>();

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
