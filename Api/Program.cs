
using System.Reflection;
using Api;
using Application.Services;
using Core;
using Core.IRepositories;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//fill configs from appsetting.json
builder.Services.AddOptions();
builder.Services.Configure<Configs>(builder.Configuration.GetSection("Configs"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c =>
// {
//   c.SwaggerDoc("v1", new OpenApiInfo { Title = "Online Store API", Version = "v1" });
//   c.EnableAnnotations();
// });

//builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

//builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddRepositories();

// DbContext registration
string connectionString = builder.Configuration.GetConnectionString("SqlConnection");

builder.Services.AddDbContext<OnlineShopDbContext>(options => {
    options.UseSqlServer(connectionString);
});

builder.Services.AddSwagger();
builder.Services.AddJWT();

// Services Registration
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddUnitOfWork();
builder.Services.AddInfraUtility();

// var config =
var config = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new Application.AutoMapperConfig());
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
