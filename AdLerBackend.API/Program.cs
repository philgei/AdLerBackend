using System.Reflection;
using AdLerBackend.API.Filters;
using AdLerBackend.Application;
using AdLerBackend.Infrastructure;
using Microsoft.Net.Http.Headers;


// This is needed, because wwwroot directory must be present in the beginning to serve files from it
Directory.CreateDirectory("wwwroot");


var builder = WebApplication.CreateBuilder(args);

// Use Global AdLer Config File (Most likely coming from a docker volume)
builder.Configuration.AddJsonFile("./config/config.json", true);

// Add HTTPS support
if (!builder.Environment.IsDevelopment())
    builder.WebHost.ConfigureKestrel(options =>
    {
        if (builder.Configuration["useHttps"] == "True")
            options.ListenAnyIP(int.Parse(builder.Configuration["httpsPort"]),
                listenOptions =>
                {
                    listenOptions.UseHttps("./config/cert/AdLerBackend.pfx",
                        builder.Configuration["httpsCertificatePassword"]);
                });
        else
            options.ListenAnyIP(int.Parse(builder.Configuration["httpPort"]));
    });


builder.Services.AddControllers(
    options => { options.Filters.Add(new ApiExceptionFilterAttribute()); }
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment.IsDevelopment());

builder.Services.AddCors(options =>
{
    options.AddPolicy("test",
        policy =>
        {
            // policy.WithOrigins("http://example.com",
            //                     "http://www.contoso.com",
            //                     "http://localhost:3000");
            policy.AllowAnyOrigin();
            policy.WithHeaders(HeaderNames.AccessControlAllowHeaders, "*");
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("test");

// Disabled for now, because it is not needed
//app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapControllers();

app.Run();