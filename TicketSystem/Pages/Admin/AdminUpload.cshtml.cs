using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.IO;
using System.Xml.Linq;
using TicketSystem.Models;
using System.Xml.Serialization;
using Newtonsoft.Json;
using TicketSystem.Services;
using TicketSystem.Extensions;
using TicketSystem.Data;
using Microsoft.AspNetCore.Authorization;

namespace TicketSystem.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminUploadModel : PageModel
    {
        public class InputModel
        {

        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public IFormFile InputFile { get; set; }


        private ExactOnlineService m_exactOnlineService;
        private ApplicationDbContext m_context;
        public AdminUploadModel(ExactOnlineService exactOnlineService, ApplicationDbContext context)
        {
            m_exactOnlineService = exactOnlineService;
            m_context = context;
        }
        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (InputFile == null)
            {
                return Page();
            }
            if (!InputFile.FileName.EndsWith(".xml"))
            {
                return Page();
            }
            else
            {
                var data = InputFile.ToByteArray();
                Stream stream = new MemoryStream(data);

                XmlDocument doc = new XmlDocument();
                doc.Load(stream);

                string json = JsonConvert.SerializeXmlNode(doc);

                var rawResponse = JsonConvert.DeserializeObject<Rootobject>(json);

                var test = rawResponse.eExact.Accounts.Account.ToList();

                var list = m_context.Company.ToList();

                foreach (var Company in test)
                {

                    var result = list.FirstOrDefault(x => x.Name == Company.Name);
                    if (result == null)
                    {
                        m_exactOnlineService.AddCompany(Company.Name, Company.Address.AddressLine1, Company.Email, Company.code, Company.HomePage, Company.Phone, Company.Address.PostalCode, Company.Address.Country, Company.Address.City);
                    }
                    else
                    {
                        var updateCompany = m_context.Company.FirstOrDefault(x => x.Name == Company.Name);

                        updateCompany.Phone = Company.Phone;
                        updateCompany.Address = Company.Address.AddressLine1;
                        updateCompany.Email = Company.Email;
                        updateCompany.Code = Company.code;
                        updateCompany.Postalcode = Company.Address.PostalCode;
                        m_context.SaveChanges();
                    }



                }

                return RedirectToPage("../Admin/AdminCompanies");
            }

        }

    }
}