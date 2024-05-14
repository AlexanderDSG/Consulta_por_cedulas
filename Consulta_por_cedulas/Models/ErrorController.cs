using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Consulta_por_cedulas.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult NotFound()
        {
            
            TempData["Mensaje"] = "No tiene autorización para acceder a esta página.";
            return View();
        }

       
    }
}
