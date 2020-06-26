using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using IdentityMvc.Models.Models;
using IdentityMvc.Data;
using System.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Diagnostics;
using IdentityMvc.CoreServices.PepiMail;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace IdentityMvc.CoreServices.Identity
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            string pepiMainEndpoint = ConfigurationManager.AppSettings["PEPI_MAIL_ENDPOINT"];
            string pepiApiKey = ConfigurationManager.AppSettings["PEPI_MAIL_KEY"];

            var mainContainer = new EmailContainer()
            {
                Content = message.Body,
                Subject = message.Subject,
                ReplyToId = "titilopeasare@pepisandbox.com",
                Personalizations = new MailPersonalization[]
                {
                    new MailPersonalization
                    {
                        Recipient = message.Destination
                    }
                },
                From = new MailFrom
                {
                    FromEmail = "titilopeasare@pepisandbox.com",
                    FromName = "Identity 2.0 Admin"
                }
            };

            string mailString = JsonConvert.SerializeObject(mainContainer, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            var client = new RestClient(pepiMainEndpoint);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("api_key", pepiApiKey);
            request.AddParameter("application/json", mailString, ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(request);

            return;
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            string accountSid = ConfigurationManager.AppSettings["TWILIO_SID"];
            string authKey = ConfigurationManager.AppSettings["TWILIO_TOKEN"];
            string srcPhone = ConfigurationManager.AppSettings["TWILIO_PHONE"];

            TwilioClient.Init(accountSid, authKey);

            var messageResponse = await MessageResource.CreateAsync(
                body: message.Body,
                from: new Twilio.Types.PhoneNumber(srcPhone),
                to: new Twilio.Types.PhoneNumber(message.Destination)
            );

            return;
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<UserAccount>
    {
        public ApplicationUserManager(IUserStore<UserAccount> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<UserAccount>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<UserAccount>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<UserAccount>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<UserAccount>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Google Authenticator Code", new GoogleaAuthTokenProvider());

            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<UserAccount>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<UserAccount, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(UserAccount user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
