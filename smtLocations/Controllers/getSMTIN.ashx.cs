using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smtLocations.Controllers
{
    /// <summary>
    /// Descripción breve de getSMTIN
    /// </summary>
    public class getSMTIN : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hola a todos");
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