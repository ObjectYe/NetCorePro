
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace WebCore_pc.core
{
    public class DefaultModuleRegister : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //注册当前程序集中以“Ser”结尾的类,暴漏类实现的所有接口，生命周期为PerLifetimeScope
            //builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Ser")).AsImplementedInterfaces().InstancePerLifetimeScope();
            //builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
           
            //注册所有"MyApp.Repository"程序集中的类

            builder.RegisterAssemblyTypes(GetAssembly("IDao")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(GetAssembly("IServices")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(GetAssembly("Daos")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(GetAssembly("Services")).AsImplementedInterfaces();


            //builder.RegisterCallback(Action<IComponentContext>).Callback(IComponentContext);

            //builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
            //    .Where(a=>a.Name.EndsWith("")).AsImplementedInterfaces().InstancePerDependency();

            //builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
            //    .Where(a => a.Name.TrimEnd().EndsWith("services")).AsImplementedInterfaces().InstancePerDependency();



        }

        public static System.Reflection.Assembly GetAssembly(string assemblyName)
        {
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(AppContext.BaseDirectory + $"{assemblyName}.dll");
            return assembly;
        }
    }
}
