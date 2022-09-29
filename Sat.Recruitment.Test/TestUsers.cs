using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Sat.Recruitment.Api;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Config;
using Sat.Recruitment.DataAccess;
using Sat.Recruitment.UsersBL;
using System.Net;
using Xunit;

namespace Sat.Recruitment.Test
{
    public class ServicesFixture
    {
        public ServicesFixture()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging();
            serviceCollection.AddScoped<IUsersDA, FileDataAccess>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
        public ServiceProvider ServiceProvider { get; private set; }
    }


    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class TestUsers : IClassFixture<ServicesFixture>
    {
        private ServiceProvider _serviceProvider;
        private IOptions<UsersTypesConfig> _optionsMock;
        public TestUsers(ServicesFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;

            _optionsMock = Substitute.For<IOptions<UsersTypesConfig>>();
            // Mocking config
            _optionsMock.Value.Returns(new UsersTypesConfig
            {
                Default = "0",
                NormalMin = "0.8",
                NormalMax = "0.12",
                SuperUser = "0.20",
                Premium = "2"
            });
        }

        [Fact]
        public async void Test_onSuccess()
        {

            var logger = _serviceProvider.GetService<ILogger<UsersController>>();
            var loggerUser = _serviceProvider.GetService<ILogger<UserBL>>();
            var dataAccess = _serviceProvider.GetService<IUsersDA>();

            var userController = new UsersController(logger, new UserBL(loggerUser, dataAccess, _optionsMock));

            UserDto userDto = new UserDto()
            {
                Name = "Mike",
                Email = "mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = 124
            };

            var result = await userController.CreateUser(userDto);

            Assert.Equal(((int)HttpStatusCode.OK), ((IStatusCodeActionResult)result).StatusCode);
        }

        [Fact]
        public async void Test_onFailure()
        {
            var logger = _serviceProvider.GetService<ILogger<UsersController>>();
            var loggerUser = _serviceProvider.GetService<ILogger<UserBL>>();
            var dataAccess = _serviceProvider.GetService<IUsersDA>();

            var userController = new UsersController(logger, new UserBL(loggerUser, dataAccess, _optionsMock));

            UserDto userDto = new UserDto()
            {
                Name = "Agustina",
                Email = "Agustina@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = 124
            };

            var result = await userController.CreateUser(userDto);

            Assert.Equal(((int)HttpStatusCode.Conflict), ((IStatusCodeActionResult)result).StatusCode);
        }
    }
}
