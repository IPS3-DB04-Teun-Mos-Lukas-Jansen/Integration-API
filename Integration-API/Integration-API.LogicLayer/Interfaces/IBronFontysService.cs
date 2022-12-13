using Integration_API.Models.BronFontys;

namespace Integration_API.LogicLayer
{
    public interface IBronFontysService
    {
        Task<List<BronFontysResponse>> GetFeed(string UserId);
        Task SetBronFontysCredentials(string UserId, BronFontysCredentials credentials);
    }
}