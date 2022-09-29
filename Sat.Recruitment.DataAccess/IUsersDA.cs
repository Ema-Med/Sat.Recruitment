using Sat.Recruitment.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.DataAccess
{
    public interface IUsersDA
    {
        public Task<List<User>> GetUsers();
    }
}
