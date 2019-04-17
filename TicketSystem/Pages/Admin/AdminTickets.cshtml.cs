using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketSystem.Data;
using TicketSystem.Services;

namespace TicketSystem.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminTicketsModel : PageModel
    {
        public class Categories
        {
            public int Id { get; set; }
            public string CategoryName { get; set; }
        }

        [BindProperty]
        public List<Models.Categories> CategoryList { get; set; }

        public class InputModel
        {
            public string CategoryName { get; set; }
            public int Id { get; set; }

        }
        [BindProperty]
        public InputModel Input { get; set; }


        public Models.Ticket Ticket { get; set; }

        //Ticket service binnenhalen
        private TicketService m_ticketService;
        private ApplicationDbContext m_context;
        public AdminTicketsModel(TicketService ticketService, ApplicationDbContext context)
        {
            m_ticketService = ticketService;
            m_context = context;
        }

        //onGet alle tickets binnenhalen
        public void OnGet()
        {
            CategoryList = m_ticketService.GetCategories();
        }

        //Add Category
        public IActionResult OnPost()
        {


            var Category = new Models.Categories()
            {
                CategoryName = Input.CategoryName
            };

            m_ticketService.AddCategory(Category);
            return RedirectToPage("../Admin/AdminTickets");

        }
        //Delete Category
        public IActionResult OnPostDelete()
        {

            m_ticketService.DeleteCategory(Input.Id);
            return RedirectToPage("../Admin/AdminTickets");

        }
        //Update Category
        public IActionResult OnPostUpdate()
        {

            foreach (var Category in CategoryList)
            {
                var newCategory = m_context.Categories.FirstOrDefault(x => x.Id == Category.Id);

                newCategory.CategoryName = Category.CategoryName;

                m_context.Categories.Update(newCategory);
                m_context.SaveChanges();
            }

            return RedirectToPage("../Admin/AdminTickets");
        }
    }
}