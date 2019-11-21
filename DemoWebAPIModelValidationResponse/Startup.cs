using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DemoWebAPIModelValidationResponse
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
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddMvcOptions(options =>
                {
                    options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor((x) => $"沒有提供 '{x}' 參數或屬性的值");
                    options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "需要一個數值");
                    options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "需要一個非空的 HTTP Request Body");
                    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor((x) => $"值 '{x}' 是無效參數");
                    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => $"值 '{x}' 對 {y} 是無效參數");
                    options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor((x) => $"值 '{x}' 是無效參數");
                    options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor((x) => $"提供的值對於 {x} 無效");
                    options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "提供的值無效");
                    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor((x) => $"值 '{x}' 是無效參數");
                    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor((x) => $"值 `{x}` 必須是數字");
                    options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "值必須是數字");
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext => new BadRequestObjectResult(new { Message = "Model binding occurs problem." });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
