using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smtLocations.Class
{
    public class CReel
    {
        private String m_item_id;
        private int m_expired;
        private int m_init_qty;
        private int m_curr_qty;

        public CReel()
        {
            m_item_id = "";
            m_curr_qty = 0;
            m_init_qty = 0;
            m_expired = 0;
        }
        public string Item_id { get => m_item_id; set => m_item_id = value; }
        public int Expired { get => m_expired; set => m_expired = value; }
        public int Init_qty { get => m_init_qty; set => m_init_qty = value; }
        public int Curr_qty { get => m_curr_qty; set => m_curr_qty = value; }
    }
}