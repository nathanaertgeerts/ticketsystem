using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Data
{
    //nog afmaken en toevoegen aan program (database seeden)
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> usermanager)
        {
            context.Database.EnsureCreated();

            //check for roles, other create admin role
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var admin = new IdentityRole { Name = "Admin" };
                var agent = new IdentityRole { Name = "Agent" };
                var customer = new IdentityRole { Name = "Customer" };

                var res = roleManager.CreateAsync(admin).Result;
                var res2 = roleManager.CreateAsync(agent).Result;
                var res3 = roleManager.CreateAsync(customer).Result;
            }


            if (!context.Users.Any(u => u.UserName == "nathan.aertgeerts@intation.eu"))
            {
                var store = new UserStore<ApplicationUser>(context);

                var admin = new ApplicationUser{ UserName = "nathan.aertgeerts@intation.eu", Email = "nathan.aertgeerts@intation.eu",EmailConfirmed = true, Firstname = "Nathan", Lastname ="Aertgeerts"};
                var agent = new ApplicationUser { UserName = "nathan.aertgeerts@student.ap.be", Email = "nathan.aertgeerts@student.ap.be", EmailConfirmed = true, Firstname = "Nathan", Lastname = "Aertgeerts" };
                var customer = new ApplicationUser { UserName = "nathanaertgeerts@hotmail.com", Email = "nathanaertgeerts@hotmail.com", EmailConfirmed = true, Firstname = "Nathan", Lastname = "Aertgeerts" };
                var customer2 = new ApplicationUser { UserName = "nathanaertgeerts@gmail.com", Email = "nathanaertgeerts@gmail.com", EmailConfirmed = true, Firstname = "Nathan", Lastname = "Aertgeerts" };

                var res2 = usermanager.CreateAsync(admin, "Nathan123!").Result;
                var res3 = usermanager.AddToRoleAsync(admin, "Admin").Result;
                var User1 = usermanager.Users.Include(x => x.Notifications).FirstOrDefault(x => x.UserName == admin.UserName);
                User1.Notifications.Add(new Models.Notifications()
                {
                    TicketCreated = true,
                    TicketUpdate = true,
                    NewArticle = true,
                    NewDocument = true
                });

                var res4 = usermanager.CreateAsync(agent, "Nathan123!").Result;
                var res5 = usermanager.AddToRoleAsync(agent, "Agent").Result;
                var User2 = usermanager.Users.Include(x => x.Notifications).FirstOrDefault(x => x.UserName == agent.UserName);
                User2.Notifications.Add(new Models.Notifications()
                {
                    TicketCreated = true,
                    TicketUpdate = true,
                    NewArticle = true,
                    NewDocument = true
                });

                var res6 = usermanager.CreateAsync(customer, "Nathan123!").Result;
                var res7 = usermanager.AddToRoleAsync(customer, "Customer").Result;
                var User3 = usermanager.Users.Include(x => x.Notifications).FirstOrDefault(x => x.UserName == customer.UserName);
                User3.Notifications.Add(new Models.Notifications()
                {
                    TicketCreated = true,
                    TicketUpdate = true,
                    NewArticle = true,
                    NewDocument = true
                });

                var res8 = usermanager.CreateAsync(customer2, "Nathan123!").Result;
                var res9 = usermanager.AddToRoleAsync(customer2, "Customer").Result;
                var User4 = usermanager.Users.Include(x => x.Notifications).FirstOrDefault(x => x.UserName == customer2.UserName);
                User4.Notifications.Add(new Models.Notifications()
                {
                    TicketCreated = true,
                    TicketUpdate = true,
                    NewArticle = true,
                    NewDocument = true
                });

            }

            if (!context.Ticket.Any(u => u.TicketSubject =="First Ticket"))
            {

                var ticket = context.Ticket.Add(new Models.Ticket()
                {
                    TicketSubject = "First Ticket",
                    TicketDetails = "This ticket has been added when DB was Initialized",
                    TicketRequestor = "nathan.aertgeerts@intation.eu",
                    PriorityType = Models.Priority.Normal,
                    StatusType = Models.Status.Open,
                    ProductType = Models.Product.InControl,
                    Category = new Models.Categories() { CategoryName = "First Category" }
                   
                });
            }

            context.SaveChanges();

        }
    }
}
