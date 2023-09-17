using E_Pharmacy.Helpers;
using E_Pharmacy.Models;
using E_Pharmacy.Sefvices.AuthenticationServices;
using E_Pharmacy.Sefvices.CartServices;
using E_Pharmacy.Sefvices.MedicineServices;
using E_Pharmacy.Sefvices.ShortCommingServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var conncetion = builder.Configuration.GetConnectionString("Connection");

// Add services to the container.
builder.Services.AddAuthentication(op => {
    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = false;
    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateLifetime=true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience= builder.Configuration["JWT:Audience"],
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});


//Map JWT class to JWT object in appsettings.json
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

//to use Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddDbContext<ApplicationDBContext>(options => {
    options.UseSqlServer(conncetion);
} );

//inject medicineTypeService within IMedicineType
builder.Services.AddScoped<IMedicineType, MedicineTypeService>();

//inject MedicineService within IMedicine
builder.Services.AddScoped<IMedicine, MedicineService>();

//inject AuthService within IAuthService
builder.Services.AddScoped<IAuthService, AuthService>();

//inject CartService within ICartService
builder.Services.AddScoped<ICartService, CartService>();

//inject ShortCommingService within IShortComming
builder.Services.AddScoped<IShortComming, ShortCommingService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
