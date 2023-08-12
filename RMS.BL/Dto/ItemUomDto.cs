using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL.Dto
{
    public class ItemUomDto
    {
        public int uom_cd { get; set; }
        public string uom_dsc { get; set; }

        /*custom fields*/
        public string Submit { get; set; }
    }
}
