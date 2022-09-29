using Microsoft.Extensions.Logging;
using Sat.Recruitment.Config;
using Sat.Recruitment.DataAccess;
using Sat.Recruitment.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sat.Recruitment.UsersBL
{
    public class DefaultUserCreator : UserCreator
    {
        private readonly UsersTypesConfig _options;
        public DefaultUserCreator(User user, IUsersDA usersDA, ILogger logger, UsersTypesConfig options)
            : base(user, usersDA, logger, options) {
            _options = options;
        }

        protected override void GetUserGif(User user)
        {
            var percentage = Convert.ToDecimal(_options.Default);
            var gif = user.Money * percentage;
            user.Money += gif;
        }
    }
}
