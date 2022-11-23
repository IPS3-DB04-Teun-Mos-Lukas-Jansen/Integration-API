namespace Integration_API.LogicLayer
{
    public interface IIntegrationsHelper
    {
        Task<string> GetIntegrationCredentials(string userId);
        Task<int> RemoveIntegrationCredentials(string userId, string integrationName);
    }
}