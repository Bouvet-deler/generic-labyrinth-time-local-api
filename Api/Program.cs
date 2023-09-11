using generic_high_score_local_api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                      policy =>
                      {
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          policy.AllowAnyOrigin();
                      });
});

builder.Services.AddSingleton<Application>();
//Comment the line below if using simulate methods
builder.Services.AddHostedService<HardwereBackgroundService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DocumentTitle = "PhobosRelay";
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EPOXI");
    c.RoutePrefix = string.Empty;  // Set Swagger UI at apps root
});

//app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.MapRoutes(); // Custom function to register endpoints (controllers)

app.Run("https://localhost:5050");
