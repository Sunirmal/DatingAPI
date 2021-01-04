using DatingAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAPI.Interfaces
{
    public interface ITokerService
    {
        string CreateToken(AppUser user);
    }
}
