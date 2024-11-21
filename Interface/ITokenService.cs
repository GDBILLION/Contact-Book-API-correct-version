using Registration.Models;

namespace Registration.Interface
{   
      public interface ITokenService
        {
            public string CreateToken(User user);
        }
    
}
