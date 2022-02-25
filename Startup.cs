using KarKhanaBook.Core.Challan;
using KarKhanaBook.Core.Login;
using KarKhanaBook.Core.Taka;
using KarKhanaBook.Middleware;
using KarKhanaBook.Model.Kariger;
using KarKhanaBook.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace KarKhanaBook
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            KarkhanaBookContext.KarkhanaBookDataContext db
           = new KarkhanaBookContext.KarkhanaBookDataContext
           ("Data Source=DESKTOP-D9KFJSI;Initial Catalog=KarkhanaBook;Integrated Security=True;Persist Security Info=True;License Key=qHnH5wx/L422kFN4WQussVkqbelF0xGMaZi+DGL6lhFu+VTasW/ZRA22+dVoDbuQ64trDZsBMziLDE9kumHeTDKlcRSCvsotqn7rHn9VHFXS3Jmh/rFBVSxav6UlKmT4POdU+hnX8ACaigXhFdBiZ4NeHNVRNTqJ4fUTou0czKt8ATWxOB2MjUrprbYTV2ECFJOo2uLgwGzqeEpv1gGPLKR3p5DOKdeMu61FRAak23fmjt8PPQpz50o1E0r0FFdoQrJIYKkMxqRiD2IhVxlcVCvpIqR31rWwKJ1sNquGBMU=;");

            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy",
                                  builder =>
                                  {
                                      builder.SetIsOriginAllowed(host => true)
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials();
                                  });
            });
            services.AddControllers();
            //JSON Serializer
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
                = new DefaultContractResolver());

            services.AddControllers().AddNewtonsoftJson();
            services.AddControllers().AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.Converters.Add(new DateConverter());

            });

            services.AddAuthentication(option =>
            {

                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                 .AddJwtBearer(option =>
                 {
                     option.SaveToken = true;
                     option.RequireHttpsMetadata = false;
                     option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidAudience = Configuration["JWT:ValidAudience"],
                         ValidIssuer = Configuration["JWT:ValidIssuer"],
                         IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                     };
                 });


            services.AddTransient<ChallanPayments>();
            services.AddTransient<ChallanSlips>();
            services.AddTransient<TakaChallans>();
            services.AddTransient<Token>();
            services.AddTransient<KarigerDailySheet>();
            services.AddTransient<Roles>();
            services.AddTransient<Users>();
            services.AddTransient<TakaSheets>();
            services.AddTransient<TakaChallans>();


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "KarKhanaBook", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("MyPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KarKhanaBook v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseMiddleware<CustomeException>();
            app.UseMiddleware<JWTHandler>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
