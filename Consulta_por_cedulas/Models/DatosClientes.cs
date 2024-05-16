using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Consulta_por_cedulas.Models
{
    public class DatosClientes
    {
        public string ee_cedula { get; set; }
        public string ee_nombre { get; set; }
        public string ee_apellido { get; set; }
        public string ee_empresa { get; set; }
        public string ee_estado { get; set; }
        public string ip_valida { get; set; }


    }
}