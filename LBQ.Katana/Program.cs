using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using LBQ.Katana.MockupModels;
using LBQ.Katana.Model;
using Microsoft.Owin.Hosting;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;
using Owin;

namespace LBQ.Katana
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    internal class Program
    {
        private static void Main(string[] args)
        {
            string uri = "http://localhost:8080";
            using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine("Lets Start!");
                Console.ReadKey();
                Console.WriteLine("Stopping!");
            }
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.Use(async (environment, next) =>
            //{
            //    foreach (var pair in environment.Environment)
            //    {
            //        Console.WriteLine("{0}:{1}", pair.Key, pair.Value);
            //    }
            //    await next();
            //});
            app.Use(async (environment, next) =>
            {
                Console.WriteLine("Requesting => " + environment.Request.Path);
                await next();
                Console.WriteLine("Response => " + environment.Response.StatusCode);
            }
                );
            app.MapSignalR();
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("bugs", "api/{Controller}");
            app.UseWebApi(config);

            app.UseNancy(options =>
            {
                options.Bootstrapper = new CustomBootstrapper();
            });
            //app.UseHelloWorld();
            //  app.UseWelcomePage();
            //app.Run(ctx =>
            //{
            //    return ctx.Response.WriteAsync("Hello World");
            //}
            //    );
        }
    }
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            string[] jsStrings = new[] {"js"};
            string[] contentStrings = new[] { "css", "png","jpg" };
            Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("scripts", "Scripts", jsStrings));
            Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("content", "Content", contentStrings));
           
        }
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            container.Register<ILogFilter,MockEventLogFilter>();
        }
    }

    public static class AppBuildExtensions
    {
        public static void UseHelloWorld(this IAppBuilder app)
        {
            app.Use<HelloWorldComponent>();
        }
    }
    


public class HelloWorldComponent
    {
        private AppFunc _next;
        public HelloWorldComponent( AppFunc next)
        {
            _next = next;
        }
        public  Task Invoke(IDictionary<string, object> environment)
        {
           // await _next(environment);
            var response = environment["owin.ResponseBody"] as Stream;
            using (var writer = new StreamWriter(response))
            {
                return writer.WriteAsync("Hello Where is my Katana!!");
            }

        }
    }
}


