using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketSystem.Data;
using TicketSystem.Services;

namespace TicketSystem.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminMailTicketsModel : PageModel
    {
        //Email service binnenhalen
        private EmailReceiver m_emailReceiver;
        private ApplicationDbContext m_context;
        private UserManager<ApplicationUser> m_userManager;
        private readonly IMailManager m_mailmanager;
        private TicketService m_ticketService;

        public string Substring {get; set;}

        public List<Models.MailTicket> Emails { get; set; }

        //iets van het object MailTicket
        public List<Models.MailTicket> MailTickets { get; set; }
        public Models.MailTicket MailTicket { get; set; }

        public int NewMailTickets { get; set; }

        public AdminMailTicketsModel(EmailReceiver emailReceiver, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMailManager mailmanager, TicketService ticketService)
        {
            m_emailReceiver = emailReceiver;
            m_context = context;
            m_userManager = userManager;
            m_mailmanager = mailmanager;
            m_ticketService = ticketService;
        }
        public void OnGet()
        {
            //Emails = m_emailReceiver.DownloadMessages();

            MailTickets = m_emailReceiver.GetMailTickets();

            //NewMailTickets = m_emailReceiver.NewMailTickets();
            //CheckForMailAndRequestor();
        }
        public IActionResult OnPostFetch()
        {
            Emails = m_emailReceiver.DownloadMessages();
            MailTickets = m_emailReceiver.GetMailTickets();

            return Page();
        }
        //Delete ticket
        public IActionResult OnPost(int id)
        {
            MailTicket = m_emailReceiver.DeleteMailTicket(id);

            return RedirectToPage("../Admin/AdminMailTickets");

        }

        //check if requestor is already in DB
        //public void CheckForMailAndRequestor()
        //{
        //    using (var client = new ImapClient(/*new ProtocolLogger("imap.log")*/))
        //    {
        //        //connect to imap office 365
        //        client.Connect("outlook.office365.com", 993, SecureSocketOptions.SslOnConnect);
        //        // Note: since we don't have an OAuth2 token, disable the XOAUTH2 authentication mechanism.
        //        client.AuthenticationMechanisms.Remove("XOAUTH2");
        //        client.Authenticate("nathan.aertgeerts@intation.eu" + @"\support@intation.eu", "Y378BdBD");
        //        client.Inbox.Open(FolderAccess.ReadWrite);
        //        var uids = client.Inbox.Search(SearchQuery.All);

        //        foreach (var uid in uids)
        //        {
        //            var message = client.Inbox.GetMessage(uid);
        //            //als de user in de database zit dan kijken we eerst of de subject structuur goed is? 
        //            // indien niet dan sturen we hem een reply om naar het web portaal om een ticket aan te maken
        //            if (m_userManager.FindByEmailAsync(message.From.Mailboxes.FirstOrDefault().Address) != null)
        //            {
        //                //check if #INT-ID contains in subject if so substract ID
        //                if (message.Subject.Contains("#INT-"))
        //                {
        //                    //get position of #INT-01291
        //                    int positionOfINT = message.Subject.IndexOf("#INT-") + 5;
        //                    //new string after #INT-
        //                    var newString = message.Subject.Substring(positionOfINT);

        //                    //doorloop de nieuwe string en zoek de eerste plaats waar ons ID breekt
        //                    for (int ctr = 0; ctr < newString.Length; ctr++)
        //                    {
        //                        if (Char.IsLetter(newString[ctr]) || Char.IsWhiteSpace(newString[ctr]))
        //                        {
        //                            Substring = newString.Substring(0,ctr);
        //                            ctr += newString.Length;
        //                        }
        //                        else
        //                        {
        //                            Substring = newString;
        //                        }
        //                    }

        //                    //Regex on digits of that string

        //                    string subjectIsId = Regex.Match(Substring, @"\d+").Value;
        //                    if (subjectIsId != "")
        //                    {
        //                        //controleren of het subject de afgesproken vorm heeft => Bestaans ticket ID + requestor van het ticket = de zender van de mail?
        //                        if (m_ticketService.GetTicketById(Convert.ToInt32(subjectIsId)) != null && m_ticketService.GetTicketById(Convert.ToInt32(subjectIsId)).TicketRequestor == message.From.Mailboxes.FirstOrDefault().Address)
        //                        {
        //                            //Add reply to ticket (send mail, ticket updated)
        //                            m_ticketService.AddReply(message.From.Mailboxes.FirstOrDefault().Address, DateTime.Now, message.HtmlBody, Convert.ToInt32(subjectIsId));


        //                            var link = Url.TicketLink(Request.Scheme);
        //                            m_mailmanager.SendEmailTicketUpdate(message.From.Mailboxes.FirstOrDefault().Address, link, Convert.ToInt32(subjectIsId));

        //                        }
        //                        else
        //                        {
        //                            //create ticket on portal first(send mail)
        //                            var link = Url.CreateFirst(Request.Scheme);
        //                            m_mailmanager.SendEmailCreateFirst(message.From.Mailboxes.FirstOrDefault().Address, link);

        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    //just throw email if there is no ID in it 
        //                    var link = Url.RegisterFirst(Request.Scheme);
        //                    m_mailmanager.SendEmailQuestion(message.From.Mailboxes.FirstOrDefault().Address, link);
        //                }
        //            }
        //            else
        //            {
        //                // send mail to requestor to register on the portal first an create a ticket there (send mail)
        //                var link = Url.RegisterFirst(Request.Scheme);
        //                m_mailmanager.SendEmailRegisterFirst(message.From.Mailboxes.FirstOrDefault().Address, link);
        //            }

        //            client.Inbox.AddFlags(uid, MessageFlags.Deleted, true);
        //            client.Inbox.Expunge();

        //        }
        //    }
        //}
    }
}