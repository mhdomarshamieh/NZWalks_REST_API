using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repositories
{
    public interface ITokenReposiroty
    {

        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
