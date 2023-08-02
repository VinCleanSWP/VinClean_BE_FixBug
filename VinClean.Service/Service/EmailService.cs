using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VinClean.Service.DTO.Email;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using Azure;
using VinClean.Service.DTO.CustomerResponse;
using VinClean.Repo.Models;

namespace VinClean.Service.Service
{
    public interface IEmailService
    {
        void SendEmail(EmailFormDTO request);
        Task<ServiceResponse<AccountDTO>> SendEmailVerify(EmailFormDTO request);
        Task<ServiceResponse<AccountDTO>> SendEmailResetPassword(EmailFormDTO request);
        Task<ServiceResponse<AccountDTO>> SendAssignToCustomer(EmailFormDTO request);
        Task<ServiceResponse<AccountDTO>> SendAssignToEmployee(EmailFormDTO request);
        Task<ServiceResponse<AccountDTO>> SendEmailToChangeEmployee(EmailFormDTO request);
        Task<ServiceResponse<AccountDTO>> SendEmailToDeniedProcess(EmailFormDTO request);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly IAccountRepository _Accrepository;
        private readonly IOrderRepository _process;
        public EmailService(IConfiguration config, IAccountRepository Accrepository, IOrderRepository process)
        {
            _config = config;
            _Accrepository = Accrepository;
            _process = process;
        }

        public void SendEmail(EmailFormDTO request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) {
                Text = "<!DOCTYPE html> <html lang='en' xmlns='http://www.w3.org/1999/xhtml' xmlns:o='urn:schemas-microsoft-com:office:office'>" +
                    "<head>   <meta charset='UTF-8'>   <meta name='viewport' content='width=device-width,initial-scale=1'>" +
                    "  <meta name='x-apple-disable-message-reformatting'>   <title></title>   <!--[if mso]>   " +
                    "<noscript>     <xml>       <o:OfficeDocumentSettings>         <o:PixelsPerInch>96</o:PixelsPerInch>       </o:OfficeDocumentSettings>     " +
                    "</xml></noscript>  <![endif]-->   " +
                    "<style>" +
                    "    table, td, div, h1, p {font-family: Arial, sans-serif;}" +
                    "   .token{padding:5px 5px;       background-color:rgba(0, 0, 0, 0.1);   border-radius:5px; text-align: center;    padding : 5px;}" +
                    "</style>" +
                    "</head>" +
                    "<body style='margin:0;padding:0;'>  " +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;background:#ffffff;'>     " +
                    "<tr>       <td align='center' style='padding:0;'>         " +
                    "<table role='presentation' style='width:602px;border-collapse:collapse;border:1px solid #cccccc;border-spacing:0;text-align:left;'>           " +
                    "<tr>             <td align='center' style='padding:40px 0 30px 0;background:#ffffff;'>               " +
                    "<img src='https://firebasestorage.googleapis.com/v0/b/swp-vinclean-7b1d3.appspot.com/o/Customer%2Flogo.png?alt=media&token=150400e6-77bd-41d4-a0f7-cd69d94ce0f3' alt='' width='300' style='height:auto;display:block;' /></td></tr>" +
                    "<tr>           <td style='padding:36px 30px 42px 30px;'>          " +
                    " <table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;'>" +
                    " <tr>         <td style='padding:0 0 36px 0;color:#153643;'>      " +
                    $" <h1 style='font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;'>{request.Subject}</h1>            " +
                    " <p style='margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;'></p>     " +

                    $" <p>{request.Body}</p> " +

                    "<p style='margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;'>" +
                    "<a href='http://www.example.com' style='color:#ee4c50;text-decoration:underline;'> </a></p> </td> </tr>" +
                    "<tr><td style='padding:0;'>" +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;'>" +
                    "<tr><td style='width:260px;padding:0;vertical-align:top;color:#153643;'>" +
                    " <tr> <td style='padding:30px;background:#ee4c50;'> " +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;font-size:9px;font-family:Arial,sans-serif;'>  " +
                    "<tr><td style='padding:0;width:50%;' align='left'>" +
                    "<p style='margin:0;font-size:14px;line-height:16px;font-family:Arial,sans-serif;color:#ffffff;'>" +
                    "&reg; VinClean, HoChiMinh 2023<br/><a href='http://www.example.com' style='color:#ffffff;text-decoration:underline;'>Unsubscribe</a></p>   </td>           " +
                    "<td style='padding:0;width:50%;' align='right'>                 <table role='presentation' style='border-collapse:collapse;border:0;border-spacing:0;'>             " +
                    " <tr>     <td style='padding:0 0 0 10px;width:38px;'>  <a href='http://www.twitter.com/' style='color:#ffffff;'><img src='https://assets.codepen.io/210284/tw_1.png' alt='Twitter' width='38' style='height:auto;display:block;border:0;' /></a> </td>  <td style='padding:0 0 0 10px;width:38px;'> " +
                    " <a href='http://www.facebook.com/' style='color:#ffffff;'><img src='https://assets.codepen.io/210284/fb_1.png' alt='Facebook' width='38' style='height:auto;display:block;border:0;' /></a></td> " +
                    "</tr>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</table></td></tr></table></td></tr></table></body></html>"

            };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public async Task<ServiceResponse<AccountDTO>> SendEmailVerify(EmailFormDTO request)
        {
            ServiceResponse<AccountDTO> _response = new();
            try
            {
                var checkemail = await _Accrepository.GetbyEmail(request.To);
                if (checkemail == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
                email.To.Add(MailboxAddress.Parse(checkemail.Email));
                email.Subject = "Verification Account";
                email.Body = new TextPart(TextFormat.Html)
                {

                    Text= "<!DOCTYPE html> <html lang='en' xmlns='http://www.w3.org/1999/xhtml' xmlns:o='urn:schemas-microsoft-com:office:office'>" +
                    "<head>   <meta charset='UTF-8'>   <meta name='viewport' content='width=device-width,initial-scale=1'>" +
                    "  <meta name='x-apple-disable-message-reformatting'>   <title></title>   <!--[if mso]>   " +
                    "<noscript>     <xml>       <o:OfficeDocumentSettings>         <o:PixelsPerInch>96</o:PixelsPerInch>       </o:OfficeDocumentSettings>     " +
                    "</xml></noscript>  <![endif]-->   " +
                    "<style>" +
                    "    table, td, div, h1, p {font-family: Arial, sans-serif;}" +
                    "   .token{padding:5px 5px;       background-color:rgba(0, 0, 0, 0.1);   border-radius:5px; text-align: center;    padding : 5px;}" +
                    "</style>" +
                    "</head>" +
                    "<body style='margin:0;padding:0;'>  " +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;background:#ffffff;'>     " +
                    "<tr>       <td align='center' style='padding:0;'>         " +
                    "<table role='presentation' style='width:602px;border-collapse:collapse;border:1px solid #cccccc;border-spacing:0;text-align:left;'>           " +
                    "<tr>             <td align='center' style='padding:40px 0 30px 0;background:#ffffff;'>               " +
                    "<img src='https://firebasestorage.googleapis.com/v0/b/swp-vinclean-7b1d3.appspot.com/o/Customer%2Flogo.png?alt=media&token=150400e6-77bd-41d4-a0f7-cd69d94ce0f3' alt='' width='300' style='height:auto;display:block;' /></td></tr>" +
                    "<tr>           <td style='padding:36px 30px 42px 30px;'>          " +
                    " <table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;'>" +
                    " <tr>         <td style='padding:0 0 36px 0;color:#153643;'>      " +
                    " <h1 style='font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;'>Verification Account</h1>            " +
                    " <p style='margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;'></p>     " +
                    
                    $" <p>Hello <i>{checkemail.Name}</i></p> " +
                    $"<p>You registered an account on {checkemail.Email}, before being able to use your account you need to verify that this is your email address by clicking here: </p>" +
                    $"<p>https://localhost:7013/api/Account/Verify?token={checkemail.VerificationToken}</p>" +
                    $"<p>Or you can copy this Token and paste in Token form : </p>" +
                    $"<p><b class='token'>{checkemail.VerificationToken}</b></p>" +
                    $"<p>Kind Regards, <h3>VinClean</h3></p> " +
                    
                    "<p style='margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;'>" +
                    "<a href='http://www.example.com' style='color:#ee4c50;text-decoration:underline;'> </a></p> </td> </tr>" +
                    "<tr><td style='padding:0;'>" +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;'>" +
                    "<tr><td style='width:260px;padding:0;vertical-align:top;color:#153643;'>" +
                    " <tr> <td style='padding:30px;background:#ee4c50;'> " +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;font-size:9px;font-family:Arial,sans-serif;'>  " +
                    "<tr><td style='padding:0;width:50%;' align='left'>" +
                    "<p style='margin:0;font-size:14px;line-height:16px;font-family:Arial,sans-serif;color:#ffffff;'>" +
                    "&reg; VinClean, HoChiMinh 2023<br/><a href='http://www.example.com' style='color:#ffffff;text-decoration:underline;'>Unsubscribe</a></p>   </td>           " +
                    "<td style='padding:0;width:50%;' align='right'>                 <table role='presentation' style='border-collapse:collapse;border:0;border-spacing:0;'>             " +
                    " <tr>     <td style='padding:0 0 0 10px;width:38px;'>  <a href='http://www.twitter.com/' style='color:#ffffff;'><img src='https://assets.codepen.io/210284/tw_1.png' alt='Twitter' width='38' style='height:auto;display:block;border:0;' /></a> </td>  <td style='padding:0 0 0 10px;width:38px;'> " +
                    " <a href='http://www.facebook.com/' style='color:#ffffff;'><img src='https://assets.codepen.io/210284/fb_1.png' alt='Facebook' width='38' style='height:auto;display:block;border:0;' /></a></td> " +
                    "</tr>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</table></td></tr></table></td></tr></table></body></html>"

                    //Text = "<head>" +
                    //"<style>" +
                    //".myDiv {" +
                    //"   border: 2px solid grey;"+
                    //"background-color:rgba(0, 0, 0, 0.01);"+
                    //"   text-align: center;" +
                    //"   margin: 30px 100px;" +
                    //"   padding:20px 20px;" +
                    //"   font-family: Arial, sans-serif;" +
                    //"   font-size: 15px;"+
                    //"}" +
                    //".token{" +
                    //"   padding:5px 5px;" +
                    //"   background-color:rgba(0, 0, 0, 0.1);" +
                    //"   border-radius:5px;" +
                    //"   text-align: center;" +
                    //"}" +
                    //"</style></head>" +
                    //"<body>" +
                    //"<h1>The div element</h1>" +
                    //"<div class='myDiv'>" +
                    //$"<p>Hello <i>{checkemail.Name}</i></p> " +
                    //$"<p>You registered an account on {checkemail.Email}, before being able to use your account you need to verify that this is your email address by clicking here: </p>" +
                    //$"<p>https://localhost:7013/api/Account/Verify?token={checkemail.VerificationToken}</p>" +
                    //$"<p>Or you can copy this Token and paste in Token form : </p>" +
                    //$"<p><b class='token'>{checkemail.VerificationToken}</b></p>" +
                    //" <p>Kind Regards, <h3>VinClean</h3></p>" +
                    //"+</div></body></html>"

                };

                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
                smtp.Send(email);
                smtp.Disconnect(true);

                _response.Success = true;
                _response.Message = "OK";
                _response.Data = null;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        /*public async void SendEmailResetPassword(string Accountemail)
        {
            var checkemail = await _Accrepository.GetbyEmail(Accountemail);
            if (checkemail != null)
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
                email.To.Add(MailboxAddress.Parse(Accountemail));
                email.Subject = "Reset Password";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = "Hello [name]," +
                    $"  Somebody requested a new password for the {checkemail.Name} account associated with {checkemail.Email}." +
                    "  No changes have been made to your account yet." +
                    "  You can reset your password by clicking the   below:" +
                    "  Or Use your secret code!:" +
                    $"  {checkemail.PasswordResetToken}" +
                    "  If you did not request a new password, please let us know immediately by replying to this email." +
                    "  Yours, The VinClean team"
                };

                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }*/

        public async Task<ServiceResponse<AccountDTO>> SendEmailResetPassword(EmailFormDTO request)
        {
            ServiceResponse<AccountDTO> _response = new();
            try
            {
                var checkemail = await _Accrepository.GetbyEmail(request.To);
                if (checkemail == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
                email.To.Add(MailboxAddress.Parse(checkemail.Email));
                email.Subject = "Verification Account";
                email.Body = new TextPart(TextFormat.Html)
                {

                    Text = "<!DOCTYPE html> <html lang='en' xmlns='http://www.w3.org/1999/xhtml' xmlns:o='urn:schemas-microsoft-com:office:office'>" +
                    "<head>   <meta charset='UTF-8'>   <meta name='viewport' content='width=device-width,initial-scale=1'>" +
                    "  <meta name='x-apple-disable-message-reformatting'>   <title></title>   <!--[if mso]>   " +
                    "<noscript>     <xml>       <o:OfficeDocumentSettings>         <o:PixelsPerInch>96</o:PixelsPerInch>       </o:OfficeDocumentSettings>     " +
                    "</xml></noscript>  <![endif]-->   " +
                    "<style>" +
                    "    table, td, div, h1, p {font-family: Arial, sans-serif;}" +
                    "   .token{padding:5px 5px;       background-color:rgba(0, 0, 0, 0.1);   border-radius:5px; text-align: center;    padding : 5px;}" +
                    "</style>" +
                    "</head>" +
                    "<body style='margin:0;padding:0;'>  " +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;background:#ffffff;'>     " +
                    "<tr>       <td align='center' style='padding:0;'>         " +
                    "<table role='presentation' style='width:602px;border-collapse:collapse;border:1px solid #cccccc;border-spacing:0;text-align:left;'>           " +
                    "<tr>             <td align='center' style='padding:40px 0 30px 0;background:#ffffff;'>               " +
                    "<img src='https://firebasestorage.googleapis.com/v0/b/swp-vinclean-7b1d3.appspot.com/o/Customer%2Flogo.png?alt=media&token=150400e6-77bd-41d4-a0f7-cd69d94ce0f3' alt='' width='300' style='height:auto;display:block;' /></td></tr>" +
                    "<tr>           <td style='padding:36px 30px 42px 30px;'>          " +
                    " <table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;'>" +
                    " <tr>         <td style='padding:0 0 36px 0;color:#153643;'>      " +
                    " <h1 style='font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;'>Verification Account</h1>            " +
                    " <p style='margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;'></p>     " +
                   
                    $"<p>Hello <i>{checkemail.Name}</i>,</p>" +
                    $"<p>Somebody requested a new password for the {checkemail.Name} account associated with {checkemail.Email}.</p>" +
                    "<p>No changes have been made to your account yet.</p>" +
                    "<p>You can reset your password by clicking the   below:</p>" +
                    "<p>Or Use your secret code!:</p>" +
                    $"<p><b class='token'>{checkemail.PasswordResetToken}</b></p>" +
                    
                    "<p>If you did not request a new password, please let us know immediately by replying to this email.</p>" +
                    "<p>Yours,<h3>The VinClean team</h3></p>" +
                    "<p style='margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;'>" +
                    "<a href='http://www.example.com' style='color:#ee4c50;text-decoration:underline;'> </a></p> </td> </tr>" +
                    "<tr><td style='padding:0;'>" +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;'>" +
                    "<tr><td style='width:260px;padding:0;vertical-align:top;color:#153643;'>" +
                    " <tr> <td style='padding:30px;background:#ee4c50;'> " +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;font-size:9px;font-family:Arial,sans-serif;'>  " +
                    "<tr><td style='padding:0;width:50%;' align='left'>" +
                    "<p style='margin:0;font-size:14px;line-height:16px;font-family:Arial,sans-serif;color:#ffffff;'>" +
                    "&reg; VinClean, HoChiMinh 2023<br/><a href='http://www.example.com' style='color:#ffffff;text-decoration:underline;'>Unsubscribe</a></p>   </td>           " +
                    "<td style='padding:0;width:50%;' align='right'>                 <table role='presentation' style='border-collapse:collapse;border:0;border-spacing:0;'>             " +
                    " <tr>     <td style='padding:0 0 0 10px;width:38px;'>  <a href='http://www.twitter.com/' style='color:#ffffff;'><img src='https://assets.codepen.io/210284/tw_1.png' alt='Twitter' width='38' style='height:auto;display:block;border:0;' /></a> </td>  <td style='padding:0 0 0 10px;width:38px;'> " +
                    " <a href='http://www.facebook.com/' style='color:#ffffff;'><img src='https://assets.codepen.io/210284/fb_1.png' alt='Facebook' width='38' style='height:auto;display:block;border:0;' /></a></td> " +
                    "</tr>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</table></td></tr></table></td></tr></table></body></html>"



                };

                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
                smtp.Send(email);
                smtp.Disconnect(true);

                _response.Success = true;
                _response.Message = "OK";
                _response.Data = null;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }


        public async Task<ServiceResponse<AccountDTO>> SendAssignToCustomer(EmailFormDTO request)
        {
            ServiceResponse<AccountDTO> _response = new();
            try
            {
                var process = await _process.GetAllInfoById(request.OrderId);
                if (process == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
                email.To.Add(MailboxAddress.Parse(process.Email));
                email.Subject = "Thông báo dịch vụ VinClean";
                email.Body = new TextPart(TextFormat.Html)
                {

                    Text = "<!DOCTYPE html> <html lang='en' xmlns='http://www.w3.org/1999/xhtml' xmlns:o='urn:schemas-microsoft-com:office:office'>" +
                    "<head>   <meta charset='UTF-8'>   <meta name='viewport' content='width=device-width,initial-scale=1'>" +
                    "  <meta name='x-apple-disable-message-reformatting'>   <title></title>   <!--[if mso]>   " +
                    "<noscript>     <xml>       <o:OfficeDocumentSettings>         <o:PixelsPerInch>96</o:PixelsPerInch>       </o:OfficeDocumentSettings>     " +
                    "</xml></noscript>  <![endif]-->   " +
                    "<style>" +
                    "    table, td, div, h1, p {font-family: Arial, sans-serif;}" +
                    "   .token{padding:5px 5px;       background-color:rgba(0, 0, 0, 0.1);   border-radius:5px; text-align: center;    padding : 5px;}" +
                    "</style>" +
                    "</head>" +
                    "<body style='margin:0;padding:0;'>  " +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;background:#ffffff;'>     " +
                    "<tr>       <td align='center' style='padding:0;'>         " +
                    "<table role='presentation' style='width:602px;border-collapse:collapse;border:1px solid #cccccc;border-spacing:0;text-align:left;'>           " +
                    "<tr>             <td align='center' style='padding:30px 0 0px 0;background:#ffffff;'>               " +
                    "<img src='https://firebasestorage.googleapis.com/v0/b/swp-vinclean-7b1d3.appspot.com/o/Customer%2Flogo.png?alt=media&token=150400e6-77bd-41d4-a0f7-cd69d94ce0f3' alt='' width='200' style='height:auto;display:block;' /></td></tr>" +
                    "<tr>           <td style='padding:15px 30px 42px 30px;'>          " +
                    " <table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;'>" +
                    " <tr>         <td style='padding:0 0 36px 0;color:#153643;'>      " +
                    " <h1 style='font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;'>Thông báo dịch vụ VinClean</h1>            " +
                    " <p style='margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;'></p>     " +
                    $"<div><p>Xin chào <i>{process.Name}</i>,</p>" +
                    $"<p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</p>" +
                    "<p>Chúng tôi gửi lại bạn thông tin Dịch vụ và Nhân viên:</p>" +
                    "<h4>Thông tin Khách Hàng:</h4>" +
                   
                    $"<p><b>Tên</b>: {process.Name} </p>" +
                    $"<p><b>Điện thoại</b>: {process.Phone} </p>" +
                    $"<p><b>Địa chỉ</b>: {process.Address} </p>" +
                    $"<p><b>Ngày Làm</b>: {process.Date.ToString("dd/MM/yyyy")}</p>" +
                    $"<p><b>Giờ Làm</b>: {process.StartTime} - {process.EndTime}</p>" +
                    "<h4>Thông tin Nhân viên:</h4>" +
                    $"<p><b>Tên Nhân Viên</b>: {process.EmployeeName} </p>" +
                    $"<p><b>Điện thoại</b>: {process.EmployeePhone} </p>" +
                    $"<p><b>Email</b>: {process.EmployeeEmail} </p>" +
                    
                    "<p>Nếu bạn có bất cứ phàn nàn nào hãy gửi email phản hồi tới địa chỉ Email: vincleanvhgp@gmail.com " +
                    "<p>hoặc Hotline: 0329 300 159.</p>" +
                    "<p>Chúng tôi sẽ phản hồi trong thờ gian sớm nhất.</p>" +
                    "<p><h3>The VinClean team</h3></p </div>" +
                    "<p style='margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;'>" +
                    "<a href='http://www.example.com' style='color:#ee4c50;text-decoration:underline;'> </a></p> </td> </tr>" +
                    "<tr><td style='padding:0;'>" +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;'>" +
                    "<tr><td style='width:260px;padding:0;vertical-align:top;color:#153643;'>" +
                    " <tr> <td style='padding:30px;background:#ee4c50;'> " +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;font-size:9px;font-family:Arial,sans-serif;'>  " +
                    "<tr><td style='padding:0;width:50%;' align='left'>" +
                    "<p style='margin:0;font-size:14px;line-height:16px;font-family:Arial,sans-serif;color:#ffffff;'>" +
                    "&reg; VinClean, HoChiMinh 2023<br/><a href='http://www.example.com' style='color:#ffffff;text-decoration:underline;'>Unsubscribe</a></p>   </td>           " +
                    "<td style='padding:0;width:50%;' align='right'>                 <table role='presentation' style='border-collapse:collapse;border:0;border-spacing:0;'>             " +
                    " <tr>     <td style='padding:0 0 0 10px;width:38px;'>  <a href='http://www.twitter.com/' style='color:#ffffff;'><img src='https://assets.codepen.io/210284/tw_1.png' alt='Twitter' width='38' style='height:auto;display:block;border:0;' /></a> </td>  <td style='padding:0 0 0 10px;width:38px;'> " +
                    " <a href='http://www.facebook.com/' style='color:#ffffff;'><img src='https://assets.codepen.io/210284/fb_1.png' alt='Facebook' width='38' style='height:auto;display:block;border:0;' /></a></td> " +
                    "</tr>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</table></td></tr></table></td></tr></table></body></html>"

                    
                };

                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
                smtp.Send(email);
                smtp.Disconnect(true);

                _response.Success = true;
                _response.Message = "OK";
                _response.Data = null;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        public async Task<ServiceResponse<AccountDTO>> SendAssignToEmployee(EmailFormDTO request)
        {
            ServiceResponse<AccountDTO> _response = new();
            try
            {
                var process = await _process.GetAllInfoById(request.OrderId);
                if ( process == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
                email.To.Add(MailboxAddress.Parse(process.EmployeeEmail));
                email.Subject = "THÔNG BÁO CÔNG VIỆC";
                email.Body = new TextPart(TextFormat.Html)
                {

                    Text = "<!DOCTYPE html> <html lang='en' xmlns='http://www.w3.org/1999/xhtml' xmlns:o='urn:schemas-microsoft-com:office:office'>" +
                    "<head>   <meta charset='UTF-8'>   <meta name='viewport' content='width=device-width,initial-scale=1'>" +
                    "  <meta name='x-apple-disable-message-reformatting'>   <title></title>   <!--[if mso]>   " +
                    "<noscript>     <xml>       <o:OfficeDocumentSettings>         <o:PixelsPerInch>96</o:PixelsPerInch>       </o:OfficeDocumentSettings>     " +
                    "</xml></noscript>  <![endif]-->   " +
                    "<style>" +
                    "    table, td, div, h1, p {font-family: Arial, sans-serif;}" +
                    "   .token{padding:5px 5px;       background-color:rgba(0, 0, 0, 0.1);   border-radius:5px; text-align: center;    padding : 5px;}" +
                    "</style>" +
                    "</head>" +
                    "<body style='margin:0;padding:0;'>  " +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;background:#ffffff;'>     " +
                    "<tr>       <td align='center' style='padding:0;'>         " +
                    "<table role='presentation' style='width:602px;border-collapse:collapse;border:1px solid #cccccc;border-spacing:0;text-align:left;'>           " +
                    "<tr>             <td align='center' style='padding:40px 0 30px 0;background:#ffffff;'>               " +
                    "<img src='https://firebasestorage.googleapis.com/v0/b/swp-vinclean-7b1d3.appspot.com/o/Customer%2Flogo.png?alt=media&token=150400e6-77bd-41d4-a0f7-cd69d94ce0f3' alt='' width='300' style='height:auto;display:block;' /></td></tr>" +
                    "<tr>           <td style='padding:36px 30px 42px 30px;'>          " +
                    " <table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;'>" +
                    " <tr>         <td style='padding:0 0 36px 0;color:#153643;'>      " +
                    " <h1 style='font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;'>THÔNG BÁO CÔNG VIỆC</h1>            " +
                    " <p style='margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;'></p>     " +
                    
                    $"<p>Thông báo tới nhân viên <i>{process.EmployeeName}</i>,</p>" +
                    $"<p>Bạn có một công việc mới.</p>" +
                    "<p>Chúng tôi gửi lại bạn thông tin Dịch vụ và Khác Hàng:</p>" +
                    "<h4>Thông tin Khách Hàng:</h4>" +
                    $"<p><b>Tên</b>: {process.Name} </p>" +
                    $"<p><b>Điện thoại</b>: {process.Phone} </p>" +
                    $"<p><b>Địa chỉ</b>: {process.Address} </p>" +
                    $"<p><b>Ngày Làm</b>: {process.Date.ToString("dd/MM/yyyy")}</p>" +
                    $"<p><b>Giờ Làm</b>: {process.StartTime} - {process.EndTime}</p>" +
                    "<h4>Thông tin Nhân viên:</h4>" +
                    $"<p><b>Tên Nhân Viên</b>: {process.EmployeeName} </p>" +
                    $"<p><b>Điện thoại</b>: {process.EmployeePhone} </p>" +
                    $"<p><b>Email</b>: {process.EmployeeEmail} </p>" +
                    $"<p><b>Xin vui lòng hãy đến đúng giờ. </p>" +

                    "<p>Nếu bạn có bất cứ phàn nàn nào hãy gửi email phản hồi tới địa chỉ Email: vincleanvhgp@gmail.com hoặc Hotline: 0329 300 159." +
                    "Chúng tôi sẽ phản hồi trong thờ gian sớm nhất.</p>" +
                    "<p>Yours,<h3>The VinClean team</h3></p>" +
                    "<p style='margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;'>" +
                    "<a href='http://www.example.com' style='color:#ee4c50;text-decoration:underline;'> </a></p> </td> </tr>" +
                    "<tr><td style='padding:0;'>" +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;'>" +
                    "<tr><td style='width:260px;padding:0;vertical-align:top;color:#153643;'>" +
                    " <tr> <td style='padding:30px;background:#ee4c50;'> " +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;font-size:9px;font-family:Arial,sans-serif;'>  " +
                    "<tr><td style='padding:0;width:50%;' align='left'>" +
                    "<p style='margin:0;font-size:14px;line-height:16px;font-family:Arial,sans-serif;color:#ffffff;'>" +
                    "&reg; VinClean, HoChiMinh 2023<br/><a href='http://www.example.com' style='color:#ffffff;text-decoration:underline;'>Unsubscribe</a></p>   </td>           " +
                    "<td style='padding:0;width:50%;' align='right'>                 <table role='presentation' style='border-collapse:collapse;border:0;border-spacing:0;'>             " +
                    " <tr>     <td style='padding:0 0 0 10px;width:38px;'>  <a href='http://www.twitter.com/' style='color:#ffffff;'><img src='https://assets.codepen.io/210284/tw_1.png' alt='Twitter' width='38' style='height:auto;display:block;border:0;' /></a> </td>  <td style='padding:0 0 0 10px;width:38px;'> " +
                    " <a href='http://www.facebook.com/' style='color:#ffffff;'><img src='https://assets.codepen.io/210284/fb_1.png' alt='Facebook' width='38' style='height:auto;display:block;border:0;' /></a></td> " +
                    "</tr>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</table></td></tr></table></td></tr></table></body></html>"

                };

                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
                smtp.Send(email);
                smtp.Disconnect(true);

                _response.Success = true;
                _response.Message = "OK";
                _response.Data = null;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }


        public async Task<ServiceResponse<AccountDTO>> SendEmailToChangeEmployee(EmailFormDTO request)
        {
            ServiceResponse<AccountDTO> _response = new();
            try
            {
                var checkemail = await _Accrepository.GetbyEmail(request.To);
                var process = await _process.GetAllInfoById(request.OrderId);
                if (checkemail == null && process == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
                email.To.Add(MailboxAddress.Parse(checkemail.Email));
                email.Subject = "THÔNG BÁO CẬP NHẬT NHÂN VIÊN";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = "<!DOCTYPE html> <html lang='en' xmlns='http://www.w3.org/1999/xhtml' xmlns:o='urn:schemas-microsoft-com:office:office'>" +
                    "<head>   <meta charset='UTF-8'>   <meta name='viewport' content='width=device-width,initial-scale=1'>" +
                    "  <meta name='x-apple-disable-message-reformatting'>   <title></title>   <!--[if mso]>   " +
                    "<noscript>     <xml>       <o:OfficeDocumentSettings>         <o:PixelsPerInch>96</o:PixelsPerInch>       </o:OfficeDocumentSettings>     " +
                    "</xml></noscript>  <![endif]-->   " +
                    "<style>" +
                    "    table, td, div, h1, p {font-family: Arial, sans-serif;}" +
                    "   .token{padding:5px 5px;       background-color:rgba(0, 0, 0, 0.1);   border-radius:5px; text-align: center;    padding : 5px;}" +
                    "</style>" +
                    "</head>" +
                    "<body style='margin:0;padding:0;'>  " +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;background:#ffffff;'>     " +
                    "<tr>       <td align='center' style='padding:0;'>         " +
                    "<table role='presentation' style='width:602px;border-collapse:collapse;border:1px solid #cccccc;border-spacing:0;text-align:left;'>           " +
                    "<tr>             <td align='center' style='padding:40px 0 30px 0;background:#ffffff;'>               " +
                    "<img src='https://firebasestorage.googleapis.com/v0/b/swp-vinclean-7b1d3.appspot.com/o/Customer%2Flogo.png?alt=media&token=150400e6-77bd-41d4-a0f7-cd69d94ce0f3' alt='' width='300' style='height:auto;display:block;' /></td></tr>" +
                    "<tr>           <td style='padding:36px 30px 42px 30px;'>          " +
                    " <table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;'>" +
                    " <tr>         <td style='padding:0 0 36px 0;color:#153643;'>      " +
                    " <h1 style='font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;'>THÔNG BÁO CẬP NHẬT NHÂN VIÊN</h1>            " +
                    " <p style='margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;'></p>     " +
                    
                    $"<p>Xin chào <i>{checkemail.Name}</i>,</p>" +
                    $"<p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</p>" +
                    "<p>Chúng tôi gửi lại bạn thông tin Dịch vụ và Nhân viên:</p>" +
                    "<h4>Thông tin Khách Hàng:</h4>" +
                    $"<p><b>Tên</b>: {checkemail.Name} </p>" +
                    $"<p><b>Điện thoại</b>: {process.Phone} </p>" +
                    $"<p><b>Địa chỉ</b>: {process.Address} </p>" +
                    $"<p><b>Ngày Làm</b>: {process.Date.ToString("dd/MM/yyyy")}</p>" +
                    $"<p><b>Giờ Làm</b>: {process.StartTime} - {process.EndTime}</p>" +
                    "<h4>Thông tin Nhân viên:</h4>" +
                    $"<p><b>Tên Nhân Viên</b>: {process.EmployeeName} </p>" +
                    $"<p><b>Điện thoại</b>: {process.EmployeePhone} </p>" +
                    $"<p><b>Email</b>: {process.EmployeeEmail} </p>" +
                    $"<p><b>Xin vui lòng hãy đến đúng giờ. </p>" +
                    
                    "<p>Nếu bạn có bất cứ phàn nàn nào hãy gửi email phản hồi tới địa chỉ Email: vincleanvhgp@gmail.com " +
                    "<p>hoặc Hotline: 0329 300 159.</p>" +
                    "<p>Chúng tôi sẽ phản hồi trong thờ gian sớm nhất.</p>" +
                    "<p><h3>The VinClean team</h3></p>" +
                    "<p style='margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;'>" +
                    "<a href='http://www.example.com' style='color:#ee4c50;text-decoration:underline;'> </a></p> </td> </tr>" +
                    "<tr><td style='padding:0;'>" +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;'>" +
                    "<tr><td style='width:260px;padding:0;vertical-align:top;color:#153643;'>" +
                    " <tr> <td style='padding:30px;background:#ee4c50;'> " +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;font-size:9px;font-family:Arial,sans-serif;'>  " +
                    "<tr><td style='padding:0;width:50%;' align='left'>" +
                    "<p style='margin:0;font-size:14px;line-height:16px;font-family:Arial,sans-serif;color:#ffffff;'>" +
                    "&reg; VinClean, HoChiMinh 2023<br/><a href='http://www.example.com' style='color:#ffffff;text-decoration:underline;'>Unsubscribe</a></p>   </td>           " +
                    "<td style='padding:0;width:50%;' align='right'>                 <table role='presentation' style='border-collapse:collapse;border:0;border-spacing:0;'>             " +
                    " <tr>     <td style='padding:0 0 0 10px;width:38px;'>  <a href='http://www.twitter.com/' style='color:#ffffff;'><img src='https://assets.codepen.io/210284/tw_1.png' alt='Twitter' width='38' style='height:auto;display:block;border:0;' /></a> </td>  <td style='padding:0 0 0 10px;width:38px;'> " +
                    " <a href='http://www.facebook.com/' style='color:#ffffff;'><img src='https://assets.codepen.io/210284/fb_1.png' alt='Facebook' width='38' style='height:auto;display:block;border:0;' /></a></td> " +
                    "</tr>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</table></td></tr></table></td></tr></table></body></html>"
                };

                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
                smtp.Send(email);
                smtp.Disconnect(true);

                _response.Success = true;
                _response.Message = "OK";
                _response.Data = null;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }


        public async Task<ServiceResponse<AccountDTO>> SendEmailToDeniedProcess(EmailFormDTO request)
        {
            ServiceResponse<AccountDTO> _response = new();
            try
            {
                var process = await _process.GetAllInfoById(request.OrderId);
                if (process == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
                email.To.Add(MailboxAddress.Parse(process.Email));
                email.Subject = "Thông báo dịch vụ VinClean";
                email.Body = new TextPart(TextFormat.Html)
                {

                    Text = "<!DOCTYPE html> <html lang='en' xmlns='http://www.w3.org/1999/xhtml' xmlns:o='urn:schemas-microsoft-com:office:office'>" +
                    "<head>   <meta charset='UTF-8'>   <meta name='viewport' content='width=device-width,initial-scale=1'>" +
                    "  <meta name='x-apple-disable-message-reformatting'>   <title></title>   <!--[if mso]>   " +
                    "<noscript>     <xml>       <o:OfficeDocumentSettings>         <o:PixelsPerInch>96</o:PixelsPerInch>       </o:OfficeDocumentSettings>     " +
                    "</xml></noscript>  <![endif]-->   " +
                    "<style>" +
                    "    table, td, div, h1, p {font-family: Arial, sans-serif;}" +
                    "   .token{padding:5px 5px;       background-color:rgba(0, 0, 0, 0.1);   border-radius:5px; text-align: center;    padding : 5px;}" +
                    "</style>" +
                    "</head>" +
                    "<body style='margin:0;padding:0;'>  " +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;background:#ffffff;'>     " +
                    "<tr>       <td align='center' style='padding:0;'>         " +
                    "<table role='presentation' style='width:602px;border-collapse:collapse;border:1px solid #cccccc;border-spacing:0;text-align:left;'>           " +
                    "<tr>             <td align='center' style='padding:30px 0 0px 0;background:#ffffff;'>               " +
                    "<img src='https://firebasestorage.googleapis.com/v0/b/swp-vinclean-7b1d3.appspot.com/o/Customer%2Flogo.png?alt=media&token=150400e6-77bd-41d4-a0f7-cd69d94ce0f3' alt='' width='200' style='height:auto;display:block;' /></td></tr>" +
                    "<tr>           <td style='padding:15px 30px 42px 30px;'>          " +
                    " <table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;'>" +
                    " <tr>         <td style='padding:0 0 36px 0;color:#153643;'>      " +
                    " <h1 style='font-size:24px;margin:0 0 20px 0;font-family:Arial,sans-serif;'>Hủy Yêu Cầu Đặt Dịch Vụ</h1>            " +
                    " <p style='margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;'></p>     " +
                    $"<div><p>Xin chào <i>{process.Name}</i>,</p>" +
                    $"<p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</p>" +
                    "<p>Chúng tôi gửi lại bạn thông tin Dịch vụ và Nhân viên:</p>" +
                    "<h4>Thông tin Khách Hàng:</h4>" +

                    $"<p><b>Tên</b>: {process.Name} </p>" +
                    $"<p><b>Điện thoại</b>: {process.Phone} </p>" +
                    $"<p><b>Địa chỉ</b>: {process.Address} </p>" +
                    $"<p><b>Ngày Làm</b>: {process.Date.ToString("dd/MM/yyyy")}</p>" +
                    $"<p><b>Giờ Làm</b>: {process.StartTime} - {process.EndTime}</p>" +
                    "<h4>Lý do hủy:</h4>" +
                    $"<p><b>Lý do</b>: ${request.Subject}. </p>" +
                    $"<p>Chúng tôi chân thành gửi lời xin lỗi đến phía bạn {process.Name}. </p>" +
                    $"<p><b>Mong rằng bạn có thể bỏ qua và tiếp tục đặt dịch vụ của chúng tôi vào một ngày khác </p>" +
                    $"<p><b>Xin chân thành cảm ơn. </p>" +

                    "<p>Nếu bạn có bất cứ phàn nàn nào hãy gửi email phản hồi tới địa chỉ Email: vincleanvhgp@gmail.com " +
                    "<p>hoặc Hotline: 0329 300 159.</p>" +
                    "<p>Chúng tôi sẽ phản hồi trong thờ gian sớm nhất.</p>" +
                    "<p><h3>The VinClean team</h3></p </div>" +
                    "<p style='margin:0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;'>" +
                    "<a href='http://www.example.com' style='color:#ee4c50;text-decoration:underline;'> </a></p> </td> </tr>" +
                    "<tr><td style='padding:0;'>" +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;'>" +
                    "<tr><td style='width:260px;padding:0;vertical-align:top;color:#153643;'>" +
                    " <tr> <td style='padding:30px;background:#ee4c50;'> " +
                    "<table role='presentation' style='width:100%;border-collapse:collapse;border:0;border-spacing:0;font-size:9px;font-family:Arial,sans-serif;'>  " +
                    "<tr><td style='padding:0;width:50%;' align='left'>" +
                    "<p style='margin:0;font-size:14px;line-height:16px;font-family:Arial,sans-serif;color:#ffffff;'>" +
                    "&reg; VinClean, HoChiMinh 2023<br/><a href='http://www.example.com' style='color:#ffffff;text-decoration:underline;'>Unsubscribe</a></p>   </td>           " +
                    "<td style='padding:0;width:50%;' align='right'>                 <table role='presentation' style='border-collapse:collapse;border:0;border-spacing:0;'>             " +
                    " <tr>     <td style='padding:0 0 0 10px;width:38px;'>  <a href='http://www.twitter.com/' style='color:#ffffff;'><img src='https://assets.codepen.io/210284/tw_1.png' alt='Twitter' width='38' style='height:auto;display:block;border:0;' /></a> </td>  <td style='padding:0 0 0 10px;width:38px;'> " +
                    " <a href='http://www.facebook.com/' style='color:#ffffff;'><img src='https://assets.codepen.io/210284/fb_1.png' alt='Facebook' width='38' style='height:auto;display:block;border:0;' /></a></td> " +
                    "</tr>" +
                    "</table>" +
                    "</td>" +
                    "</tr>" +
                    "</table></td></tr></table></td></tr></table></body></html>"

                };

                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
                smtp.Send(email);
                smtp.Disconnect(true);

                _response.Success = true;
                _response.Message = "OK";
                _response.Data = null;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }



    }
}
