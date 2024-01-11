using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using URLShortener;
using URLShortener.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContextPool<UrlShortenerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("URLShortenerDB"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUrlShortenerService, UrlShortenerServiceImpl>();

builder.Services.AddFluentValidationAutoValidation();

string AllowOrigins = builder.Configuration["Cors"];
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigins",
               builder =>
               {
                   builder.SetIsOriginAllowed(origin => true)
                //builder.WithOrigins(AllowOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();

               });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(AllowOrigins);

app.MapControllers();

app.Run();
