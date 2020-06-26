namespace IdentityMvc.CoreServices.Identity
{
    public interface IGoogleAuthenticatorService
    {
        byte[] SecreteKey { get; }
        string GetQRCode(string userId);
        bool ValidatCode(byte[] key, string code);
    }
}