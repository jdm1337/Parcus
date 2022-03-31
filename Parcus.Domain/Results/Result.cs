using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.Results
{
    public class Result<T>
    {
        public bool Succeeded { get; set; } = false;
        public T? Item { get; set; }
        public List<T>? Items { get; set; }

    }
}
