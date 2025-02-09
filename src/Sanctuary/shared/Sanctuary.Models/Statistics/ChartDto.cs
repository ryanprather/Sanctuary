using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanctuary.Models.Statistics
{
    public class ChartDto
    {
        public string ChartType { get; set; }
        public List<string> Labels { get; set; }
        public string DatasetLabel { get; set; }
        public List<decimal> Data { get; set; }
    }
}
