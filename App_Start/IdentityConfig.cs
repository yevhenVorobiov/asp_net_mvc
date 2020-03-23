using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace NewHotel.App_Start
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(HotelDbContext.Create);
            app.CreatePerOwinContext<UserManager<Employee>>((options, context) =>
                new UserManager<Employee>(new UserStore<Employee>(context.Get<HotelDbContext>())));
            app.CreatePerOwinContext<RoleManager<IdentityRole>>((options, context) =>
                new RoleManager<IdentityRole>(new MyRoleStore(context.Get<HotelDbContext>())));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }
    }
    public class EmployeeSignInManager : SignInManager<Employee, string>
    {
        public EmployeeSignInManager(UserManager<Employee> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }
        public static EmployeeSignInManager Create(IdentityFactoryOptions<EmployeeSignInManager> options, IOwinContext context)
        {
            return new EmployeeSignInManager(context.GetUserManager<UserManager<Employee>>(), context.Authentication);
        }
    }
    public class MyRoleStore : RoleStore<IdentityRole>
    {
        public MyRoleStore(HotelDbContext context) : base(context)
        { }
    }

    public class MyRoleManager : RoleManager<IdentityRole>
    {
        public MyRoleManager(MyRoleStore roleStore) : base(roleStore) { }
    }
}