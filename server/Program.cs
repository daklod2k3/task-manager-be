using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using server.Context;
using server.Controllers;
using server.Entities;
using server.Interfaces;
using server.Repository;
using server.Services;
using Supabase;

Env.Load();

var builder = WebApplication.CreateBuilder(args);
var bytes = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:JwtSecret"]!);
var cookieAuthName = builder.Configuration["Authentication:CookieAuthName"]!;


// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Auth builder
builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(bytes),
        ValidAudience = builder.Configuration["Authentication:ValidAudience"],
        ValidIssuer = builder.Configuration["Authentication:ValidIssuer"]
    };
    option.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies[cookieAuthName];
            return Task.CompletedTask;
        }
    };
});

//ORM builder
var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
dataSourceBuilder.MapEnum<ETaskPriority>("TaskPriority");
dataSourceBuilder.MapEnum<ETaskStatus>("TaskStatus");
var dataSource = dataSourceBuilder.Build();
builder.Services.AddDbContext<SupabaseContext>(options =>
    options.UseNpgsql(dataSource));

// add supabase
var supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL");
var supabaseAnonKey = Environment.GetEnvironmentVariable("SUPABASE_KEY");
var supabase = new Client(supabaseUrl, supabaseAnonKey);
supabase.Auth.Options.AllowUnconfirmedUserSessions = true;
builder.Services.AddSingleton(supabase);


builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IReportService, ReportService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.MapControllers().RequireAuthorization();
app.UseExceptionHandler(e =>
{
    e.Run(async context =>
    {
        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature == null) return;
        var json = JsonSerializer.Serialize(new ErrorResponse(contextFeature.Error.Message)
            { Status = HttpStatusCode.InternalServerError });
        Console.WriteLine(context.Response.StatusCode);
        // if (context.Response.StatusCode != 0)
        //     context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(json);
    });
});
// app.UseMiddleware<AuthMiddleware>();

app.Run();