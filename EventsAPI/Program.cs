using EventsAPI.Adapters;
using EventsAPI.BLL.DTO;
using EventsAPI.BLL.Interfaces;
using EventsAPI.BLL.Mappers;
using EventsAPI.BLL.Services;
using EventsAPI.BLL.Validators;
using EventsAPI.DAL.Data;
using EventsAPI.DAL.Interfaces;
using EventsAPI.DAL.Repositories;
using EventsAPI.Mappers;
using EventsAPI.Middlewares;
using EventsAPI.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EventsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<UserMapper>();
                cfg.AddProfile<EventMapper>();
                cfg.AddProfile<UserDTOMapper>();
                cfg.AddProfile<EventDTOMapper>();
            });

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddScoped<IValidator<RegisterDTO>, RegisterDTOValidator>();
            builder.Services.AddScoped<IValidator<LoginDTO>, LoginDTOValidatior>();
            builder.Services.AddScoped<IValidator<CreateEventDTO>, CreateEventDTOValidator>();
            builder.Services.AddScoped<IValidator<ChangeEventDTO>, ChangeEventDTOValidator>();

            builder.Services.AddScoped<EventsService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<EmailSender>();
            builder.Services.AddScoped<ImageService>();
            builder.Services.AddScoped<PasswordService>();
            builder.Services.AddScoped<TokenService>();

            builder.Services.AddSingleton<IImagePath, WebHostAdapter>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IEventRepository, EventRepository>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                    };
                });

            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"))
                .AddPolicy("UserPolicy", policy => policy.RequireRole("User"));

            var app = builder.Build();

            app.UseCors(builder =>
            {
                builder
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("X-Page-Count", "Location")
                    .AllowCredentials();
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.Run();
        }
    }
}
