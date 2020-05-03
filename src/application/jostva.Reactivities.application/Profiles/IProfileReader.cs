using System.Threading.Tasks;

namespace jostva.Reactivities.application.Profiles
{
    public interface IProfileReader
    {
        Task<Profile> ReadProfile(string username);
    }
}