using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IAuthenticationService
    {
        Task<Token?> ValiteUser(UserAuthentication userAuthentication);
      Token CreateToken(bool populateExp);
    }
}
