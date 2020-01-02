using LicenseManagement.Models.Email;
using LicenseManagement.Services.Logger;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LicenseManagement.Services.Email
{
    public class UserEmail
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }

    public class EmailService : IEmailService
    {
        private readonly ILogger _log = DependencyResolver.Current.GetService<ILogger>();
        private readonly ExchangeService _exchangeService;
        private readonly ExchangeService _outlookService_POSearching;

        public EmailService()
        {
            //CREATE EXCHANGE SERVICE FOR SENDING EMAIL
            var usrDtl = new UserEmail
            {
                //UserId = HttpContext.Current.User.Identity.Name,
                //Password = I3DSSHelper.Decrypt(CookiesHelper.GetCookie<string>(EnumCollection.CookiesKey.EmailPassword), true)
                UserId = AppSettings.mail_username,
                Password = AppSettings.mail_pass
            };

            _exchangeService = new ExchangeService(ExchangeVersion.Exchange2010_SP2)
            {
                Credentials = new NetworkCredential(usrDtl.UserId, usrDtl.Password),
                Url = new Uri(AppSettings.EmailServer),
                //PreAuthenticate = true
            };

            //CREATE OUTLOOK SERVICE FOR SEARCHING PO ATTACHMENT
            var userName = AppSettings.orders_mailbox_username;
            var passWord = AppSettings.orders_mailbox_pass;

            _outlookService_POSearching = new ExchangeService(ExchangeVersion.Exchange2010_SP2)
            {
                Credentials = new NetworkCredential(userName, passWord),
                Url = new Uri(AppSettings.EmailServer_Office365),
                //PreAuthenticate = true
            };
        }

        #region Email Sending
        public string SendEmail(EmailViewModel emailModel)
        {
            try
            {
                bool isSendOK;
                return SendEmailByMSExchange(emailModel, out isSendOK);
            }
            catch (Exception ex)
            {
                _log.Error("SendEmail", ex.Message);
                throw ex;
            }

            /*
            try //try sending email by office365 first
            {
                bool isSendOK;
                //return EmailInvoiceToSent(emailModel, out isSendOK);
                return SendEmailByMSOffice365(emailModel, out isSendOK);
            }
            catch (Exception ex)
            {
                bool isSendOK = false;
                var strMessageId = string.Empty;

                for (int i = 0; i < 1; i++) //try once again with office365
                {
                    Thread.Sleep(3000);

                    try
                    {
                        strMessageId = SendEmailByMSOffice365(emailModel, out isSendOK);
                        if (isSendOK)
                        {
                            return strMessageId;
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                }

                if (!isSendOK) //re-try with exchange 3 times
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Thread.Sleep(3000);

                        try
                        {
                            strMessageId = SendEmailByMSExchange(emailModel, out isSendOK);
                            if (isSendOK)
                            {
                                return strMessageId;
                            }
                        }
                        catch
                        {
                            // ignore
                        }
                    }
                }

                if (!isSendOK)
                {
                    _log.Error("SendEmail", ex.Message);
                    throw ex;
                }
                return strMessageId;
            }
            */
        }

        private string SendEmailByMSExchange(EmailViewModel emailModel, out bool isSentOK)
        {
            //var usDtl = SessionHelper.Get<UserInformation>(HttpContext.Current.Session, EnumCollection.SessionKey.UserLogin);

            //if (usDtl == null)
            //{
            //    throw new HttpResponseException(HttpStatusCode.Unauthorized);
            //}

            ExchangeService exchangeSrv = _exchangeService;

            //if (!string.Equals(emailModel.FromName.ToLower().Trim(),
            //        !string.IsNullOrEmpty(usDtl.UserLdapDetail.EmailAddress)
            //            ? usDtl.UserLdapDetail.EmailAddress.ToLower().Trim()
            //            : ""))
            //{
            //    exchangeSrv = new ExchangeService(ExchangeVersion.Exchange2010_SP2)
            //    {
            //        Credentials = new NetworkCredential(
            //            AppSettings.mail_username,
            //            AppSettings.mail_pass),
            //        Url = new Uri(AppSettings.EmailServer),
            //        //PreAuthenticate = true
            //    };
            //}

            var message = new EmailMessage(exchangeSrv);

            message.From = emailModel.FromName.ToLower().Trim() != AppSettings.orders_account_mail
                ? AppSettings.orders_account_mail
                : emailModel.FromName;
            message.Subject = emailModel.Subject;
            message.Body = emailModel.Content;

            if (!string.IsNullOrEmpty(emailModel.ToName))
            {
                var strReceives = emailModel.ToName.Split(';');

                foreach (var strReceive in strReceives)
                {
                    if (!string.IsNullOrEmpty(strReceive.Trim()))
                        message.ToRecipients.Add(strReceive.Trim());
                }
            }

            if (!string.IsNullOrEmpty(emailModel.CCName))
            {
                var strCcReceives = emailModel.CCName.Split(';');

                foreach (var strCcReceive in strCcReceives)
                {
                    if (!string.IsNullOrEmpty(strCcReceive.Trim()))
                        message.CcRecipients.Add(strCcReceive.Trim());
                }
            }

            if (!string.IsNullOrEmpty(emailModel.BCCName))
            {
                var strBCcReceives = emailModel.BCCName.Split(';');

                foreach (var strBCcReceive in strBCcReceives)
                {
                    if (!string.IsNullOrEmpty(strBCcReceive.Trim()))
                        message.BccRecipients.Add(strBCcReceive.Trim());
                }
            }

            if (emailModel.Attachment != null)
            {
                foreach (var attachment in emailModel.Attachment)
                {
                    if (string.IsNullOrEmpty(attachment) == false)
                    {
                        var attachmentPath = HttpContext.Current.Server.MapPath("~" + attachment);
                        message.Attachments.AddFileAttachment(attachmentPath);
                    }
                }
            }

            message.SendAndSaveCopy();

            if (emailModel.Attachment == null /*|| emailModel.Attachment.Any(m => !string.IsNullOrEmpty(m)) == false*/)
            {
                isSentOK = true;
                return string.Empty;
            }
            isSentOK = true;
            return message.Id.UniqueId;
        }

        private string SendEmailByMSOffice365(EmailViewModel emailModel, out bool isSentOK)
        {
            /*
            var usDtl = SessionHelper.Get<UserInformation>(HttpContext.Current.Session, EnumCollection.SessionKey.UserLogin);
            
            if (usDtl == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            ExchangeService _outlookService;

            if (!string.Equals(emailModel.FromName.ToLower().Trim(),
                !string.IsNullOrEmpty(usDtl.UserLdapDetail.EmailAddress)
                    ? usDtl.UserLdapDetail.EmailAddress.ToLower().Trim()
                    : ""))
            {
                //If sender is orders mailbox: using genacc and send by exchange service

                _outlookService = new ExchangeService(ExchangeVersion.Exchange2010_SP2)
                {
                    Credentials = new NetworkCredential(
                        AppSettings.mail_username,
                        AppSettings.mail_pass),
                    Url = new Uri(AppSettings.EmailServer),
                    //PreAuthenticate = true
                };
            }
            else
            {
                //If sender is user: using user account and send by office365 service

                _outlookService = new ExchangeService(ExchangeVersion.Exchange2010_SP2)
                {
                    Credentials = new NetworkCredential(usDtl.UserLdapDetail.EmailAddress, usDtl.UserLdapDetail.Password),
                    Url = new Uri(AppSettings.EmailServer_Office365),
                    //PreAuthenticate = true
                };
            }
            */
            ExchangeService _outlookService = _outlookService_POSearching;

            var message = new EmailMessage(_outlookService);
            //message.From = emailModel.FromName;
            message.From = AppSettings.ar_account_mail;
            message.Subject = emailModel.Subject;
            message.Body = emailModel.Content;

            if (!string.IsNullOrEmpty(emailModel.ToName))
            {
                var strReceives = emailModel.ToName.Split(';');

                foreach (var strReceive in strReceives)
                {
                    if (!string.IsNullOrEmpty(strReceive.Trim()))
                        message.ToRecipients.Add(strReceive.Trim());
                }
            }

            if (!string.IsNullOrEmpty(emailModel.CCName))
            {
                var strCcReceives = emailModel.CCName.Split(';');

                foreach (var strCcReceive in strCcReceives)
                {
                    if (!string.IsNullOrEmpty(strCcReceive.Trim()))
                        message.CcRecipients.Add(strCcReceive.Trim());
                }
            }

            if (!string.IsNullOrEmpty(emailModel.BCCName))
            {
                var strBCcReceives = emailModel.BCCName.Split(';');

                foreach (var strBCcReceive in strBCcReceives)
                {
                    if (!string.IsNullOrEmpty(strBCcReceive.Trim()))
                        message.BccRecipients.Add(strBCcReceive.Trim());
                }
            }

            if (emailModel.Attachment != null)
            {
                foreach (var attachment in emailModel.Attachment)
                {
                    var attachmentPath = HttpContext.Current.Server.MapPath("~" + attachment);
                    message.Attachments.AddFileAttachment(attachmentPath);
                }
            }

            message.SendAndSaveCopy();

            if (emailModel.Attachment == null/* || emailModel.Attachment.Any(m => !string.IsNullOrEmpty(m)) == false*/)
            {
                isSentOK = true;
                return string.Empty;
            }
            isSentOK = true;
            return message.Id.UniqueId;
        }
        #endregion
    }
}