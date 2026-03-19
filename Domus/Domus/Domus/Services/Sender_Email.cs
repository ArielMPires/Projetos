using Domus.Models.DB;
using Org.BouncyCastle.Asn1.X509;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;

namespace Domus.Services
{
    public class Sender_Email
    {
        private SmtpClient _smtpClient;
        private MailMessage _mailMessage;

        public Sender_Email(SmtpClient smtpClient, MailMessage mailMessage)
        {
            mailMessage.From = new MailAddress("suporteti@waldesa.com.br");
            smtpClient.Host = "email-ssl.com.br";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("suporteti@waldesa.com.br", "Supor@2025");
            smtpClient.EnableSsl = true;

            _smtpClient = smtpClient;
            _mailMessage = mailMessage;
        }

        public bool SendReset(string email, string NewPassword)
        {
            try
            {
                _mailMessage.To.Add(email);
                _mailMessage.Subject = "Reset Password";
                _mailMessage.Body = $"Sua Senha foi resetada, Nova Senha: {NewPassword}";
                _smtpClient.Send(_mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SendNewUser(Users user, string Password)
        {
            try
            {
                _mailMessage.To.Add(user.Email);
                _mailMessage.Subject = "Cadastro de Usuario no Agnus";
                _mailMessage.Body = $"Segue seu Id de Acesso e Senha para entrar no Agnus \n\n\n" +
                    $"ID : {user.ID} \n" +
                    $"Senha: {Password}";
                _smtpClient.Send(_mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SendRate(Users user, int order,int tenant)
        {
            try
            {
                _mailMessage.To.Add(user.Email);
                _mailMessage.Subject = $"Chamado {order} Finalizado";
                _mailMessage.Body = $"Chamado {order} Finalizado com sucesso, Segue um link abaixo para avaliação do serviço Opcional. \n\n\n" +
                    $"http://agnus.ddns.net:5254/?id={order}&tenant={tenant} \n";
                _smtpClient.Send(_mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool SendReport(Service_Order os, int tenant)
        {
            try
            {
                _mailMessage.To.Add("ariel@waldesa.com.br");
                _mailMessage.CC.Add("suporte.ti@waldesa.com.br");
                _mailMessage.Subject = $"Chamado {os.ID} Finalizado";
                _mailMessage.Body = $"Chamado {os.ID} Finalizado com sucesso, Segue Anexo Sobre a OS realizada.";
                var caminhoPdf = @$"PDF\{os.ID}-{tenant}.pdf";
                if (File.Exists(caminhoPdf))
                    _mailMessage.Attachments.Add(new Attachment(caminhoPdf, MediaTypeNames.Application.Pdf));
                _smtpClient.Send(_mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool SendPurchase(Purchase_Order order, int tenant)
        {
            try
            {
                _mailMessage.To.Add("ariel@waldesa.com.br");
                _mailMessage.CC.Add("suporte.ti@waldesa.com.br");
                _mailMessage.CC.Add("suporte.ti3@waldesa.com.br");
                _mailMessage.Subject = $"Pedido de Compra";
                _mailMessage.Body = $"Segue Em anexo Pedido de Compras";
                var caminhoPdf = @$"PDF\C-{order.ID}-{tenant}.pdf";
                if (File.Exists(caminhoPdf))
                    _mailMessage.Attachments.Add(new Attachment(caminhoPdf, MediaTypeNames.Application.Pdf));
                _smtpClient.Send(_mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
