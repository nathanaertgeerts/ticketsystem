using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketSystem.Data;

namespace TicketSystem.Pages.DMS
{
    public class DocumentsModel : PageModel
    {
        public List<Models.DMS> Docs { get; set; }
        public List<String> FilePaths { get; set; }
        
        public string DirectoryPath { get; set; }

        public class InputModel
        {
            public string fileName { get; set; }
            public string DirectoryP { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }

        private ApplicationDbContext m_context;
        public DocumentsModel(ApplicationDbContext context)
        {
            m_context = context;
        }
        public void OnGet(string id)
        {
            DirectoryPath = id;

            string[] filesindirectory = Directory.GetFiles(DirectoryPath);

            FilePaths = filesindirectory.ToList();


        }

        public FileResult OnPostDownload()
        {
            var fileName = Input.fileName;
            var DirectoryPath = Input.DirectoryP;
            var filePath = DirectoryPath + "/" + fileName;
            var fileExists = System.IO.File.Exists(filePath);
            var mimeType = System.Net.Mime.MediaTypeNames.Application.Octet;
            return PhysicalFile(filePath, mimeType, fileName);
        }
    }
}