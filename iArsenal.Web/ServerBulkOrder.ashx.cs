using System;
using System.Web;
using System.Web.Script.Serialization;

using iArsenal.Entity;

namespace iArsenal.Web
{
    public class ServerBulkOrder : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string responseText = string.Empty;

            if (!string.IsNullOrEmpty(context.Request.QueryString["SelectedOrderIDs"]))
            {
                try
                {
                    string[] arrayOrderIDs = context.Request.QueryString["SelectedOrderIDs"].Split('|');

                    int countSucceed = 0;
                    int countFailed = 0;

                    if (arrayOrderIDs.Length > 0)
                    {
                        foreach (string strID in arrayOrderIDs)
                        {
                            int _id;
                            if (int.TryParse(strID, out _id))
                            {
                                try
                                {
                                    Order o = new Order();
                                    o.OrderID = _id;
                                    o.Select();

                                    if (o != null && o.Status.Equals(OrderStatusType.Confirmed))
                                    {
                                        o.Status = OrderStatusType.Ordered;
                                        o.UpdateTime = DateTime.Now;
                                        o.Update();

                                        countSucceed++;
                                        continue;
                                    }
                                    else
                                    {
                                        countFailed++;
                                        continue;
                                    }
                                }
                                catch
                                {
                                    countFailed++;
                                    continue;
                                }
                            }
                            else
                            {
                                countFailed++;
                                continue;
                            }
                        }

                        var returnObj = new
                        {
                            result = "success",
                            countTotal = arrayOrderIDs.Length,
                            countSucceed = countSucceed,
                            countFailed = countFailed
                        };

                        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                        responseText = jsonSerializer.Serialize(returnObj);
                    }
                    else
                    {
                        throw new Exception("invalid Selected OrderIDs");
                    }
                }
                catch (Exception ex)
                {
                    responseText = string.Format("{{  \"result\": \"error\", \"error_msg\": \"{0}\" }}", ex.Message);
                }
            }

            //responseText = "{  \"result\": \"success\", \"countTotal\": \"10\", \"countSucceed\": \"8\", \"countFailed\": \"2\" }";

            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.Write(responseText);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}