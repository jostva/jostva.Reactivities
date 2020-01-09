using jostva.Reactivities.Domain;

namespace jostva.Reactivities.application.Interfaces
{
    public interface IJwtGenerator
    {

        string CreateToken(AppUser user);
    }
}