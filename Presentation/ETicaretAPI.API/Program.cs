using ETicaretAPI.Application;
using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.Validators.Products;
using ETicaretAPI.Infrastructure;
using ETicaretAPI.Infrastructure.Concretes.Storage.Local;
using ETicaretAPI.Infrastructure.Filters;
using ETicaretAPI.Infrastructure.Services.Storage.Azure;
using ETicaretAPI.Persistent;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddPersistanceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

//builder.Services.AddStorage<LocalStorage>();
builder.Services.AddStorage<AzureStorage>();

//builder.Services.AddCors(options => options.AddDefaultPolicy(policy=>policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
builder.Services.AddCors(options=> options.AddDefaultPolicy(policy=>policy.WithOrigins("http://localhost:4200", "htpps://localhost:4200").AllowAnyOrigin().AllowAnyMethod()));

builder.Services.AddControllers(opt=>opt.Filters.Add<ValidationFilter>()).AddFluentValidation(config=>config.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>()).ConfigureApiBehaviorOptions(opt=>opt.SuppressModelStateInvalidFilter=true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication("Admin").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateAudience = true, //olu�turulacak token de�erini hangi originlerin/sitelerin kullanaca��n� belirledi�imiz de�erdir.
        ValidateIssuer = true, //olu�turulacak token de�erini kimin da��tt���n� ifade edece�imiz aland�r.
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true, //olu�turulacak token de�erinin uygulamam�za ait bir de�er oldu�unu ifade eden suciry key verisinin do�rulanmas�d�r.
        ValidAudience = builder.Configuration["Token:Audience"],
        ValidIssuer = builder.Configuration["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]))
    };
});

 
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
