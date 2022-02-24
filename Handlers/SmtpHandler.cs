using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.IO;
using MimeKit.Text;
using System;
using System.IO;
using Umbraco.Cms.Core.Configuration.Models;
using MailKit.Net;

namespace BasisProjectUmbraco.Handlers
{
    public interface ISmtpHandler
    {
        bool Send(string body, string from, string to, string subject);
    }

    //public interface ISmtpHandler
    //{

    //}
    public class SmtpHandler : ISmtpHandler
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<SmtpHandler> _logger;
        private readonly GlobalSettings _globalSettings;
        public SmtpHandler(IConfiguration config, IWebHostEnvironment env, ILogger<SmtpHandler> logger, IOptions<GlobalSettings> options)
        {
            _config = config;
            _env = env;
            _logger = logger;
            _globalSettings = options.Value;
        }
        public bool Send(string body, string from, string to, string subject)
        {
            var succes = false;

            //if (string.IsNullOrEmpty(from))
            //    from = _config.GetValue<string>("Nibc:Email:From");

            //if (string.IsNullOrEmpty(to))
            //    to = _config.GetValue<string>("Nibc:Email:To");

            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));

            foreach (string toAddress in to.Split(';'))
            {
                if (!string.IsNullOrWhiteSpace(toAddress))
                    email.To.Add(MailboxAddress.Parse(toAddress));
            }

            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            // send email
            var smtp = new SmtpClient();

            try
            {
                if (_globalSettings.Smtp.DeliveryMethod == System.Net.Mail.SmtpDeliveryMethod.SpecifiedPickupDirectory)
                {
                    SaveToPickupDirectory(email, _globalSettings.Smtp.PickupDirectoryLocation);
                    succes = true;
                }
                else
                {
                    smtp.Connect(_globalSettings.Smtp.Host, _globalSettings.Smtp.Port);
                    if (!string.IsNullOrEmpty(_globalSettings.Smtp.Username))
                    {
                        smtp.Authenticate(_globalSettings.Smtp.Username, _globalSettings.Smtp.Password);
                    }
                    smtp.Send(email);
                    smtp.Disconnect(true);

                    succes = true;
                }
            }
            catch
            {
                _logger.LogError($"Could not send mail {subject}");
            }

            return succes;

        }
        private static void SaveToPickupDirectory(MimeMessage message, string pickupDirectory)
        {
            do
            {
                // Generate a random file name to save the message to.
                var path = Path.Combine(pickupDirectory, Guid.NewGuid().ToString() + ".eml");
                Stream stream;

                try
                {
                    // Attempt to create the new file.
                    stream = File.Open(path, FileMode.CreateNew);
                }
                catch (IOException)
                {
                    // If the file already exists, try again with a new Guid.
                    if (File.Exists(path))
                        continue;

                    // Otherwise, fail immediately since it probably means that there is
                    // no graceful way to recover from this error.
                    throw;
                }

                try
                {
                    using (stream)
                    {
                        // IIS pickup directories expect the message to be "byte-stuffed"
                        // which means that lines beginning with "." need to be escaped
                        // by adding an extra "." to the beginning of the line.
                        //
                        // Use an SmtpDataFilter "byte-stuff" the message as it is written
                        // to the file stream. This is the same process that an SmtpClient
                        // would use when sending the message in a `DATA` command.
                        using (var filtered = new FilteredStream(stream))
                        {
                            filtered.Add(new SmtpDataFilter());

                            // Make sure to write the message in DOS (<CR><LF>) format.
                            var options = FormatOptions.Default.Clone();
                            options.NewLineFormat = NewLineFormat.Dos;

                            message.WriteTo(options, filtered);
                            filtered.Flush();
                            return;
                        }
                    }
                }
                catch
                {
                    // An exception here probably means that the disk is full.
                    //
                    // Delete the file that was created above so that incomplete files are not
                    // left behind for IIS to send accidentally.
                    File.Delete(path);
                    throw;
                }
            } while (true);
        }
    }
}
