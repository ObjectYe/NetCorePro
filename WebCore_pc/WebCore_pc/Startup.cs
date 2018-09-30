using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebCore_pc.core;

namespace WebCore_pc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
           // var uni= Configuration.GetType();

        }

        public IConfiguration Configuration { get; }

        public static IContainer AutofacContainer;

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.AddConnections().SelectMany<d>

            #region 注入核心服务 加载signalcores
            //services.AddCors(options => {
            //    options.AddPolicy("SignalRCores",
            //            policy => policy.AllowAnyOrigin()
            //                          .AllowAnyHeader()
            //                          .AllowAnyMethod()
            //                          .AllowCredentials());
            //});
            //services.AddSignalR();
            #endregion

            #region autofac register
            ContainerBuilder builder = new ContainerBuilder();
            //将services中的服务填充到Autofac中.
            builder.Populate(services);
            //新模块组件注册
            builder.RegisterModule<DefaultModuleRegister>();
            //创建容器.
            AutofacContainer = builder.Build();
            //使用容器创建 AutofacServiceProvider 
            return new AutofacServiceProvider(AutofacContainer);

            #endregion


        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseHsts();
            }

            #region signalr 注入容器

            //app.UseCors("SignalRCores");

            //app.UseSignalR(routes =>
            //{

            //    //通过 api/chathub 调用监听方法
            //    routes.MapHub<ChatHub>("/chathub");

            //});
            #endregion 

            app.UseHttpsRedirection();
            app.UseMvc();


            //程序停止调用函数
            appLifetime.ApplicationStopped.Register(() => { AutofacContainer.Dispose(); });

        }
    }
}
