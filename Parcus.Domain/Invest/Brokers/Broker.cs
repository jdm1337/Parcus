using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.Invest.Brokers
{
    public class Broker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Percentage { get; set;}
        // Broker logo image
    }
}
