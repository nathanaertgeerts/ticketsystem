using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;
using TicketSystem.Data;
using TicketSystem.Models;
using TicketSystem.Services;

namespace TicketSystem.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminDMSModel : PageModel
    {
        public class InputModel
        {
            public string NewPath { get; set; }
            public int Id { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public List<TargetDirectory> targetDirectory { get; set; }
        public List<string> DirectoryPaths { get; set; }

        private ApplicationDbContext m_context;
        private DocumentManager m_documentManager;
        public AdminDMSModel(ApplicationDbContext context, DocumentManager documentManager)
        {
            m_context = context;
            m_documentManager = documentManager;
        }

        public List<String> FilePaths { get; set; }
        public List<String> FileNames { get; set; }
        public List<Models.DMS> DocumentList { get; set; }
        public void OnGet()
        {
            targetDirectory = m_documentManager.GetTargetDirectories();



            //string[] directoriesindirectory = Directory.GetDirectories("C:/Users/VSTS/INTATION/");

            //DirectoryPaths = directoriesindirectory.ToList();
            ////for elke filepath kijken of deze al bestaat, anders toevoegen aan DB
            //foreach (var item in DirectoryPaths)
            //{
            //    var newPath = targetDirectory.Find(x => x.DirectoryPath == item);
            //    if (newPath == null)
            //    {
            //        m_context.TargetDirectory.Add(new TargetDirectory
            //        {
            //            DirectoryPath = item,
            //        });
            //        //add new => document always invisible
            //        m_context.SaveChanges();
            //    }
            //    else
            //    {
            //        //path already in DataBase => content will still be available
            //    }
            //}




        }

        public IActionResult OnPostDeleteDirectory(int id)
        {
            m_documentManager.DeleteTargetDirectory(id);
            return RedirectToPage();
        }
        public IActionResult OnPostAdd()
        {
            //Save Path van directory voor documenten

            m_context.TargetDirectory.Add(new TargetDirectory
            {
                DirectoryPath = Input.NewPath
            });
            m_context.SaveChanges();
            return RedirectToPage("/Admin/AdminDMS");

        }
    }
}