using OtpSharp;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMvc.CoreServices.Identity
{
    public interface IGoogleAuthenticatorService1
    {
        byte[] SecreteKey { get; }

        string GetQRCode();
    }

    public class GoogleAuthenticatorService : IGoogleAuthenticatorService
    {
        public byte[] SecreteKey { get; private set; }
        private string _KeyUrl;

        public GoogleAuthenticatorService()
        {
            SecreteKey = KeyGeneration.GenerateRandomKey(20);
        }

        private void GetKeyUrl(string userId)
        {
            string totpUrl = KeyUrl.GetTotpUrl(SecreteKey, userId);

            _KeyUrl = $"{totpUrl}&issuer=AspNetIdentityTest";
        }

        public string GetQRCode(string userId)
        {
            GetKeyUrl(userId);
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qRCodeGenerator.CreateQrCode(_KeyUrl, QRCodeGenerator.ECCLevel.Q);
            Base64QRCode qrCode = new Base64QRCode(qrCodeData);
            return qrCode.GetGraphic(10);
        }

        public bool ValidatCode(byte[] key, string code)
        {
            var totp = new Totp(key);
            long timeStepMatched = 0;
            return totp.VerifyTotp(code, out timeStepMatched, new VerificationWindow(2, 2));
        }
    }
}
