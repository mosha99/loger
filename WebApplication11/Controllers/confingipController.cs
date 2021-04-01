using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Web.Security;
using System.Data.OleDb;
using System.Data;
using System.Web.Mvc;

namespace WebApplication11.Controllers
{
    public class confingipController : Controller
    {

        // GET: api/confingip
        public void Get()
        {
            string path = Server.MapPath("~/App_Data/ip.txt");
            string message = Request.GetOwinContext().Request.RemoteIpAddress;
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }

        }

    }
}
