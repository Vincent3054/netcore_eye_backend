using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DBContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

namespace project
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //呼叫下面
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //設定差件
        public void ConfigureServices(IServiceCollection services)
        {
            
            //跨域設定
            services.AddCors(option =>
            option.AddPolicy("AnotherPolicy", builder =>
            {
                builder.AllowAnyOrigin()/*設定允許的原始來源 */
                       .AllowAnyHeader()/*設定允許所有作者要求標頭*/
                       .AllowAnyMethod();/*設定允許任何 HTTP(Get、post...) */
            }));
            //JWT設定   
            // STEP1: 設定用哪種方式驗證 HTTP Request 是否合法
            services
                // 檢查 HTTP Header 的 Authorization 是否有 JWT Bearer Token
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                // 設定 JWT Bearer Token 的檢查選項
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, //發行者驗證
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidateAudience = true,//接收者驗證
                        ValidAudience = Configuration["Jwt:Issuer"],
                        ValidateLifetime = true, //存活時間驗證
                        ValidateIssuerSigningKey = true, //金鑰驗證
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });
            //引入Controllers回傳格式為json檔
            services.AddControllers().AddNewtonsoftJson(
                options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                }
            ); //AddControllers
            services.AddDbContext<MyContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            //用來接前端的資料 mvc之前自動寫好了 這邊要設定
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //自動產生API文件的差件
            services.AddSwaggerGen();
            //在研究
            services.AddAutoMapper(typeof(Startup));
        }
        //啟用差件(按照正確的順序)
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MyContext dbContext)
        {
            //開發模式的設定
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //建立資料庫的連接
            dbContext.Database.EnsureCreated();
            
            //ROUT的東西
            app.UseRouting();
            //跨域
            app.UseCors("AnotherPolicy");
            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            // STEP2: 使用驗證權限的 Middleware
            app.UseAuthorization();//驗證
            app.UseHttpsRedirection();//HTTP導向HTTPS
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();//MVC
            });
        }
    }
}
//建構子