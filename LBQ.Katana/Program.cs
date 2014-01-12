using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using LBQ.Katana.Mocks;
using LBQ.Katana.Model;
using LBQ.Katana.Repo;
using Microsoft.Owin.Hosting;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Owin;
using Nancy.TinyIoc;
using Owin;

namespace LBQ.Katana
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    public static class Global_Const
    {
        public const string EVENTLOGTYPE = "EventLog";
        public const string EVENTLOGTITLE = "Filterered Eventslogs";
        public const int FIRST_TIME = 5000;
        public const int NEXT_TIME = 1 * 15 * 1000;
        //public static List<string> TBL_HEADERS = new List<string>(){"Title","LastRefreshedTime","LogRows","FromDateTime","ToDateTime"};
        public const string SOURCE = "application";
        public const string DATE_FORMAT = "yyyyMMddHHmmss";
        public const string DATE_FORMAT_STR = "{0:yyyyMMddHHmmss}";
        public const int MAXGETROWS = 5000;
        public const int TIMEOUT_S = 2;
        public const String DEFAULT_SQL_STATEMENT =
    @"SELECT  msdb.dbo.agent_datetime(h.run_date,h.run_time) as run_time,run_duration,run_status,h.server,s.name as jobname,  h.step_name,  h.message
        FROM  msdb..sysjobs as s
          join  msdb..sysjobhistory as h ON h.job_id = s.job_id
          where  msdb.dbo.agent_datetime(h.run_date,h.run_time)between @FromTime AND @ToTime
          Order by  h.run_date desc, h.run_time desc; ";
    }
    internal class Program
    {
        
        private static ILogFilterRepo _evFilterR;

        public Program()
        {
            // ToDo why is the default tinyIOC current empty here?
            _evFilterR = SetDepInj(TinyIoCContainer.Current).Resolve<ILogFilterRepo>();
        }
        private static void Main(string[] args)
        {
            string uri = "http://localhost:8080";
            if (args.Length >0)
            {
                uri = args[0] ?? "http://localhost:8080";
            }
            _evFilterR = SetDepInj(TinyIoCContainer.Current).Resolve<ILogFilterRepo>();
            
            Console.WriteLine("Lets Start listen to this address: " + uri);
            using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine("Now we are listening to : " + uri );
                Timer tmr = new Timer(RefreshCache, null, Global_Const.FIRST_TIME, Global_Const.NEXT_TIME);
                Console.ReadKey();
                tmr.Dispose();
                Console.WriteLine("Stopping!");                      
            }
        }
        private static void RefreshCache(object state)
        {
            
            AskForLastHourData(1);
        }

        private static void AskForLastHourData( int lasthours)
        {
            Console.WriteLine("Uppdate cache for last" + 1 + "hour for eventlogs");
            DateTime tTime = DateTime.Now; //DateTime.Parse("2014-01-06 21:45:00");
            DateTime tCDateTime = new DateTime(tTime.Year, tTime.Month, tTime.Day, tTime.Hour, tTime.Minute - tTime.Minute%5, 0);
            DateTime fTime = tCDateTime.AddHours(lasthours*-1);
            var model = _evFilterR.GetData(fromTime: fTime, toTime: tCDateTime);
        }

        /// <summary>
        /// I want to use the same dependency injection for Nancy framework and outside of it
        /// Since I cant hold of the Nancy current IOC call this method from program start and from nancy startup.
        /// </summary>
        /// <param name="tinyIoC"></param>
        /// <returns>the same tinyIOC where I have made my registrations.</returns>
        public static TinyIoCContainer SetDepInj(TinyIoCContainer tinyIoC)
        {
            tinyIoC.Register<ISettingsProvider, SettingsProvider>().AsSingleton();
            tinyIoC.Register<ICacheLayer, CacheLayer>().AsSingleton();
            // container.Register<IEventRecordTimeSpanSearcher, MockEventRecordTimeSpanSearcher>(); 
            tinyIoC.Register<IEventRecordTimeSpanSearcher, EventRecordTimeSpanSearcher>();
            tinyIoC.Register<ILogFilterRepo, EventLogFilterRepo>();
            //container.Register<ILogFilter,MockEventLogFilter>();
            // container.Register<ILogFilterRepo, MockEventLogFilterRepo>();
            tinyIoC.Register<ILogFilter, EventLogFilter>();
            return tinyIoC;
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
                options.PassThroughWhenStatusCodesAre(HttpStatusCode.NotFound);
            });


            app.UseMissingPage();
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
        private ILogFilterRepo _evFilterR;
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
           container= LBQ.Katana.Program.SetDepInj(container);
        }

  
    }

    public static class AppBuildExtensions
    {
        public static void UseMissingPage(this IAppBuilder app)
        {
            app.Use<MissingPageComponent>();
        }
    }
    


public class MissingPageComponent
    {
        private AppFunc _next;
        public MissingPageComponent(AppFunc next)
        {
            _next = next;
        }
        public  Task Invoke(IDictionary<string, object> environment)
        {
           // await _next(environment);
            var response = environment["owin.ResponseBody"] as Stream;
            using (var writer = new StreamWriter(response))
            {
                return writer.WriteAsync("Hello Where is my Katana!!. This page is missing");
            }

        }
    }
}


