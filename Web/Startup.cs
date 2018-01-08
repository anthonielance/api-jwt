using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Web.Constraints;
using Web.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddDbContext<ApiDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IRepository<BaseEntity>, ApiRepository<BaseEntity>>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddAzureAdBearer(options => Configuration.Bind("AzureAd", options));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ADMIN_ACCESS",
                    policy => policy.RequireRole("admin_access"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("ALLOW_SOME_SITE",
                    builder => builder
                        .WithOrigins("https://some.site.com")
                        .WithMethods("GET")
                        .AllowAnyHeader());
            });

            services.Configure<RouteOptions>(config => config.ConstraintMap.Add("email", typeof(EmailConstraint)));
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new RequireHttpsAttribute());
                options.Filters.Add(new CorsAuthorizationFilterFactory("ALLOW_SOME_SITE"));
                options.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseAuthentication()
               .UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent())
               .UseCors("ALLOW_SOME_SITE");

            app.UseMiddleware<SerilogMiddleware>()
               .UseSerilogCorrelationIdEnricher()
               .UseSerilogUPNEnricher();

            app.UseMvc();
        }
    }
}
