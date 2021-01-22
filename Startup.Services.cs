using Communication.EventDataService;
using TestWunderMobilityCheckout.Actions.ProcessEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestWunderMobilityCheckout
{
    public partial class Startup
    {
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"> Services </param>
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<HostedService>();

            services.AddDbContext<TestWunderMobilityCheckoutDBContext>(
                options =>
                options.UseSqlServer(Configuration["Database:ConnectionAuth"]),
                ServiceLifetime.Scoped);

            services.AddScoped<IScopedProcessEventsService, ScopedProcessEventsService>();
            services.AddScoped<IEventDataFactory<TestWunderMobilityCheckoutDBContext>, EventDataFactory<TestWunderMobilityCheckoutDBContext>>();
            services.AddScoped<IEventDataAction<TestWunderMobilityCheckoutDBContext>, EventDataAction<TestWunderMobilityCheckoutDBContext>>();
            services.AddScoped<IProcessEventsAct, ProcessEventsAct>();

            services.AddSingleton(Configuration);
        }
    }
}
