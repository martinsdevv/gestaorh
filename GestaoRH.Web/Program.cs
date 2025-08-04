using GestaoRH.Data;
using Microsoft.EntityFrameworkCore;
using GestaoRH.Web.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddFolderApplicationModelConvention("/", model =>
    {
        model.Filters.Add(new AuthPageFilter());
    });
});

var envConn = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");
if (!string.IsNullOrEmpty(envConn))
{
    builder.Configuration["ConnectionStrings:DefaultConnection"] = envConn;
}


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona HttpClient para consumir a API
builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri("http://localhost:5156"); // URL da API
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var config = builder.Configuration;

        var jwtKey = config["Jwt:Key"] ?? Environment.GetEnvironmentVariable("JWT_KEY")
            ?? throw new Exception("JWT Key não configurada!");

        var jwtIssuer = config["Jwt:Issuer"] ?? Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "GestaoRH";
        var jwtAudience = config["Jwt:Audience"] ?? Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "GestaoRH";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("jwt", out var token))
                    context.Token = token;

                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddAuthorization();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapRazorPages();

app.Run();
