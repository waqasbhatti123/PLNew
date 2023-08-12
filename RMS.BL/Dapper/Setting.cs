using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL.Dapper
{
    public class Setting
    {
        public static string ConnectionString { get; set; } = "Server=tcp:s24.winhost.com;User=db_140886_pac_user;Password=tsoft$;Database=db_140886_pac;";
        public static int CommandTimout { get; set; } = 300;

    }
}
