using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms.Data
{
    /// <summary>
    /// Класс содержащий закодирвоанные данные, которые будт сохранены в файл
    /// </summary>
    [Serializable]
    public class EncodedData
    {
        /// <summary>
        /// Метаданные
        /// </summary>
        public dynamic MetaDada { get; set; }

        /// <summary>
        /// Данные
        /// </summary>
        public BitArray Data { get; set; }

        public EncodedData(FanoMetaData metaData, BitArray data)
        {
            MetaDada = metaData;
            Data = data;
        }
    }
}
