namespace VeriDocCertificate.CofoundaryCMS.App_Start
{
    public class RouteRegistration : IRouteRegistration
    {
        public void RegisterRoutes(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder.MapControllerRoute(
                "Account",
                "{controller}/{action}/{id?}",
                new { controller = "Home", action = "Index" }
            );
        }
    }
}
