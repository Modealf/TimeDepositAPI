using Hangfire.Dashboard;

namespace TimeDepositAPI.Services
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow only authenticated admins to access the dashboard
            return httpContext.User.Identity.IsAuthenticated &&
                   httpContext.User.IsInRole("Admin");
        }
    }
}