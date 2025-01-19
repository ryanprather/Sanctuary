using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanctuary.CsvDataReader.Models
{
    public class DataFileEndpoint
    {
        public string Name { get; set; }
        public Type DataType { get; set; }
        public int? Index { get; set; }
    }
}
