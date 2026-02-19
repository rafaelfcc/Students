using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escola.Services.Records
{
    public record UserDeleteRecord
    {
        public int? Id;
        public string? UserName;
    }
}
