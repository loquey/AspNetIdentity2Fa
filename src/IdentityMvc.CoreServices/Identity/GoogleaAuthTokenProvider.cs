using Base32;
using IdentityMvc.Models.Models;
using Microsoft.AspNet.Identity;
using OtpSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMvc.CoreServices.Identity
{
    public class GoogleaAuthTokenProvider : IUserTokenProvider<UserAccount, string>
    {
        public Task<string> GenerateAsync(string purpose, UserManager<UserAccount, string> manager, UserAccount user)
        {
            return Task.FromResult((string)null);
        }

        public Task<bool> IsValidProviderForUserAsync(UserManager<UserAccount, string> manager, UserAccount user)
        {
            return Task.FromResult(user.IsGoogleAuthenticatorEnabled);
        }

        public Task NotifyAsync(string token, UserManager<UserAccount, string> manager, UserAccount user)
        {
            return Task.FromResult(true);
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<UserAccount, string> manager, UserAccount user)
        {
            long timeStepMatched = 0;
            Totp totp = new Totp(Base32Encoder.Decode(user.GoogleAuthenticatorSecretKey));
            bool IsValid = totp.VerifyTotp(token, out timeStepMatched, new VerificationWindow(2, 2));
            return Task.FromResult(IsValid);
        }
    }
}
