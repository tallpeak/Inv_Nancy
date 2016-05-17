namespace Inv_Nancy
{
    using Microsoft.AspNet.Builder;
    using Nancy.Owin;
 
    public class Startup
    {
        public static Inventory inv = new Inventory(); 
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(x => x.UseNancy());
        }

        // Entry point for the application.
        public static void Main(string[] args) => Microsoft.AspNet.Hosting.WebApplication.Run<Startup>(args);
    }
}
