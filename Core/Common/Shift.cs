using KarKhanaBook.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KarKhanaBook.Core.Common
{
    public class Shift
    {
       
            public List<IntegerNullString> shifts { get; set; } = new List<IntegerNullString>()
            {
                new IntegerNullString(){ ID=1,Text="Morning",},
                new IntegerNullString(){ ID=2,Text="Evening"}
            };
        
    }
}
