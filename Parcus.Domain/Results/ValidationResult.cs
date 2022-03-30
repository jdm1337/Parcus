using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.Results
{
    public class ValidationResult
    {
        public bool Succeeded { get; set; } = false;
        public List<string> Errors { get; set; }
    }
}
