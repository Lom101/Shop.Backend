using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shop.WebAPI.Config;
using Shop.WebAPI.Data;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Infrastructure.Handlers;
using Shop.WebAPI.Infrastructure.Mappings;
using Shop.WebAPI.Repositories;
using Shop.WebAPI.Repository;
using Shop.WebAPI.Repository.Interfaces;
using Shop.WebAPI.SeedData;
using Shop.WebAPI.Services;
using Shop.WebAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ShopApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

# region Auth

//JWT Config
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

// Validation params
Byte[]? key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);
TokenValidationParameters? tokenValidationParams = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    RequireExpirationTime = false
};

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = tokenValidationParams;
});
builder.Services.AddSingleton(tokenValidationParams);

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>() // Add roles support
    .AddEntityFrameworkStores<ShopApplicationContext>();

// Register authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.Requirements.Add(new RoleRequirement("Admin")));
    options.AddPolicy("User", policy =>
        policy.Requirements.Add(new RoleRequirement("User")));
});

// custom authorization handler to handle role-based authorization
builder.Services.AddScoped<IAuthorizationHandler, RoleRequirementHandler>();

builder.Services.AddScoped<IJwtService, JwtService>();  

#endregion

#region Services and repositories

builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IAddressService, AddressService>();
builder.Services.AddTransient<ICommentService, CommentService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IBrandService, BrandService>();
builder.Services.AddTransient<IColorService, ColorService>();
builder.Services.AddTransient<IModelService, ModelService>();
builder.Services.AddTransient<ISizeService, SizeService>();


builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IAddressRepository, AddressRepository>();
builder.Services.AddTransient<ICommentRepository, CommentRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IBrandRepository, BrandRepository>();
builder.Services.AddTransient<IColorRepository, ColorRepository>();
builder.Services.AddTransient<IModelRepository, ModelRepository>();
builder.Services.AddTransient<ISizeRepository, SizeRepository>();



#endregion

var app = builder.Build();

// Seed the database with roles and users
await SeedData.Initialize(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Включаем использование CORS с политикой "AllowReactApp"
//app.UseCors("AllowReactApp");
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();