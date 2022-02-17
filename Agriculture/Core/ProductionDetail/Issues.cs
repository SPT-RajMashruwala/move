using Agriculture.Middleware;
using Agriculture.Models.Common;
using Agriculture.Models.Search;
using ProductInventoryContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Core.ProductionDetails
{
    public class Issues
    {
        public List<SearchIssue> searchIssues = new List<SearchIssue>();
        public Result Add(Models.ProductionDetail.Issue value)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Issue i = new Issue();
                MAC login = new MAC();
                var MacAddress = login.GetMacAddress().Result;
                var LoginID = context.LoginDetails.FirstOrDefault(c => c.SystemMac == MacAddress);
                var qs = (from obj in value.issueDetails
                          select new Issue()
                          {

                              ProductID = obj.Product.Id,
                              MainAreaID = value.MainArea.Id,
                              SubAreaID = value.SubArea.Id,
                              Remark = obj.Remark,
                              LoginID = LoginID.LoginID,
                              DateTime = DateTime.Now,
                              PurchaseQuantity = obj.IssueQuantity,
                              IssueDate=value.Date.ToLocalTime(),
                              

                          }).ToList();
                var mainArea = (from obj in context.MainAreas
                                where obj.MainAreaID == value.MainArea.Id
                                select obj.MainAreaName).SingleOrDefault();
                foreach (var item in qs)
                {
                    var p = (from obj in context.Products
                             where obj.ProductID == item.ProductID
                             select obj.TotalProductQuantity).SingleOrDefault();
                    if (p < item.PurchaseQuantity)
                    {
                        var pname =(from obj in context.Products
                         where obj.ProductID == item.ProductID
                         select obj.ProductName).SingleOrDefault();
                        throw new ArgumentException($"Product name : {pname} ," +
                            $"Enter quantity : {item.PurchaseQuantity} more than existing quantity {p}");
                    }
                   

                }

                context.Issues.InsertAllOnSubmit(qs);
                context.SubmitChanges();


                foreach (var item in qs)
                {
                    var updatePQ = context.Products.FirstOrDefault(c => c.ProductID == item.ProductID);
                    updatePQ.TotalProductQuantity = updatePQ.TotalProductQuantity - item.PurchaseQuantity;
                    context.SubmitChanges();

                }

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Issue Products Successful",
                    Data = $"Total {qs.Count()} Product Issue For Main Area : {mainArea} ",
                };

            }
        }
        public Result ViewById(int ID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {

                var qs = (from obj in context.Issues
                         /* join obj2 in context.MainAreas
                          on obj.MainAreaID equals obj2.MainAreaID into JoinTableMA
                          from MA in JoinTableMA.DefaultIfEmpty()
                          join obj3 in context.SubAreas
                          on obj.SubAreaID equals obj3.SubAreaID into JoinTableSA
                          from SA in JoinTableSA.DefaultIfEmpty()
                          join obj4 in context.Products
                          on obj.ProductID equals obj4.ProductID into JoinTablePD
                          from PD in JoinTablePD.DefaultIfEmpty()*/
                          where obj.IssueID == ID
                          select new
                          {
                              IssueID = obj.IssueID,
                              Product=new IntegerNullString() { Id=obj.Product.ProductID,Text=obj.Product.ProductName},
                              MainArea=new IntegerNullString() { Id=obj.MainArea.MainAreaID,Text=obj.MainArea.MainAreaName},
                              SubAreaName = new IntegerNullString() { Id=obj.SubArea.SubAreaID,Text=obj.SubArea.SubAreaName},
                              IssueQuantity = obj.PurchaseQuantity,
                              Remark = obj.Remark,
                              Date=obj.DateTime,


                          }).ToList();
                return new Result()
                {
                    Status=Result.ResultStatus.success,
                    Message="Issue ViewByID",
                    Data=qs,
                };

            }
        }
        public Result View()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var qs = (from obj in context.Issues
                    /*      join obj2 in context.MainAreas
                          on obj.MainAreaID equals obj2.MainAreaID into JoinTableMA
                          from MA in JoinTableMA.DefaultIfEmpty()
                          join obj3 in context.SubAreas
                          on obj.SubAreaID equals obj3.SubAreaID into JoinTableSA
                          from SA in JoinTableSA.DefaultIfEmpty()
                          join obj4 in context.Products
                          on obj.ProductID equals obj4.ProductID into JoinTablePD
                          from PD in JoinTablePD.DefaultIfEmpty()*/
                          select new
                          {
                              IssueID = obj.IssueID,
                              Product = new IntegerNullString() { Id = obj.Product.ProductID, Text = obj.Product.ProductName },
                              MainArea = new IntegerNullString() { Id = obj.MainArea.MainAreaID, Text = obj.MainArea.MainAreaName },
                              SubAreaName = new IntegerNullString() { Id = obj.SubArea.SubAreaID, Text = obj.SubArea.SubAreaName },
                              IssueQuantity = obj.PurchaseQuantity,
                              Remark = obj.Remark,
                              Date = obj.DateTime,


                          }).ToList();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Issue View",
                    Data = qs,
                };
            }
        }
        public Result ViewSearch(DataTable table) 
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext()) 
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow dr = table.Rows[i];
                    searchIssues.Add(new Models.Search.SearchIssue
                    {
                      
                        IssueID = Int16.Parse(dr["IssueID"].ToString()),
                        IssueDate= Convert.ToDateTime(dr["IssueDate"].ToString()),
                        MainAreaName= dr["MainAreaName"].ToString(),
                        SubAreaName= dr["SubAreaName"].ToString(),
                        ProductName= dr["ProductName"].ToString(),
                        PurchaseQuantity=float.Parse(dr["PurchaseQuantity"].ToString()),
                        Remark = dr["Remark"].ToString(),
                        UserName = dr["UserName"].ToString(),
                        DateTime = Convert.ToDateTime(dr["DateTime"].ToString()),
                        


                    });

                }
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Issue View",
                    Data = searchIssues,
                };

            }

        }
        public Result Update(Models.ProductionDetail.Issue value, int ID)
        {
            float RemainQuantity = 0;
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                MAC mac = new MAC();
                var macObj = mac.GetMacAddress().Result;
                var MacAddress = context.LoginDetails.FirstOrDefault(c => c.SystemMac == macObj);
                var qs = (
                         from obj in context.Issues
                         where obj.IssueID == ID
                         select obj).SingleOrDefault();
                var p = (from obj in value.issueDetails
                         select obj).SingleOrDefault();
                var pd = (from obj in context.Products
                          where obj.ProductID == qs.ProductID
                          select obj).SingleOrDefault();
                var ps = (from obj in context.Products
                          where obj.ProductID == p.Product.Id
                          select obj).SingleOrDefault();
                var pid = (from obj in context.Issues
                           where obj.SubAreaID == value.SubArea.Id
                           select new
                           {
                               ProductID = obj.ProductID
                           }).ToList();


                if (qs.SubAreaID == value.SubArea.Id)
                {
                    if (qs.ProductID == p.Product.Id)
                    {
                        var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                        RemainQuantity = (float)temp - p.IssueQuantity;
                        qs.DateTime = DateTime.Now;
                        qs.ProductID = p.Product.Id;
                        qs.MainAreaID = value.MainArea.Id;
                        qs.SubAreaID = value.SubArea.Id;
                        qs.LoginID = MacAddress.LoginID;
                        qs.Remark = p.Remark;
                        qs.PurchaseQuantity = p.IssueQuantity;

                        ps.TotalProductQuantity = RemainQuantity;
                        context.SubmitChanges();
                        return new Result()
                        {
                            Status=Result.ResultStatus.success,
                            Message="Issue Update Successfully",
                            Data=$"Issue ID : {ID} updated successfully",
                        };
                    }
                    else
                    {
                        foreach (var item in pid)
                        {
                            if (p.Product.Id == item.ProductID)
                            {
                                throw new Exception($"Entered Product : {item.ProductID} already issued for given sub" +
                                    $" area : {value.SubArea.Text}");

                            }





                        }
                        var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                        qs.DateTime = DateTime.Now;
                        qs.ProductID = p.Product.Id;
                        qs.MainAreaID = value.MainArea.Id;
                        qs.SubAreaID = value.SubArea.Id;
                        qs.LoginID = MacAddress.LoginID;
                        qs.Remark = p.Remark;
                        qs.PurchaseQuantity = p.IssueQuantity;
                        RemainQuantity = (float)ps.TotalProductQuantity - p.IssueQuantity;
                        ps.TotalProductQuantity = RemainQuantity;
                        pd.TotalProductQuantity = temp;
                        context.SubmitChanges();
                        return new Result()
                        {
                            Status = Result.ResultStatus.success,
                            Message = "Issue Update Successfully",
                            Data = $"Issue ID : {ID} updated successfully",
                        };
                    }



                }
                else
                {
                    foreach (var item in pid)
                    {
                        if (p.Product.Id == item.ProductID)
                        {
                            throw new Exception($"Entered Product : {item.ProductID} already issued for given sub" +
                                $" area : {value.SubArea.Text}");

                        }




                    }
                    var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                    pd.TotalProductQuantity = temp;
                    qs.DateTime = DateTime.Now;
                    qs.ProductID = p.Product.Id;
                    qs.MainAreaID = value.MainArea.Id;
                    qs.SubAreaID = value.SubArea.Id;
                    qs.LoginID = MacAddress.LoginID;
                    qs.Remark = p.Remark;
                    qs.PurchaseQuantity = p.IssueQuantity;
                    RemainQuantity = (float)ps.TotalProductQuantity - p.IssueQuantity;
                    ps.TotalProductQuantity = RemainQuantity;
                    context.SubmitChanges();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Issue Update Successfully",
                        Data = $"Issue ID : {ID} updated successfully",
                    };

                }

                /*   qs.DateTime = DateTime.Now;
                   qs.ProductID = p.Product.Id;
                   qs.MainAreaID = issueModel.MainArea.Id;
                   qs.SubAreaID = issueModel.SubArea.Id;
                   qs.UserLoginID = mac.LoginID;
                   qs.Remark = p.Remark;
                   qs.PurchaseQuantity = p.IssueQuantity;

                   ps.TotalProductQuantity = RemainQuantity;
                   context.SubmitChanges();

                   return "";
   */


            }

        }
    }
}
