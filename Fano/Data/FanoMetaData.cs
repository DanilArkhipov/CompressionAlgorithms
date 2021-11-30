using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fano.Data
{
    [Serializable]
    public class FanoMetaData
    {
        public SymbolCode[] Codes { get; set; }

        public FanoMetaData(SymbolCode[] codes)
        {
            Codes = codes;
        }
    }
}
