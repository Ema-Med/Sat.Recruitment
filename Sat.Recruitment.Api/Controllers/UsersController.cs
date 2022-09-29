using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Entities;
using Sat.Recruitment.UsersBL;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UserBL _userBL;

        public UsersController(ILogger<UsersController> logger, UserBL userBL)
        {
            _logger = logger;
            _userBL = userBL;
        }

        [HttpPost]
        [Route("/create-user")]
        public async Task<IActionResult> CreateUser(UserDto user)
        {
            try
            {
                User newUser = new User()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Address = user.Address,
                    Phone = user.Phone,
                    UserType = user.UserType,
                    Money = user.Money
                };

                _logger.LogInformation("New user received.");
                var isDuplicated = await _userBL.CreateUser(newUser);
                if(isDuplicated)
                {
                    throw new InvalidOperationException();
                }


                var response = new Result()
                {
                    IsSuccess = true,
                    Description = "User Created"
                };

                _logger.LogInformation(response.Description);
                return Ok(response);

            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid Operation");
                return StatusCode(((int)HttpStatusCode.Conflict), new Result()
                {
                    IsSuccess = false,
                    Description = "The user is duplicated"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal error - Failed to create user");
                return StatusCode(((int)HttpStatusCode.InternalServerError), new Result()
                {
                    IsSuccess = false,
                    Description = "Internal error - Failed to create user"
                });
            }
        }
    }
}
