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
using Utils;

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
            services
                // 檢查 HTTP Header 的 Authorization 是否有 JWT Bearer Token
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                // 設定 JWT Bearer Token 的檢查選項
                .AddJwtBearer(options =>
                {
                    // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
                    options.IncludeErrorDetails = true; // 預設值為 true，有時會特別關閉
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // 一般我們都會驗證 Issuer
                        ValidateIssuer = true, //發行者驗證
                        ValidIssuer = Configuration.GetValue<string>("JwtSettings:Issuer"),
                        // 通常不太需要驗證 Audience
                        ValidateAudience = true,//接收者驗證
                        ValidAudience = Configuration.GetValue<string>("JwtSettings:Issuer"), // 不驗證就不需要填寫
                        // 一般我們都會驗證 Token 的有效期間
                        ValidateLifetime = true, //存活時間驗證
                        // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
                        ValidateIssuerSigningKey = true, //金鑰驗證
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:SignKey"]))
                    };
                });

            //引入Controllers回傳格式為json檔
            services.AddControllers().AddNewtonsoftJson(
                options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                }
            ); 
            
            //AddDbContext
            services.AddDbContext<MyContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //用來接前端的資料 mvc之前自動寫好了 這邊要設定
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<JwtHelpers>();

            //自動產生API文件的差件
            services.AddSwaggerGen();
            //AutoMap
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
            
            app.UseCors("AnotherPolicy");//跨域
        
            dbContext.Database.EnsureCreated();//建立資料庫的連接

            app.UseHttpsRedirection();//HTTP導向HTTPS
            
            app.UseRouting();//ROUT的東西
            
            app.UseAuthentication();//先驗證

            app.UseAuthorization();//再授權
            
            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();//MVC
            });
        }
    }
}
//建構子