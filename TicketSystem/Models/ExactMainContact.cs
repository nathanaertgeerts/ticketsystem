using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{

    public class ExactMainContact
    {
        public D3 d { get; set; }
    }

    public class D3
    {
        public Result3[] results { get; set; }
    }

    public class Result3
    {
        public __Metadata3 __metadata { get; set; }
        public string Account { get; set; }
        public bool AccountIsCustomer { get; set; }
        public bool AccountIsSupplier { get; set; }
        public string AccountMainContact { get; set; }
        public string AccountName { get; set; }
        public object AddressLine2 { get; set; }
        public object AddressStreet { get; set; }
        public object AddressStreetNumber { get; set; }
        public object AddressStreetNumberSuffix { get; set; }
        public object BirthDate { get; set; }
        public string BirthName { get; set; }
        public object BirthNamePrefix { get; set; }
        public object BirthPlace { get; set; }
        public string BusinessEmail { get; set; }
        public object BusinessFax { get; set; }
        public string BusinessMobile { get; set; }
        public string BusinessPhone { get; set; }
        public object BusinessPhoneExtension { get; set; }
        public object City { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public string Creator { get; set; }
        public string CreatorFullName { get; set; }
        public int Division { get; set; }
        public string Email { get; set; }
        public object EndDate { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public int HID { get; set; }
        public string ID { get; set; }
        public object IdentificationDate { get; set; }
        public object IdentificationUser { get; set; }
        public object IdentificationDocument { get; set; }
        public object Initials { get; set; }
        public bool IsMailingExcluded { get; set; }
        public bool IsMainContact { get; set; }
        public string JobTitleDescription { get; set; }
        public string Language { get; set; }
        public string LastName { get; set; }
        public object MarketingNotes { get; set; }
        public object MiddleName { get; set; }
        public string Mobile { get; set; }
        public DateTime Modified { get; set; }
        public string Modifier { get; set; }
        public string ModifierFullName { get; set; }
        public object Nationality { get; set; }
        public object Notes { get; set; }
        public object PartnerName { get; set; }
        public object PartnerNamePrefix { get; set; }
        public string Person { get; set; }
        public string Phone { get; set; }
        public object PhoneExtension { get; set; }
        public object Picture { get; set; }
        public object PictureName { get; set; }
        public string PictureUrl { get; set; }
        public string PictureThumbnailUrl { get; set; }
        public object Postcode { get; set; }
        public object SocialSecurityNumber { get; set; }
        public DateTime StartDate { get; set; }
        public object State { get; set; }
        public object Title { get; set; }
        public object AllowMailing { get; set; }
        public object LeadPurpose { get; set; }
        public object LeadSource { get; set; }
    }

    public class __Metadata3
    {
        public string uri { get; set; }
        public string type { get; set; }
    }

}
