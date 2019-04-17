using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketSystem.Data;
using TicketSystem.Models;
using TicketSystem.Services;

namespace TicketSystem.Pages.Admin
{
    public class AdminDocumentSettingsModel : PageModel
    {
        public class DMS
        {
            public int Id { get; set; }
            public Models.DocumentLevel documentLevel { get; set; }
        }

        [BindProperty]
        public List<Models.DMS> Docs { get; set; }
        public class InputModel
        {
            public int Id { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }


        public Models.TargetDirectory TargetDirectory { get; set; }
        public List<String> FilePaths { get; set; }

        public List<Models.UserInCompany> UserInCompanyList { get; set; }

        public List<String> EmailAddresses { get; set; }
        public Models.DocumentLevel OldLevel { get; set; }
        public int LevelNone { get; set; }
        public int LevelBronze { get; set; }
        public int LevelSilver { get; set; }
        public int LevelGold { get; set; }

        private ApplicationDbContext m_context;
        private DocumentManager m_documentManager;
        private UserInCompanyService m_userInCompanyService;
        private UserManager<ApplicationUser> m_userManager;

        public List<String> EmailsToSend { get; set; }
        //Email sender interface
        private readonly IMailManager m_mailmanager;
        public AdminDocumentSettingsModel(ApplicationDbContext context, DocumentManager documentManager, IMailManager mailmanager, UserInCompanyService userInCompanyService, UserManager<ApplicationUser> userManager)
        {
            m_context = context;
            m_documentManager = documentManager;
            m_mailmanager = mailmanager;
            m_userInCompanyService = userInCompanyService;
            m_userManager = userManager;
        }
        public void OnGet(int Id)
        {
            TargetDirectory = m_documentManager.GetTargetDirectoryByID(Id);

            string[] filesindirectory = Directory.GetDirectories(TargetDirectory.DirectoryPath);

            FilePaths = filesindirectory.ToList();
            Docs = m_context.DMS.ToList();
            //for elke filepath kijken of deze al bestaat, anders toevoegen aan DB
            foreach (var item in FilePaths)
            {
                var newPath = Docs.Find(x => x.FilePath == item);
                if (newPath == null)
                {
                    TargetDirectory.FilePaths.Add(new Models.DMS
                    {
                        FilePath = item,
                        DocumentLevel = Models.DocumentLevel.Invisible
                    });
                    //add new => document always invisible
                    m_context.SaveChanges();
                }
                else
                {
                    //path already in DataBase => content will still be available
                }
            }


            Docs = TargetDirectory.FilePaths.ToList();
        }

        public IActionResult OnPostUpdate()
        {
            //Update de Document levels van de bestaande paths
            foreach (var item in Docs)
            {
                var Document = m_context.DMS.FirstOrDefault(x => x.Id == item.Id);
                OldLevel = Document.DocumentLevel;

                var newLevel = m_context.DMS.FirstOrDefault(x => x.Id == item.Id);
                newLevel.DocumentLevel = item.DocumentLevel;

                m_context.DMS.Update(newLevel);
                m_context.SaveChanges();

                //controleren op nieuwe document wijzigingen??
                if (newLevel.DocumentLevel != OldLevel)
                {
                    if (newLevel.DocumentLevel == DocumentLevel.None)
                    {
                        LevelNone = LevelNone + 1;
                    }
                    if (newLevel.DocumentLevel == DocumentLevel.Bronze)
                    {
                        LevelBronze = LevelBronze + 1;
                    }
                    if (newLevel.DocumentLevel == DocumentLevel.Silver)
                    {
                        LevelSilver = LevelSilver + 1;
                    }
                    if (newLevel.DocumentLevel == DocumentLevel.Gold)
                    {
                        LevelGold = LevelGold + 1;
                    }
                }
            }

            //stuur mail naar contacten uit bedrijf waarop verandering document van toepassing is.
            CheckfornewItems(LevelNone, LevelBronze, LevelSilver, LevelGold);

            return RedirectToPage();
        }

        public void CheckfornewItems(int none, int bronze, int silver, int gold)
        {
            //Als document levels geupdate worden moeten we mails sturen naar users in companies die hun meldingen aan hebben staan
            var link = Url.Documents(Request.Scheme);
            //alle gebruikers die gekoppeld zijn aan een bedrijf
            UserInCompanyList = m_userInCompanyService.GetUsersInCompany();
            foreach (var GebruikersInBedrijf in UserInCompanyList)
            {
                //user object oprvagen voor elke user in bedrijf
                var UserObject = m_userManager.FindByIdAsync(GebruikersInBedrijf.UserId).Result;
                var CompanyObject = m_context.Company.FirstOrDefault(x => x.Id == GebruikersInBedrijf.CompanyId);

                if (CompanyObject.ContractLevel == ContractLevel.None)
                {
                    if (none != 0)
                    {
                        m_mailmanager.SendEmailNewDocument(UserObject.Email, link);
                    }
                }
                if (CompanyObject.ContractLevel == ContractLevel.Bronze)
                {
                    if (none != 0 || bronze != 0)
                    {
                        m_mailmanager.SendEmailNewDocument(UserObject.Email, link);
                    }
                }
                if (CompanyObject.ContractLevel == ContractLevel.Silver)
                {
                    if (none != 0 || bronze != 0 || silver != 0)
                    {
                        m_mailmanager.SendEmailNewDocument(UserObject.Email, link);
                    }
                }
                if (CompanyObject.ContractLevel == ContractLevel.Gold)
                {
                    if (none != 0 || bronze != 0 || silver != 0 || gold != 0)
                    {
                        m_mailmanager.SendEmailNewDocument(UserObject.Email, link);
                    }
                }

            }
        }
    }
}