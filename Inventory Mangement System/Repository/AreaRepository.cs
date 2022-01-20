using Inventory_Mangement_System.Model;
using ProductInventoryContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public class AreaRepository:IAreaRepository
    {
        //New Main Area Add 
        public async Task<string> AddMainAreaAsync(MainAreaModel mainAreaModel)
        {
            using(ProductInventoryDataContext context=new ProductInventoryDataContext())
            {
                MainArea mainArea = new MainArea();
                var MA = context.MainAreas.SingleOrDefault(x => x.MainAreaName == mainAreaModel.mname);
                if(MA != null)
                {
                    var id = context.MainAreas.SingleOrDefault(x => x.MainAreaName == mainAreaModel.mname);
                    var sd1 = mainAreaModel.subarea.Select(x => new SubArea()
                    {
                        MainAreaID = id.MainAreaID,
                        SubAreaName = x.sname
                    }).ToList();
                    context.SubAreas.InsertAllOnSubmit(sd1);
                    context.SubmitChanges();
                    return "Area Added Successfully";
                }
                else
                {
                    mainArea.MainAreaName = mainAreaModel.mname;
                    context.MainAreas.InsertOnSubmit(mainArea);
                    context.SubmitChanges();

                    var Maid = context.MainAreas.SingleOrDefault(x => x.MainAreaName == mainAreaModel.mname);
                    var sd = mainAreaModel.subarea.Select(x => new SubArea()
                    {
                        MainAreaID = Maid.MainAreaID,
                        SubAreaName = x.sname
                    }).ToList();
                    context.SubAreas.InsertAllOnSubmit(sd);
                    context.SubmitChanges();
                    return "Area Added Successfully";
                }
            }
        }

       
    }
}
