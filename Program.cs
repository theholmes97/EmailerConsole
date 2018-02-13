using System;
using System.Collections.Generic;
using MailMergeLib;

namespace Mail
{
    class MainClass
    {
        static Dictionary<string, string> values = new Dictionary<string, string>(){{"Email", "willholmes@email.com"}, {"Name", "Will Holmes"}};

        public static void Main(string[] args)
        {
            //here we want to use the mailmergesender to allow us to send our message
            using(var emailSender = ConfigSender()){
                //we cast values to an object as it must be boxed...
                emailSender.Send(ConfigMessage(), (object)values);
            }
        }

        public static MailMergeMessage ConfigMessage(){
            //here we build up our subject and message in the constructor of the email...
            var mailMessage = new MailMergeMessage("An email for {Name}",
                                           "A simple message for {Name}");
            //here we build up our from and to addresses, we use our values from our dictionary for the to...
            mailMessage.MailMergeAddresses.Add(new MailMergeAddress(MailAddressType.From, "will@willsemailhost.com"));
            mailMessage.MailMergeAddresses.Add(new MailMergeAddress(MailAddressType.To, "{Name}", "{Email}"));

            return mailMessage;
        }

        public static MailMergeSender ConfigSender(){
            var sender = new MailMergeSender();
            sender.Config.MaxNumOfSmtpClients = 1;  // enough for a single email
            sender.Config.SmtpClientConfig[0].MessageOutput = MessageOutput.SmtpServer;
            sender.Config.SmtpClientConfig[0].SmtpHost = "smtp.mailprovider.net";
            sender.Config.SmtpClientConfig[0].SmtpPort = 587;
            sender.Config.SmtpClientConfig[0].NetworkCredential = new Credential("username", "password");
            sender.Config.SmtpClientConfig[0].MaxFailures = 3; // more throw an exception
            return sender;
        }
    }
}
