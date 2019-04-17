using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string GetLocalUrl(this IUrlHelper urlHelper, string localUrl)
        {
            if (!urlHelper.IsLocalUrl(localUrl))
            {
                return urlHelper.Page("/Index");
            }

            return localUrl;
        }

        public static string EmailConfirmationNewTicketLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Page(
                "/Account/ConfirmEmailNewPassword",
                pageHandler: null,
                values: new { userId, code },
                protocol: scheme);
        }

        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId, code },
                protocol: scheme);
        }

        public static string TicketLink(this IUrlHelper urlHelper, string scheme)
        {
            return urlHelper.Page(
                "/Tickets/MyTickets", null, null, scheme);
        }

        public static string TicketLinkID(this IUrlHelper urlHelper, string scheme)
        {
            return urlHelper.Page(
                "/Tickets/TicketView", null, null, scheme);
        }
        public static string Knowledgebase(this IUrlHelper urlHelper, string scheme)
        {
            return urlHelper.Page(
                "/Knowledge/Knowledgebase", null, null, scheme);
        }
        public static string Documents(this IUrlHelper urlHelper, string scheme)
        {
            return urlHelper.Page(
                "/DMS/DMS", null, null, scheme);
        }

        public static string TicketLinkFromMail(this IUrlHelper urlHelper, string scheme, int id)
        {
            return urlHelper.Page(
                "/Tickets/TicketView" + "/" + id, null, null, scheme);
        }

        public static string RegisterFirst(this IUrlHelper urlHelper, string scheme)
        {
            return urlHelper.Page(
                "/Register", null, null, scheme);
        }
        public static string CreateFirst(this IUrlHelper urlHelper, string scheme)
        {
            return urlHelper.Page(
                "/Tickets/CreateTicket", null, null, scheme);
        }


        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { userId, code },
                protocol: scheme);
        }
    }
}
