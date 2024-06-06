using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.EnterpriseServices;
using Consulta_por_cedulas.Models;

namespace Consulta_por_cedulas.Controllers
{
    public class ErrorController : Controller
    {

        // GET: Error
        public ActionResult NotFound()
        {

            string ipAddress = GetUserIpAddress();

            TempData["Mensaje"] = ipAddress;

            return View();
        }

        public string GetUserIpAddress(bool Lan = false)
        {
            string userIPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                                   Request.ServerVariables["REMOTE_ADDR"] ??
                                   Request.UserHostAddress ?? "";

            if (string.IsNullOrEmpty(userIPAddress) || userIPAddress.Trim() == "::1")
            {
                Lan = true;
                userIPAddress = string.Empty;
            }

            if (Lan && string.IsNullOrEmpty(userIPAddress))
            {
                string stringHostName = Dns.GetHostName();
                IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);

                userIPAddress = ipHostEntries.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToString() ??
                                ipHostEntries.AddressList.LastOrDefault()?.ToString() ??
                                "127.0.0.1";
            }

            return userIPAddress;
        }
    }
}
