using AutoMapper;
using DoctorWho2.Authintication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RealWord.DB;
using RealWord.DB.Entities;
using RealWord.DB.Repositories;
using RealWord.DB.Services;
using RealWordBE.Authentication;
using RealWordBE.Authentication.Logout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWordBE
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
            services.AddScoped<ILikeRepository ,LikeRepository>();
            services.AddScoped<ITagRepository ,TagRepository>();
            services.AddScoped<ICommentRebository ,CommentRebository>();

            services.AddScoped<IFollowerRepository ,FollowerRepository>();
            services.AddScoped<IArticleRebository ,ArticleRebository>();
            services.AddScoped<IArticleTagRebository ,ArticleTagRebository>();

            services.AddScoped<IArticleService ,ArticleService>();
            services.AddScoped<IProfileService ,ProfileService>();
            services.AddScoped<ICommentService ,CommentService>();
            services.AddScoped<ILikeService ,LikeService>();
            services.AddScoped<ITagService ,TagService>();

            //services.AddDbContext<ApplicationDbContext>(options =>
            //{
            //    options.UseInMemoryDatabase("postman_test_inmeomry");
            //});



            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("RealWorldDB"))

               );
            services.Configure<JWT>(Configuration.GetSection("JWT"));
            //User Manager Service
            services.AddIdentity<User ,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddScoped<IUserRepository ,UserRepository>();


            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true ,
                        ValidateIssuer = true ,
                        ValidateAudience = true ,
                        ValidateLifetime = true ,
                        ClockSkew = TimeSpan.Zero ,
                        ValidIssuer = Configuration["JWT:Issuer"] ,
                        ValidAudience = Configuration["JWT:Audience"] ,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                    };
                });
            services.AddAutoMapper();
            services.AddTransient<ITokenManager ,TokenManager>();
            services.AddSingleton<IHttpContextAccessor ,HttpContextAccessor>();
            services.AddTransient<TokenManagerMiddleware>();
            services.AddSingleton<IMemoryCache ,MemoryCache>();
            services.AddMvc(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app ,IWebHostEnvironment env)
        {
            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<TokenManagerMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
