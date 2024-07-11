using EmployeeDetails.Models;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//#Ticket No: Start: Niranjani Devi:Adding EmployeeRepository dependency to the service 
builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
//#Ticket No:End: Niranjani Devi:Adding EmployeeRepository dependency to the service

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5000); // HTTP
    serverOptions.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5000", "https://localhost:44368") // Replace with your client URL
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Comment out or remove the following line if you do not want HTTPS redirection
// app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowSpecificOrigin");


app.UseAuthorization();

app.MapControllers();

app.Run();