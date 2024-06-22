using ExamenQualisys.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

// Conexión BD
builder.Services.AddDbContext<ContextModel>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

// SeriLog
//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Warning()
//    .WriteTo.File(new CompactJsonFormatter(),"Log/log.txt", rollingInterval: RollingInterval.Day)
//    .CreateLogger();
//builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
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
