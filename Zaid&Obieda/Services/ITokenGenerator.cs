using Zaid_Obieda.Models;

namespace Zaid_Obieda.Services
{
    public interface ITokenGenerator
    {
        public string CreateToken(AppUser user);
    }
}
