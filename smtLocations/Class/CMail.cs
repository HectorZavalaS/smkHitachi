using EASendMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace smtLocations.Class
{
    public class CMail
    {
        //smtlocations!
        string From = ""; //de quien procede, puede ser un alias
        string To;  //a quien vamos a enviar el mail
        string m_Message;  //mensaje
        string Subject; //asunto
        List<string> Archivo = new List<string>(); //lista de archivos a enviar
        string DE = "siixsem.reports@siix-global.com"; //nuestro usuario de smtp
        string PASS = "S11x.r3p2024!"; //nuestro password de smtp
        //string DE = "javier.gallardo@siix-sem.com.mx"; //nuestro usuario de smtp
        //string PASS = "Zus67926"; //nuestro password de smtp

        System.Net.Mail.MailMessage Email;

        public string error = "";

        public string Message { get => m_Message; set => m_Message = value; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="FROM">Procedencia</param>
        /// <param name="Para">Mail al cual se enviara</param>
        /// <param name="Mensaje">Mensaje del mail</param>
        /// <param name="Asunto">Asunto del mail</param>
        /// <param name="ArchivoPedido_">Archivo a adjuntar, no es obligatorio</param>
        public CMail(string FROM, string Para, string Mensaje, string Asunto, List<string> ArchivoPedido_ = null)
        {
            From = FROM;
            To = Para;
            Message = Mensaje;
            Subject = Asunto;
            Archivo = ArchivoPedido_;

        }

        /// <summary>
        /// metodo que envia el mail
        /// </summary>
        /// <returns></returns>
        public bool enviaMail()
        {
            bool result = false;
            
            //una validación básica
            if (To.Trim().Equals("") || Message.Trim().Equals("") || Subject.Trim().Equals(""))
            {
                error = "El mail, el asunto y el mensaje son obligatorios";
                return result;
            }

            //aqui comenzamos el proceso
            //comienza-------------------------------------------------------------------------
            try
            {
                //creamos un objeto tipo MailMessage
                //este objeto recibe el sujeto o persona que envia el mail,
                //la direccion de procedencia, el asunto y el mensaje
                Email = new System.Net.Mail.MailMessage(From, To, Subject, Message);

                //si viene archivo a adjuntar
                //realizamos un recorrido por todos los adjuntos enviados en la lista
                //la lista se llena con direcciones fisicas, por ejemplo: c:/pato.txt
                if (Archivo != null)
                {
                    //agregado de archivo
                    foreach (string archivo in Archivo)
                    {
                        //comprobamos si existe el archivo y lo agregamos a los adjuntos
                        if (System.IO.File.Exists(@archivo))
                            Email.Attachments.Add(new System.Net.Mail.Attachment(@archivo));

                    }
                }

                Email.IsBodyHtml = true; //definimos si el contenido sera html
                Email.From = new System.Net.Mail.MailAddress(From); //definimos la direccion de procedencia

                //aqui creamos un objeto tipo SmtpClient el cual recibe el servidor que utilizaremos como smtp
                //en este caso me colgare de gmail
                System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)(768 | 3072);
                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("smtp.office365.com");
                //System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("mail.siix.mx");

                smtpMail.EnableSsl = true;
                smtpMail.UseDefaultCredentials = false; //le decimos que no utilice la credencial por defecto
                smtpMail.Host = "smtp.office365.com"; //agregamos el servidor smtp
                smtpMail.Port = 587; //le asignamos el puerto, en este caso gmail utiliza el 465
                smtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpMail.Credentials = new System.Net.NetworkCredential(DE, PASS); //agregamos nuestro usuario y pass de gmail

                //enviamos el mail
                smtpMail.Send(Email);

                //eliminamos el objeto
                smtpMail.Dispose();

                //regresamos true
                result = true;
            }
            catch (Exception ex)
            {
                //si ocurre un error regresamos false y el error
                error = "Ocurrio un error: " + ex.Message;
                result = false;
            }

            return result;
        }
        public bool enviaMail_v2()
        {
            bool result = false;

            //una validación básica
            if (To.Trim().Equals("") || Message.Trim().Equals("") || Subject.Trim().Equals(""))
            {
                error = "El mail, el asunto y el mensaje son obligatorios";
                return result;
            }
            try
            {
                SmtpMail oMail = new SmtpMail("TryIt");

                // Set sender email address, please change it to yours
                oMail.From = "smtlocations@siix.mx";
                // Set recipient email address, please change it to yours
                oMail.To = To.Trim();

                // Set email subject
                oMail.Subject = Subject.Trim();
                // Set Html body
                oMail.HtmlBody = Message;// "<font size=\"5\">This is</font> <font color=\"red\"><b>a test</b></font>";


                if (Archivo != null)
                {
                    //agregado de archivo
                    foreach (string archivo in Archivo)
                    {
                        //comprobamos si existe el archivo y lo agregamos a los adjuntos
                        if (System.IO.File.Exists(@archivo))
                            oMail.AddAttachment(@archivo);

                    }
                }

                // Your SMTP server address
                SmtpServer oServer = new SmtpServer("smtp.office365.com");

                // User and password for ESMTP authentication
                oServer.User = "javier.gallardo@siix-sem.com.mx";
                oServer.Password = "Zus67926";

                // Most mordern SMTP servers require SSL/TLS connection now.
                // ConnectTryTLS means if server supports SSL/TLS, SSL/TLS will be used automatically.
                oServer.ConnectType = SmtpConnectType.ConnectTryTLS;

                // If your SMTP server uses 587 port
                // oServer.Port = 587;

                // If your SMTP server requires SSL/TLS connection on 25/587/465 port
                // oServer.Port = 25; // 25 or 587 or 465
                // oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

                Console.WriteLine("start to send email with attachment ...");

                EASendMail.SmtpClient oSmtp = new EASendMail.SmtpClient();
                oSmtp.SendMail(oServer, oMail);

                Console.WriteLine("email was sent successfully!");
                result = true;
            }
            catch (Exception ep)
            {
                Console.WriteLine("failed to send email with the following error:");
                Console.WriteLine(ep.Message);
                error = "Ocurrio un error: " + ep.Message;
                result = false;
            }
        
            return result;
        }
    }
}