using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escola.Services.Records
{
    public record UserCreateRecord
    {
        public string UserName;
        public string Password;

        public UserCreateRecord(string UserName, string PassWord)
        {
            this.UserName = UserName;
            this.Password = PassWord;
        }
    }
}
