using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escola.Services.Records
{
    public record StudentsImportErrorRecord(
        int Line,
        string Field,
        string Message
    );

    public record StudentsImportResultRecord(
        int Total,
        int Imported,
        int Failed,
        List<StudentsImportErrorRecord> Errors
    );
}
