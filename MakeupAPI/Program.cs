using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using MakeupAPI.Context;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
//using MakeupAPI.Filters;
using MakeupAPI.Interfaces;
using MakeupAPI.Repositories;
using MakeupAPI.AuthorizationAndAuthentication;
using System.Text;
using MakeupAPI.Filters;

namespace MakeupAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(cors => cors.AddPolicy("AllowOriginAndMethod", options => options
              .WithOrigins(new[] { "https://localhost:5000", "https://localhost:5001" })
              .AllowAnyMethod())
            );

            // Add services to the container.
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(CustomExceptionFilter));
                options.Filters.Add(typeof(CustomLogsFilter));
            }).AddNewtonsoftJson();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Informe o token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }

                });
            });


            #region Conexao In Memory database

            builder.Services.AddDbContext<InMemoryContext>(options => options.UseInMemoryDatabase("MakeupAPI"));

            #endregion

            #region Injecao de dependencia do Repository

            builder.Services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
            builder.Services.AddScoped(typeof(ILoginRepository), typeof(LoginRepository));

            #endregion


            #region Injeção de dependência do JWT Token
            var tokenConfiguration = new TokenConfiguration();

            new ConfigureFromConfigurationOptions<TokenConfiguration>(builder.Configuration.GetSection("TokenConfiguration")).Configure(tokenConfiguration);

            builder.Services.AddSingleton(tokenConfiguration);

            var generateToken = new GenerateToken(tokenConfiguration);

            builder.Services.AddScoped(typeof(GenerateToken));
            #endregion

     
            //middleware de validação/autenticação do token
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidAudience = tokenConfiguration.Audience,
                    ValidIssuer = tokenConfiguration.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.Secret))
                };
            });

            builder.Services.AddAuthorization(options => options.AddPolicy("ValidateClaimModule", policy => policy.RequireClaim("module", "Web III .net")));
  


            #region Registra o Data Generator

            builder.Services.AddTransient<DataGenerator>();

            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseCors("AllowOriginAndMethod");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();


            #region Popula o banco de dados
            var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopedFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<DataGenerator>();
                service.Generate();
            }
            #endregion

            app.Run();
        }
    }
}