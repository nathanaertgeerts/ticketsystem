using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using TicketSystem.Models;
using TicketSystem.Services;
using TicketSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TicketSystem.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminExactOnlineModel : PageModel
    {
        private ExactOnlineService m_exactOnlinService;
        private ApplicationDbContext m_context;

        public AdminExactOnlineModel(ExactOnlineService exactOnlineService, ApplicationDbContext context)
        {
            m_exactOnlinService = exactOnlineService;
            m_context = context;
        }
        public string Code { get; set; }
        public void OnGet(string code)
        {

            Code = code;

            if (Code != null)
            {
                GetAPI(Code);
            }
        }
        public IActionResult OnPostOauth()
        {
            return Redirect("https://start.exactonline.be/api/oauth2/auth?client_id={208b2aab-272c-4123-a35d-fc872079bceb}&redirect_uri=http://192.168.5.20:5001/Admin/AdminExactOnline&response_type=code");
        }
        public void GetAPI(string Code)
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "code", Code },
                    { "redirect_uri", "http://192.168.5.20:5001/Admin/AdminExactOnline" },
                    { "grant_type", "authorization_code" },
                    { "client_id", "{208b2aab-272c-4123-a35d-fc872079bceb}" },
                    { "client_secret", "FcjRzukOQvEf" }
                };
                var content = new FormUrlEncodedContent(values);

                var method = new HttpMethod("POST");
                var request = new HttpRequestMessage(method, "https://start.exactonline.be/api/oauth2/token") { Content = content };
                var response = client.SendAsync(request).Result;

                //if the response is successfull, set the result to the workitem object
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;

                    var rawResponse = JsonConvert.DeserializeObject<ExactToken>(result);

                    var ACCESSTOKEN = rawResponse.access_token;
                    var RefreshToken = rawResponse.refresh_token;
                    var TokenType = rawResponse.token_type;

                    GetDivision(ACCESSTOKEN, TokenType);

                }
            }
        }
        public void GetDivision(string Token, string Type)
        {

            using (var client = new HttpClient())
            {
                //set our headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var method = new HttpMethod("GET");
                var request = new HttpRequestMessage(method, "https://start.exactonline.be/api/v1/current/Me?$select=CurrentDivision");
                var response = client.SendAsync(request).Result;

                //if the response is successfull, set the result to the workitem object
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;

                    var rawResponse = JsonConvert.DeserializeObject<ExactDivision>(result);

                    foreach (var item in rawResponse.d.results)
                    {
                        var division = item.CurrentDivision;
                        GetAccountGUID(Token, division);
                    }

                }
            }
        }
        public void GetAccountGUID(string Token, int Division)
        {
            using (var client = new HttpClient())
            {
                //set our headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var method = new HttpMethod("GET");
                var request = new HttpRequestMessage(method, "https://start.exactonline.be/api/v1/" + Division + "/crm/Accounts?$filter=Status eq'C'");

                //Working API calls
                //Juiste !!!
                //crm/Accounts?$filter=Status eq'C'


                ///api/v1/{division}/crm/Contacts?$filter=ID eq guid'00000000-0000-0000-0000-000000000000'&$select=Account,AccountIsCustomer = lijst nog te groot, geeft klanten contact personen
                ///crm/Contacts?$filter=AccountIsCustomer eq true = contacten is customer geeft alle contacten van bedrijf terug
                ///crm/Accounts?$select=Type = geeft type A, accounts terug
                ///crm/Accounts?$select=AccountManager
                ///////////////////////////////////
                ///crm/Contacts?$filter=IsMainContact eq true and AccountIsCustomer eq true === te weinig data??? 
                //////////////////////////////////

                var response = client.SendAsync(request).Result;

                //if the response is successfull, set the result to the workitem object
                if (response.IsSuccessStatusCode)
                {
                    //als we tot hier geraakt zijn, gaan we ervan uit dat we alle contacten en companies hebben kunnen binnenhalen, 
                    //dus hier verwijderen we de contacten lijst om nadien een clean add te doen voor contacten per bedrijf.
                    if (m_context.Contacts.ToList() != null)
                    {
                        var OLDContactList = m_context.Contacts.ToList();
                        foreach (var contact in OLDContactList)
                        {
                            m_context.Contacts.Remove(contact);
                            m_context.SaveChanges();
                        }
                        
                    }



                    var result = response.Content.ReadAsStringAsync().Result;
                    var rawResponse = JsonConvert.DeserializeObject<ExactCompany>(result);

                    var list = m_context.Company.ToList();
                    foreach (var Company in rawResponse.d.results)
                    {

                        var result2 = list.FirstOrDefault(x => x.Name == Company.Name);
                        if (result2 == null)
                        {
                            m_exactOnlinService.AddCompany(Company.Name, Company.AddressLine1, Company.Email, Company.Code, Company.Website, Company.Phone, Company.Postcode, Company.CountryName, Company.City);
                        }
                        else
                        {
                            var updateCompany = m_context.Company.FirstOrDefault(x => x.Name == Company.Name);

                            updateCompany.Phone = Company.Phone;
                            updateCompany.Address = Company.AddressLine1;
                            updateCompany.Email = Company.Email;
                            updateCompany.Code = Company.Code;
                            updateCompany.Postalcode = Company.Postcode;
                            updateCompany.Country = Company.CountryName;
                            updateCompany.City = Company.City;

                            m_context.SaveChanges();

                        }
                        GetMainContact(Token, Division, Company.MainContact, Company.Name);
                    }
                }
            }
        }
        public void GetMainContact(string Token, int Division, string MainContact, string CompanyName)
        {
            using (var client = new HttpClient())
            {
                //set our headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var method = new HttpMethod("GET");
                var request = new HttpRequestMessage(method, "https://start.exactonline.be/api/v1/" + Division + "/crm/Contacts?$filter=ID eq guid'" + MainContact + "'");
                var response = client.SendAsync(request).Result;

                //if the response is successfull, set the result to the workitem object
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;

                    var rawResponse = JsonConvert.DeserializeObject<ExactMainContact>(result);

                    foreach (var item in rawResponse.d.results)
                    {
                        m_exactOnlinService.AddContact(CompanyName, item.FullName, item.FirstName, item.LastName, item.Email, item.BusinessEmail, item.Phone, item.BusinessPhone, item.Mobile, item.BusinessMobile, item.JobTitleDescription);

                    }
                }
            }
        }
    }
}