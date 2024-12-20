using smtLocations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smtLocations.Class
{
    public class CItem
    {
        private String m_part_number;
        private int m_expired;
        private int m_qty;
        siixsem_smtLocations_dbEntities m_db;
        private String strError;

        public CItem()
        {
            m_expired = 0;
            m_part_number = "";
            m_qty = 0;
            Reels = new List<CReel>();
            m_db = new siixsem_smtLocations_dbEntities();
        }
        public string Part_number { get => m_part_number; set => m_part_number = value; }
        public int Expired { get => m_expired; set => m_expired = value; }
        public int Qty { get => m_qty; set => m_qty = value; }
        public List<CReel> Reels { get; set; }

        public void Add(CReel reel, String Loc)
        {
            try
            {
                m_db.insert_reel(reel.Item_id, reel.Init_qty, reel.Curr_qty, reel.Expired, Loc);
                Reels.Add(reel);
            }
            catch(Exception ex)
            {
                strError = ex.Message;
            }
        }
    }
}