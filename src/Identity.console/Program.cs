
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Stubble;
using Stubble.Core.Builders;

namespace Identity.console
{
    class Program
    {
        private static void Main()
        {
            //Execute().Wait();
            _StubbbleUssage();
        }

        private static void _StubbbleUssage()
        {
            var msg = "Hello {{name}}, You are welcome {{dest}}";
            var stubble = new StubbleBuilder().Build();
            //var stubble = new StubbleBuilder().Configure(s =>
            //{
            //    s.SetIgnoreCaseOnKeyLookup(true);
            //}).Build();

            var trl = stubble.Render(msg, new { Name = "Titilope", DEST = "Home" });
            Console.WriteLine(trl);
        }

        static async Task Execute()
        {
            string ApiKey = "dc949d0581bd05ddc37c1e7d800ca7ce";
            string emailEndpiont = "https://api.pepipost.com/v2/sendEmail";

            var mail = new PepiEmail()
            {
                Subject = "Welcome to identity 2.0",
                Content = " Welcome [%USER%], link is [%VERIFICATION_LINK%] ",
                TemplateId = 22628L,
                ReplyToId = "titilopeasare@gmail.com",
                From = new PepiEmailFrom
                {
                    FromEmail = "titilopeasare@pepisandbox.com",
                    FromName = "Identity 2.0 Admin"
                },
                Personalizations = new[]
                {
                    new PepiPersonalization
                    {
                        Recipient = "titi.secfocus@gmail.com",
                        Attributes =  new TempAtributed {
                           User = "Titilope" ,
                            VerificationLink = "https://www.google.com"
                        }
                    }
                }
            };

            string mailString = JsonConvert.SerializeObject(mail, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            var client = new RestClient(emailEndpiont);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("api_key", ApiKey);
            request.AddParameter("application/json", mailString, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);



        }

    }

    public class PepiEmail
    {
        public IEnumerable<PepiPersonalization> Personalizations { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public long? TemplateId { get; set; }
        public string ReplyToId { get; set; }
        public PepiEmailFrom From { get; set; }
        public IEnumerable<PepiAttachement> Attachements { get; set; }
    }

    public class TempAtributed
    {
        [JsonProperty("USER",NamingStrategyType = typeof(DefaultNamingStrategy))]
        public string User { get; set; }
        [JsonProperty("VERIFICATION_LINK", NamingStrategyType = typeof(DefaultNamingStrategy))]
        public string VerificationLink { get; set; }
    }

    public class PepiPersonalization
    {
        public string Recipient { get; set; }
        public object Attributes { get; set; }
        public IEnumerable<string> Recipient_cc { get; set; }
    }

    public class PepiEmailFrom
    {
        public string FromEmail { get; set; }
        public string FromName { get; set; }

    }

    public class PepiAttachement
    {
        public string fileContent { get; set; }
        public string fileName { get; set; }
    }
}
