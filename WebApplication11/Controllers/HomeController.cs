using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication11.Models;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Globalization;


namespace WebApplication11.Controllers
{
    public static class db
    {
        private static location Location { set; get; }
        public static void set(location _Location)
        {
            Location = _Location;
        }
        public static location get()
        {
            return Location ;
        }

    }
    public class HomeController : Controller
    {

        public string getip()
        {
            try
            {   
                string path = Server.MapPath("~/App_Data/ip.txt");
                location loc = Index(Request.GetOwinContext().Request.RemoteIpAddress);
                PersianCalendar pc = new PersianCalendar();
                DateTime dn =  DateTime.Now;
                DateTime Time =new DateTime(pc.GetYear(dn),pc.GetMonth(dn),pc.GetDayOfMonth(dn),dn.Hour,dn.Minute,dn.Second);
                string message ="ip:["+Request.GetOwinContext().Request.RemoteIpAddress+"]";
                message += Environment.NewLine;
                message += "Browser:" + Request.Browser.Browser;
                message += Environment.NewLine;
                message += "colors:" + Request.Browser.IsColor;
                message += Environment.NewLine;
                message += "is mobile:" + Request.Browser.IsMobileDevice;
                message += Environment.NewLine;
                if (Request.Browser.IsMobileDevice)
                {
                    message += "DeviceModel:" + Request.Browser.MobileDeviceModel;
                    message += Environment.NewLine;
                }
                message += "cookie count:" + Request.Cookies.Count;
                message += Environment.NewLine;
                message += "UserLanguages:";
                int cu =Request.UserLanguages.Count();
                string sy = Request.UserLanguages[cu - 1];
                foreach (var item in Request.UserLanguages)
                {
                    if(sy==item) message+=item;
                    else message+=" "+item+" & ";
                }
                message += Environment.NewLine;
                message += "UserHostAddress:" + Request.UserHostAddress;
                message += Environment.NewLine;
                message += "UserHostName:" + Request.UserHostName;
                message += Environment.NewLine;
                message += "contry:" + loc.country;
                message += Environment.NewLine;
                message += "city:"+loc.city;
                message += Environment.NewLine;
                message += "time:"+"  "+string.Format("{0}/{1}/{2}",Time.Year,Time.Month,Time.Day)+"  "+string.Format("{0}:{1}:{2}", Time.Hour, Time.Minute, Time.Second);;
                message += Environment.NewLine;
                //message += "time:" 
                //message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(message);
                    writer.Close();
                }
                return "succes";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            

        }
        public string logs()
        {
            string respons = "just local";
            if (Request.IsLocal)
            {
                string path = Server.MapPath("~/App_Data/ip.txt");
                using (StreamReader writer = new StreamReader(path, true))
                {
                    respons=writer.ReadToEnd();
                }
            }
            return respons;
        }
        public location Index(string ip)
        {
            location loc=null;
            using (var client = new HttpClient())
            {
                string apiUrl = "http://ip-api.com/json/" + ip;
                client.BaseAddress = new Uri(apiUrl);
                //HTTP GET
                var responseTask = client.GetAsync("");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<location>();
                    readTask.Wait();
                    loc = readTask.Result;
                }
                else //web api sent error response 
                {
                }
            }
            return loc;
        }
      
    }
}