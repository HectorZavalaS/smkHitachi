using smtLocations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml;

namespace smtLocations.Class
{
    public class COpenInter
    {
        private wsCogiscan.RPCServices rPCServices;
        siixsem_smtLocations_dbEntities m_db;

        public COpenInter()
        {
            rPCServices = new wsCogiscan.RPCServicesClient();
            m_db = new siixsem_smtLocations_dbEntities();
        }

        public List<CItem> getContents(String container)
        {
            List<CItem> items = new List<CItem>();
            m_db.deleteReels();

            String parameters = "<Parameter name=\"containerId\">" + container + "</Parameter>";

            try
            {

                String xml = rPCServices.executeCommand("getContents", "<Parameters>" + parameters + "</Parameters>");
                String locat = "";

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                //XmlElement root = doc.DocumentElement;

                XmlNodeList xml_items = doc.SelectNodes("Contents/Item");
                foreach (XmlNode item in xml_items)
                {   //list.Where(w => w.Name == "height").ToList().ForEach(s => s.Value = 30);
                    CItem i = new CItem();
                    CReel reel = new CReel();

                    reel.Item_id = item.Attributes["itemId"].Value;
                    //reel.Item_id = item.Attributes["locationInTool"].Value;


                    XmlNode rMat = item.SelectSingleNode("RawMaterial");
                    if (rMat != null)
                    {
                        i.Part_number = rMat.Attributes["partNumber"].Value;
                        reel.Init_qty = (int)Convert.ToDouble(rMat.Attributes["initialQuantity"].Value);
                        reel.Curr_qty = (int)Convert.ToDouble(rMat.Attributes["quantity"].Value.ToString());

                        XmlNode expired = item.SelectSingleNode("MSD");
                        if (expired != null)
                        {
                            reel.Expired = i.Expired = expired.Attributes["expired"].Value == "true" ? 1 : 0;
                        }
                        else i.Expired = 0;
                        if (items.Exists(x => x.Part_number == i.Part_number))
                        {
                            items.Where(w => w.Part_number == i.Part_number).ToList().ForEach(s => s.Qty = s.Qty + 1);
                            items.Where(w => w.Part_number == i.Part_number).ToList().ForEach(s => s.Add(reel, i.Part_number));
                        }
                        else
                        {
                            i.Qty = 1;
                            i.Add(reel, i.Part_number);
                            items.Add(i);
                        }
                        locat += "'" + i.Part_number + "',";
                    }

                    //Thread.Sleep(200);

                }
                m_db.clearLocations(locat.Substring(0, locat.Length - 1));
            }
            catch (Exception ex)
            {
                //model.BatchId = ex.Message;
                //model.DjNumber = "false";
            }

            return items;
        }
    }
}