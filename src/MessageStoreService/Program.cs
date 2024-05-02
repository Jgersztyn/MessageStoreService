using Hangfire;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration
    .AddEnvironmentVariables()
    .AddJsonFile("appsettings.Local.json", true, true).Build();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddServices(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseSwaggerUI();

app.UseRouting();
app.UseHttpsRedirection();
app.UseSwagger();

// API endpoint for testing
app.MapGet("/recordstores", async () =>
{
    Console.WriteLine("Add the endpoint for get recordstores here");
})
.WithName("GetRecordStores")
.WithOpenApi();

// Hangfire dashboard configuration
//app.UseHangfireDashboard(options: new DashboardOptions
//{
//    IgnoreAntiforgeryToken = true,
//    Authorization = new []{ new DashboardNoAuthorizationFilter() }
//});
//app.MapHealthChecks("health-check");

app.Run();