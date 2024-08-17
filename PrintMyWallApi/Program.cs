
using System.Security.Cryptography.X509Certificates;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


//var host = builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.ConfigureHttpsDefaults(listenOptions =>
//    {
//        listenOptions.ServerCertificate = new X509Certificate2("localhost.crt", "localhost.key");
//    });
//});

//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(5001); // Слушаем на порту 5001 вместо 5000
//});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
//{
//    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidIssuer = "printmywall.ca",
//        ValidAudience = "printmywall.ca",
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecureKeyHere"))
//    };
//});
var app = builder.Build();

//app.Use(async (context, next) =>
//{
//    var apiKey = context.Request.Headers["X-API-KEY"].FirstOrDefault();
//    var configuredApiKey = builder.Configuration.GetValue<string>("ApiKey");

//    if (apiKey == null || apiKey != configuredApiKey)
//    {
//        context.Response.StatusCode = 401; // Unauthorized
//        await context.Response.WriteAsync("Unauthorized");
//        return;
//    }

//    await next();
//});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
