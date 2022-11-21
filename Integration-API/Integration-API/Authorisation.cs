
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;

namespace Integration_API
{
    public static class Authorisation
    {
        public async static Task<string> ValidateIdToken(string id_token)
        {
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(id_token);
            return payload.Subject;
        }
    }
}
