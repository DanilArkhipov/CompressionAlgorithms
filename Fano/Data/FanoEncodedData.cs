using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fano.Data
{
    /// <summary>
    /// Класс содержащий закодирвоанные данные, которые будт сохранены в файл
    /// </summary>
    [Serializable]
    public class FanoEncodedData
    {
        /// <summary>
        /// Метаданные
        /// </summary>
        public FanoMetaData MetaDada { get; set; }

        /// <summary>
        /// Данные
        /// </summary>
        public BitArray Data { get; set; }

        public FanoEncodedData(FanoMetaData metaData, BitArray data)
        {
            MetaDada = metaData;
            Data = data;
        }
    }
}
