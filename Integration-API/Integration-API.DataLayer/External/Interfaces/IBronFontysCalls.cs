using System.ServiceModel.Syndication;

namespace Integration_API.DataLayer.External
{
    public interface IBronFontysCalls
    {
        Task<SyndicationFeed> GetNewsFeed();
    }
}