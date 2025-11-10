using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsSorter
{
    public class SorterConfig
    {
        public List<SorterRule> Rules { get; set; }
    }

    public class SorterRule
    {
        public string Extension { get; set; }
        public string Folder { get; set; }
    }

}
