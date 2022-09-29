using Microsoft.Extensions.Logging;
using Sat.Recruitment.Config;
using Sat.Recruitment.DataAccess;
using Sat.Recruitment.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.UsersBL
{
    public abstract class UserCreator
    {
        private User _user = null;
        private readonly IUsersDA _usersDA = null;
        private readonly ILogger _logger;
        private readonly UsersTypesConfig _options;
        public UserCreator() { }

        public UserCreator(User user, IUsersDA usersDA, ILogger logger, UsersTypesConfig options) 
        {
            _user = user;
            _usersDA = usersDA;
            _logger = logger;
            _options = options;
        }
        protected abstract void GetUserGif(User user);

        public async Task<bool> RegisterUser()
        {
            _logger.LogDebug("Calculate user type gift.");
            GetUserGif(this._user);

            var isDuplicated = false;
            // Get Users from DA
            _logger.LogDebug("Get users from DB.");
            List<User> users = await _usersDA.GetUsers();

            foreach (var user in users)
            {
                if (user.Email == _user.Email
                    ||
                    user.Phone == _user.Phone)
                {
                    isDuplicated = true;
                }
                else if (user.Name == _user.Name)
                {
                    if (user.Address == _user.Address)
                    {
                        isDuplicated = true;
                    }
                }
            }
            return isDuplicated;
        }


    }
}
