using BackendEY.Data;
using BackendEY.Services;
using BackendEY.Services.Scraping;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactDevClient",
        policy =>
        {
            policy.WithOrigins("http://3.85.36.85")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var apiToken = builder.Configuration["Security:ApiToken"] ?? "mi-token-secreto";


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ProveedorService>();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IScraperStrategy, OFACScraper>();
builder.Services.AddScoped<IScraperStrategy, TheWorldBankScraper>();
builder.Services.AddScoped<WebScrapingService>();

// Autenticación Bearer
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault();
                if (token != null && token.StartsWith("Bearer "))
                {
                    var providedToken = token.Substring("Bearer ".Length);
                    if (providedToken == apiToken)
                    {
                        var identity = new System.Security.Claims.ClaimsIdentity("Bearer");
                        context.Principal = new System.Security.Claims.ClaimsPrincipal(identity);
                        context.Success();
                    }
                    else
                    {
                        context.NoResult();
                        context.Response.StatusCode = 401;
                    }
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactDevClient");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
