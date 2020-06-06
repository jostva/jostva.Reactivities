using jostva.Reactivities.application.User;
using System.Threading.Tasks;

namespace jostva.Reactivities.application.Interfaces
{
    public interface IFacebookAccesor
    {
        Task<FacebookUserInfo> FacebookLogin(string accessToken);
    }
}