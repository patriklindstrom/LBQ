using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using LBQ.Katana.Mocks;
using LBQ.Katana.Model;
using LBQ.Katana.Repo;
using Microsoft.Owin.Hosting;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;
using Owin;

namespace LBQ.Katana
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    public static class Global_Const
    {
        public static string EVENTLOGTYPE="EventLog";
        public static string EVENTLOGTITLE = "Filterered Eventslogs";
        public const string SOURCE = "application";
        public const string DATE_FORMAT = "yyyyMMddHHmmss";
        public const string DATE_FORMAT_STR = "{0:yyyyMMddHHmmss}";
        public const int MAXGETROWS = 5000;
        public const int TIMEOUT_S = 2;
    }
    internal class Program
    {
        private static void Main(string[] args)
        {
            string port = "8080";
            if (args.Length >0)
            {
                port = args[0] ?? "8080";
            }
            
            string uri = "http://localhost:" + port;
            Console.WriteLine("Lets Start listen to this address: " + uri);
            using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine("Now we are listening to : " + uri );
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
            container.Register<ISettingsProvider, SettingsProvider>().AsSingleton();  
            container.Register<ICacheLayer, CacheLayer>().AsSingleton();
            container.Register<IEventRecordTimeSpanSearcher, EventRecordTimeSpanSearcher>();
           // container.Register<IEventRecordTimeSpanSearcher, MockEventRecordTimeSpanSearcher>(); 
            container.Register<ILogFilterRepo, EventLogFilterRepo>();
            //container.Register<ILogFilter,MockEventLogFilter>();
           // container.Register<ILogFilterRepo, MockEventLogFilterRepo>();
           container.Register<ILogFilter, EventLogFilter>();
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


