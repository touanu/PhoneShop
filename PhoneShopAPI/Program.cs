using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PhoneShop.DataAccess;
using PhoneShop.DataAccess.IServices;
using PhoneShop.DataAccess.Services;
using PhoneShop.DataAccess.UnitOfWork;
using PhoneShopAPI;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<PhoneShopDBcontext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("MyConnect")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        ValidAudience = builder.Configuration["Jwt:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBrandServices , BrandServices>();
builder.Services.AddScoped<IProductServices , ProductServices>();
builder.Services.AddScoped<IAccountServices, AccountServices>();
builder.Services.AddScoped<IPromotionServices, PromotionServices>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<IOrderServices, OrderServices>();
builder.Services.AddScoped<INewServices, NewServices>();
builder.Services.AddScoped<IAddressServices, AddressServices>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Hello world!");
//});
//app.UseMiddleware<MyCustomMiddleWare>();
app.UseMyMiddleware();
app.UseAuthorization();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
