using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Models;
using TicketSystem.Services;

namespace TicketSystem.Pages.DMS
{
    public class DMSModel : PageModel
    {
        public class DMS
        {
            public int Id { get; set; }
            public string DirectoryPath { get; set; }
        }
        [BindProperty]
        public List<Models.DMS> Docs { get; set; }

        public List<Models.DMS> AllDocs { get; set; }
        public List<Models.DMS> NoneMaps { get; set; }
        public List<Models.DMS> BronzeMaps { get; set; }
        public List<Models.DMS> SilverMaps { get; set; }
        public List<Models.DMS> GoldMaps { get; set; }

        public class InputModel
        {
            public int Id { get; set; }
            public string FolderPath { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }

        private ApplicationDbContext m_context;
        private UserManager<ApplicationUser> m_userManager;
        private UserInCompanyService m_userInCompanyService;

        private readonly IHostingEnvironment _hostingEnvironment;

        //public List<Models.DMS> DocumentList { get; set; }
        public ApplicationUser UserDetails { get; set; }
        public Companies CompanyDetails { get; set; }



        public DMSModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, UserInCompanyService userInCompanyService, IHostingEnvironment hostingEnvironment)
        {
            m_context = context;
            m_userManager = userManager;
            m_userInCompanyService = userInCompanyService;
            _hostingEnvironment = hostingEnvironment;
        }
        public void OnGet()
        {
            JuisteLijst();

            UserDetails = m_userManager.FindByNameAsync(User.Identity.Name).Result;
            if (m_userInCompanyService.GetUsersInCompany().FirstOrDefault(x => x.UserId == UserDetails.Id) == null)
            {
                Page();
            }
            else
            {
                CompanyDetails = GetCompanyDetails(UserDetails.Id);
                if (CompanyDetails.ContractLevel == ContractLevel.None)
                {
                    Docs = NoneMaps;
                }
                if (CompanyDetails.ContractLevel == ContractLevel.Bronze)
                {
                    Docs = BronzeMaps;
                }
                if (CompanyDetails.ContractLevel == ContractLevel.Silver)
                {
                    Docs = SilverMaps;
                }
                if (CompanyDetails.ContractLevel == ContractLevel.Gold)
                {
                    Docs = GoldMaps;
                }


            }
        }

        public void JuisteLijst()
        {
            NoneMaps = m_context.DMS.Where(x => x.DocumentLevel == DocumentLevel.None).ToList();

            BronzeMaps = m_context.DMS.Where(x => x.DocumentLevel == DocumentLevel.None ||
                                                x.DocumentLevel == DocumentLevel.Bronze).ToList();

            SilverMaps = m_context.DMS.Where(x => x.DocumentLevel == DocumentLevel.None ||
                                                x.DocumentLevel == DocumentLevel.Bronze ||
                                                x.DocumentLevel == DocumentLevel.Silver).ToList();

            GoldMaps = m_context.DMS.Where(x => x.DocumentLevel == DocumentLevel.None ||
                                                x.DocumentLevel == DocumentLevel.Bronze ||
                                                x.DocumentLevel == DocumentLevel.Silver ||
                                                x.DocumentLevel == DocumentLevel.Gold).ToList();
        }
        //company details
        public Companies GetCompanyDetails(string userId)
        {
            var CompanyFromUser = m_userInCompanyService.GetUsersInCompany().FirstOrDefault(x => x.UserId == userId);
            var company = m_context.Company.Include(x => x.Contacten).FirstOrDefault(x => x.Id == CompanyFromUser.CompanyId);
            return company;
        }

        //Download Map as a Zip file
        public ActionResult OnPostDownload(string path)
        {
            string contentRootPath = _hostingEnvironment.ContentRootPath;

           
            if (System.IO.File.Exists(contentRootPath + "/support.zip"))
            {
                System.IO.File.Delete(contentRootPath + "/support.zip");
            }
            //Create from directory => input.directorypath !!!
            ZipFile.CreateFromDirectory(path, "support.zip");

            var mimeType = System.Net.Mime.MediaTypeNames.Application.Octet;
            return PhysicalFile(contentRootPath + "/support.zip", mimeType, "support.zip");
        }
    }
}