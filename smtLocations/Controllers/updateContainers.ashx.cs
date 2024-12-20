using smtLocations.Class;
using smtLocations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smtLocations.Controllers
{
    /// <summary>
    /// Summary description for updateContainers
    /// </summary>
    public class updateContainers : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            String json = "{";
            try
            {
                COpenInter m_open = new COpenInter();
                List < CItem> items = m_open.getContents("HITACHISM");
                //List<CItem> items = m_open.getContents("LOCA1575246");
                siixsem_smtLocations_dbEntities m_db = new siixsem_smtLocations_dbEntities();

                m_db.clearCurQty();

                foreach (CItem item in items)
                {
                    m_db.updateItem(item.Part_number,item.Expired,item.Qty);
                }
                json += "\"result\":\"true\"";

            }
            catch(Exception ex)
            {
                json += "\"result\":\"false\",";
                json += "\"html\":\"" + ex.Message.Replace("\"","'") + "\"";
            }
            json += "}";
            context.Response.ContentType = "text/plain";
            context.Response.Write(json);
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