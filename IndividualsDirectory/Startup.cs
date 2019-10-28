using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using IndividualsDirectory.Entities.Context;
using IndividualsDirectory.Helpers.CustomActionFilters;
using IndividualsDirectory.Helpers.CustomMiddlewares;
using IndividualsDirectory.Helpers.Extensions;
using IndividualsDirectory.Models.Create;
using IndividualsDirectory.Models.Update;
using IndividualsDirectory.Repositories;
using IndividualsDirectory.Repositories.Abstracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IndividualsDirectory
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        public ILogger<Startup> Logger { get; }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<IndividualsDirectoryDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("IndividualsDirectoryDatabase"));
            });

            services
                .AddLocalization(i => i.ResourcesPath = "Resources");

            services
                .AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddFluentValidation()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddRouting(options => options.LowercaseUrls = true);

            services
                .AddScoped<IEntityRepository, EntityRepository>();

            services.AddSingleton<ValidateModelStateAttribute>();

            services.AddTransient<IValidator<CreateIndividualViewModel>, CreateIndividualValidator>();
            services.AddTransient<IValidator<CreatePhoneNumberViewModel>, CreatePhoneNumberValidator>();
            services.AddTransient<IValidator<CreateRelatedIndividualViewModel>, CreateRelatedIndividualValidator>();
            services.AddTransient<IValidator<UpdateIndividualViewModel>, UpdateIndividualValidator>();
            services.AddTransient<IValidator<UpdatePhoneNumberViewModel>, UpdatePhoneNumberValidator>();
            services.AddTransient<IValidator<UpdateRelatedIndividualViewModel>, UpdateRelatedIndividualValidator>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseGlobalExceptionHandlerMiddleware(Logger, "/individuals/error");
                app.UseHsts();
            }

            app.UseRequestLocalization(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("ka-GE"),
                };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseDataSeeding();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=individuals}/{action=index}/{id?}");
            });
        }
    }
}
