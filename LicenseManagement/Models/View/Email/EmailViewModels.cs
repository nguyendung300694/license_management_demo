using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LicenseManagement.Models.View.Email
{
    public class EmailViewModel
    {
        public int Id { get; set; }
        public int ProductionInvcId { get; set; }
        public int InvoiceKey { get; set; }

        public string FromName { get; set; }
        public string ToName { get; set; }
        public string CCName { get; set; }
        public string BCCName { get; set; }
        public string Subject { get; set; }
        public string[] Attachment { get; set; }
        public string Content { get; set; }

        public bool IsApproved { get; set; }
        public string UniqueMailId { get; set; }
    }

    public class SentEmailViewModel
    {
        public int CompanyID { get; set; }
        public string ScreenID { get; set; }

        public string DocType { get; set; }
        public string RefNbr { get; set; }
        public int EmailType { get; set; }

        public string FromName { get; set; }
        public string ToName { get; set; }
        public string CCName { get; set; }
        public string BCCName { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string[] Attachment { get; set; }

        public string MailUniqueID { get; set; }
    }
}