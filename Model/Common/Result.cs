using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KarKhanaBook.Model.Common
{
    public class Result
    {
        public enum ResultStatus
        {
            none,
            success,
            danger,
            warning,
            info

        }

        public String Message { get; set; }
        public String Status { get; set; }
        public int StatusCode { get; set; }

    }
}
