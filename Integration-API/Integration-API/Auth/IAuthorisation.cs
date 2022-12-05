namespace Integration_API.Auth
{
    public interface IAuthorisation
    {
        Task<string> ValidateIdToken(string id_token);
    }
}