using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{


    public class ExactCompany
    {
        public D2 d { get; set; }
    }

    public class D2
    {
        public Result2[] results { get; set; }
    }

    public class Result2
    {
        public __Metadata2 __metadata { get; set; }
        public object Accountant { get; set; }
        public string AccountManager { get; set; }
        public string AccountManagerFullName { get; set; }
        public int? AccountManagerHID { get; set; }
        public object ActivitySector { get; set; }
        public object ActivitySubSector { get; set; }
        public string AddressLine1 { get; set; }
        public object AddressLine2 { get; set; }
        public object AddressLine3 { get; set; }
        public Bankaccounts BankAccounts { get; set; }
        public bool Blocked { get; set; }
        public object BRIN { get; set; }
        public object BusinessType { get; set; }
        public bool CanDropShip { get; set; }
        public object ChamberOfCommerce { get; set; }
        public string City { get; set; }
        public object Classification { get; set; }
        public object Classification1 { get; set; }
        public object Classification2 { get; set; }
        public object Classification3 { get; set; }
        public object Classification4 { get; set; }
        public object Classification5 { get; set; }
        public object Classification6 { get; set; }
        public object Classification7 { get; set; }
        public object Classification8 { get; set; }
        public object ClassificationDescription { get; set; }
        public string Code { get; set; }
        public object CodeAtSupplier { get; set; }
        public object CompanySize { get; set; }
        public int ConsolidationScenario { get; set; }
        public object ControlledDate { get; set; }
        public object Costcenter { get; set; }
        public object CostcenterDescription { get; set; }
        public int CostPaid { get; set; }
        public string Country { get; set; }
        public string CountryName { get; set; }
        public DateTime Created { get; set; }
        public string Creator { get; set; }
        public string CreatorFullName { get; set; }
        public int CreditLinePurchase { get; set; }
        public int CreditLineSales { get; set; }
        public object Currency { get; set; }
        public DateTime? CustomerSince { get; set; }
        public object DatevCreditorCode { get; set; }
        public object DatevDebtorCode { get; set; }
        public int DiscountPurchase { get; set; }
        public int DiscountSales { get; set; }
        public int Division { get; set; }
        public object Document { get; set; }
        public object DunsNumber { get; set; }
        public string Email { get; set; }
        public object EndDate { get; set; }
        public object EstablishedDate { get; set; }
        public string Fax { get; set; }
        public string GLAccountPurchase { get; set; }
        public string GLAccountSales { get; set; }
        public object GLAP { get; set; }
        public object GLAR { get; set; }
        public bool? HasWithholdingTaxSales { get; set; }
        public string ID { get; set; }
        public bool IgnoreDatevWarningMessage { get; set; }
        public object IntraStatArea { get; set; }
        public object IntraStatDeliveryTerm { get; set; }
        public object IntraStatSystem { get; set; }
        public object IntraStatTransactionA { get; set; }
        public object IntraStatTransactionB { get; set; }
        public object IntraStatTransportMethod { get; set; }
        public object InvoiceAccount { get; set; }
        public object InvoiceAccountCode { get; set; }
        public object InvoiceAccountName { get; set; }
        public int InvoiceAttachmentType { get; set; }
        public int InvoicingMethod { get; set; }
        public int IsAccountant { get; set; }
        public int IsAgency { get; set; }
        public bool IsBank { get; set; }
        public int IsCompetitor { get; set; }
        public object IsExtraDuty { get; set; }
        public int IsMailing { get; set; }
        public bool IsMember { get; set; }
        public bool IsPilot { get; set; }
        public bool IsPurchase { get; set; }
        public bool IsReseller { get; set; }
        public bool IsSales { get; set; }
        public bool IsSupplier { get; set; }
        public string Language { get; set; }
        public string LanguageDescription { get; set; }
        public float? Latitude { get; set; }
        public object LeadSource { get; set; }
        public object LeadPurpose { get; set; }
        public object LogoFileName { get; set; }
        public string LogoThumbnailUrl { get; set; }
        public string LogoUrl { get; set; }
        public object Logo { get; set; }
        public float? Longitude { get; set; }
        public string MainContact { get; set; }
        public DateTime Modified { get; set; }
        public string Modifier { get; set; }
        public string ModifierFullName { get; set; }
        public string Name { get; set; }
        public string OINNumber { get; set; }
        public object Parent { get; set; }
        public string PaymentConditionPurchase { get; set; }
        public string PaymentConditionPurchaseDescription { get; set; }
        public string PaymentConditionSales { get; set; }
        public string PaymentConditionSalesDescription { get; set; }
        public object PayAsYouEarn { get; set; }
        public string Phone { get; set; }
        public object PhoneExtension { get; set; }
        public string Postcode { get; set; }
        public object PriceList { get; set; }
        public string PurchaseCurrency { get; set; }
        public string PurchaseCurrencyDescription { get; set; }
        public int PurchaseLeadDays { get; set; }
        public string PurchaseVATCode { get; set; }
        public string PurchaseVATCodeDescription { get; set; }
        public string Remarks { get; set; }
        public bool RecepientOfCommissions { get; set; }
        public object Reseller { get; set; }
        public object ResellerCode { get; set; }
        public object ResellerName { get; set; }
        public object RSIN { get; set; }
        public string SalesCurrency { get; set; }
        public string SalesCurrencyDescription { get; set; }
        public string SalesVATCode { get; set; }
        public string SalesVATCodeDescription { get; set; }
        public string SearchCode { get; set; }
        public int SecurityLevel { get; set; }
        public int SeparateInvPerProject { get; set; }
        public int SeparateInvPerSubscription { get; set; }
        public int ShippingLeadDays { get; set; }
        public object ShippingMethod { get; set; }
        public DateTime StartDate { get; set; }
        public string State { get; set; }
        public string StateName { get; set; }
        public string Status { get; set; }
        public DateTime StatusSince { get; set; }
        public object SalesTaxSchedule { get; set; }
        public object SalesTaxScheduleCode { get; set; }
        public object SalesTaxScheduleDescription { get; set; }
        public object TradeName { get; set; }
        public string Type { get; set; }
        public object UniqueTaxpayerReference { get; set; }
        public string VATLiability { get; set; }
        public string VATNumber { get; set; }
        public string Website { get; set; }
    }

    public class __Metadata2
    {
        public string uri { get; set; }
        public string type { get; set; }
    }

    public class Bankaccounts
    {
        public __Deferred __deferred { get; set; }
    }

    public class __Deferred
    {
        public string uri { get; set; }
    }


}
