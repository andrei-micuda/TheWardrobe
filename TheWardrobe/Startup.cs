using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using TheWardrobe.Models;

[assembly: OwinStartupAttribute(typeof(TheWardrobe.Startup))]

namespace TheWardrobe
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateAdminUserAndApplicationRoles();
        }

        private void CreateAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new
            RoleStore<IdentityRole>(context));

            var UserManager = new UserManager<ApplicationUser>(new
            UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "admin@gmail.com";
                user.Email = "admin@gmail.com";

                var adminCreated = UserManager.Create(user, "!1Admin");
                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Admin");
                }
            }

            if (!roleManager.RoleExists("Editor"))
            {
                var role = new IdentityRole();
                role.Name = "Editor";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "editor@gmail.com";
                user.Email = "editor@gmail.com";

                var editorCreated = UserManager.Create(user, "!1Editor");
                if (editorCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Editor");
                }
            }

            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
        }
    }
}