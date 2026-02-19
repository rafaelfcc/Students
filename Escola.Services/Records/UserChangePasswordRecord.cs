using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escola.Services.Records
{
    public record UserChangePasswordRecord
    {
        public int? UserId;
        public string? UserName;
        public string? PrevPassword;
        public string? NewPassword;
    }
}
