using Sat.Recruitment.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Sat.Recruitment.DataAccess
{
    public class FileDataAccess : IUsersDA
    {

        public async Task<List<User>> GetUsers()
        {
            try
            {
                List<User> users = new List<User>();
                var path = Directory.GetCurrentDirectory() + "/Files/Users.txt"; //TODO: Get path from config

                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    StreamReader reader = new StreamReader(fileStream);

                    while (reader.Peek() >= 0)
                    {
                        var line = await reader.ReadLineAsync();
                        var user = new User
                        {
                            Name = line.Split(',')[0].ToString(),
                            Email = line.Split(',')[1].ToString(),
                            Phone = line.Split(',')[2].ToString(),
                            Address = line.Split(',')[3].ToString(),
                            UserType = line.Split(',')[4].ToString(),
                            Money = decimal.Parse(line.Split(',')[5].ToString()),
                        };
                        users.Add(user);
                    }
                    reader.Close();
                }
                return users;
            }
            catch
            {
                throw new Exception("Users could not be read");
            }
        }
    }
}
