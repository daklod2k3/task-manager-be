using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using server;
using server.Context;
using server.Entities;
using server.Helpers;
using server.Middlewares;
using Supabase;

Env.Load();

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
var dataSource = dataSourceBuilder.Build();
builder.Services.AddDbContext<SupabaseContext>(options =>
    options.UseNpgsql(dataSource));

// add supabase
var supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL");
var supabaseAnonKey = Environment.GetEnvironmentVariable("SUPABASE_KEY");
var supabase = new Supabase.Client(supabaseUrl, supabaseAnonKey);
builder.Services.AddSingleton(supabase);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<AuthMiddleware>();

app.UseHttpsRedirection();
// app.UseAuthorization();
app.MapControllers();

app.Run();