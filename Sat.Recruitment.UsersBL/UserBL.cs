using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sat.Recruitment.Config;
using Sat.Recruitment.DataAccess;
using Sat.Recruitment.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sat.Recruitment.UsersBL
{
    public class UserBL
    {
        private readonly IUsersDA _usersDA;
        private readonly ILogger _logger;
        private readonly UsersTypesConfig _options;
        public UserBL(ILogger<UserBL> logger, IUsersDA usersDA, IOptions<UsersTypesConfig> options)
        {
            _logger = logger;
            _usersDA = usersDA;
            _options = options.Value;
        }

        public async Task<bool> CreateUser(User user)
        {
            UserCreator creator = BuildUserCreator(user);
            return await creator.RegisterUser();
        }

        private UserCreator BuildUserCreator(User user)
        {
            _logger.LogDebug($"User type -> [{user.UserType}].");

            return user.UserType switch
            {
                "Normal" => new NormalUserCreator(user, _usersDA, _logger, _options),
                "SuperUser" => new SuperUserCreator(user, _usersDA, _logger, _options),
                "Premium" => new PremiumUserCreator(user, _usersDA, _logger, _options),
                _ => new DefaultUserCreator(user, _usersDA, _logger, _options) // if not a valit user, return default type.
                //_ => throw new InvalidOperationException("Cannot create user without valid user type.")
            };
        }

    }
}
