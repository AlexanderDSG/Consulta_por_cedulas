using Consulta_por_cedulas.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Consulta_por_cedulas.Controllers
{
    public class DatosController : Controller
    {
        // GET: Datos
        private static string conexion = ConfigurationManager.ConnectionStrings["ConexionSQL"].ToString();

        private static List<DatosClientes> olista = new List<DatosClientes>();
        public ActionResult Index()
        {

            return View(olista);
        }

        //validar direcciones IP
        private bool ValidIPAddress(string IP)
        {
            if (IPAddress.TryParse(IP, out var address) == false)
                return false;

            if (address.AddressFamily == AddressFamily.InterNetworkV6)
            {
                if (IP.IndexOf("::") > -1)
                    return true;
                return false;
            }
            else
            {
                if (Regex.IsMatch(IP, @"(^0\d|\.0\d)"))
                    return false;
                else if (IP.Count(c => c == '.') != 3)
                    return false;
                else
                    return true;
            }
        }
        private bool ValidaIP()
        {
            // Obtiene la dirección IP del cliente que realizó la solicitud
            string ipAddress = Request.UserHostAddress;

            // Verifica si la dirección IP del cliente es válida utilizando ValidIPAddress
            if (!ValidIPAddress(ipAddress))
            {
                return false;
            }
            else
            {
                return true;
            }

            
        }

        [HttpPost]
        public ActionResult Consulta(string cedula)
        {
            // verifica si la dirección IP del cliente es valida
            if (!ValidaIP())
            {
                TempData["Mensaje"] = "Error de Consultas";
                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(cedula) && cedula.Length <= 10)
            {
                string query = "SELECT ee_nombre, ee_apellido,ee_empresa, ee_estado FROM ext.tbl_EmpresasExternas WHERE ee_cedula = @Cedula";

                // Lista para almacenar los resultados de la consulta
                List<DatosClientes> contactosFiltrados = new List<DatosClientes>();

                try
                {
                    using (SqlConnection oconexion = new SqlConnection(conexion))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, oconexion))
                        {
                            cmd.Parameters.AddWithValue("@Cedula", cedula);
                            oconexion.Open();
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    // Si se encuentra la persona, obtener su estado
                                    string estado = dr["ee_estado"].ToString();
                                    if (estado == "A")
                                    {
                                        DatosClientes nuevoContacto = new DatosClientes();
                                        nuevoContacto.ee_nombre = dr["ee_nombre"].ToString();
                                        nuevoContacto.ee_apellido = dr["ee_apellido"].ToString();
                                        nuevoContacto.ee_empresa = dr["ee_empresa"].ToString();
                                        nuevoContacto.ee_estado = estado;
                                        contactosFiltrados.Add(nuevoContacto);
                                        return View("Index", contactosFiltrados);
                                    }
                                    else
                                    {
                                        TempData["Mensaje"] = "La cédula " + cedula + " está inactiva.";


                                        return RedirectToAction("Index");
                                    }
                                }
                            }
                        }
                    }

                    //mostrar un mensaje de error
                    TempData["Mensaje"] = "No se encontraron datos correspondientes a la cédula ingresada.";

                    //mostrar los resultados
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    TempData["Mensaje"] = "Ocurrió un error al procesar la solicitud." + ex;
                }
            }

            // mostrar nada si no se ingresó ninguna cédula válida
            return RedirectToAction("Index");
        }



    }

}
