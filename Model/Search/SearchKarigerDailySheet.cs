using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KarKhanaBook.Model.Search
{
    public class SearchKarigerDailySheet
    {

       
            public int IndexNumber { get; set; }
         
            public Model.Common.IntegerNullString UserName { get; set; } = new Common.IntegerNullString();
            
            public Model.Common.IntegerNullString Shift { get; set; } = new Model.Common.IntegerNullString();
           

            public DateTime Date { get; set; }
            public int MachineNumber { get; set; }

            public float AVGOfMachine { get; set; }
            public string Remark { get; set; }









    }
        
        
        
} 
