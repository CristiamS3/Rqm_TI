using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Windows.Interop;
using iText.Layout.Element;
using RQMTI.Models;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace RQMTI
{
    public partial class Default : System.Web.UI.Page
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MiConexionSQLServer"].ConnectionString;
        private static bool eventoEjecutado = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string esUsuarioAutenticado = "";
                string esUsuarioAutenticado2 = "";

                if (usuarioTxt.Value.Length > 3 && contraseñaTxt.Value.Length > 4)
                {
                    if (ValidaC())
                    {
                        using (SqlConnection conexion = new SqlConnection(connectionString))
                        {
                            conexion.Open();
                            using (SqlCommand comando = new SqlCommand("AUTENTICAU2", conexion))
                            {
                                comando.CommandType = CommandType.StoredProcedure;
                                comando.Parameters.Add("@p_NombreUsuario", SqlDbType.VarChar, 15).Value = usuarioTxt.Value.Trim();
                                comando.Parameters.Add("@p_Contraseña", SqlDbType.VarChar, 20).Value = contraseñaTxt.Value.Trim();
                                comando.Parameters.Add("@p_Respuesta", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                                comando.Parameters.Add("@p_Respuesta2", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                                comando.ExecuteNonQuery();

                                string resultado = comando.Parameters["@p_Respuesta2"].Value.ToString();
                                bool contieneLetra = resultado.Contains("U") || resultado.Contains("P") || resultado.Contains("A");

                                if (contieneLetra)
                                {
                                    esUsuarioAutenticado = Convert.ToString(comando.Parameters["@p_Respuesta"].Value.ToString());
                                    esUsuarioAutenticado2 = Convert.ToString(comando.Parameters["@p_Respuesta2"].Value.ToString());
                                }
                                else
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Usuario o Contraseña Incorrecta','warning')", true);
                                }
                            }
                            conexion.Close();
                        }

                        if (!string.IsNullOrEmpty(esUsuarioAutenticado) && !string.IsNullOrEmpty(esUsuarioAutenticado2))
                        {
                            // Autenticado correctamente
                            Session["NombUsu"] = usuarioTxt.Value;
                            Session["Tipo"] = esUsuarioAutenticado2;
                            Session.Timeout = 30;

                            // Redirigir sin abortar el hilo
                            Response.Redirect(esUsuarioAutenticado + ".aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            // No autenticado correctamente
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Nombre de usuario o contraseña incorrectos!','warning')", true);
                        }
                    }
                    else
                    {
                        CompruebaActC.Visible = true;
                        login.Visible = false;
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Llenar todos los campos','warning')", true);
                }
            }
            catch (Exception ex)
            {
                ILogE("B1D-", ex.Message);
                Console.WriteLine("Error: " + ex.Message);
            }
        }


        //protected void btnLogin_Click(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        string esUsuarioAutenticado = "";
        //        string esUsuarioAutenticado2 = "";
        //        //System.Threading.Thread.Sleep(3000);

        //        if (usuarioTxt.Value.Length > 4 && contraseñaTxt.Value.Length > 4)
        //        {

        //            if (ValidaC())
        //            {
        //                using (SqlConnection conexion = new SqlConnection(connectionString))
        //                {
        //                    conexion.Open();
        //                    using (SqlCommand comando = new SqlCommand("AUTENTICAU2", conexion))
        //                    {
        //                        comando.CommandType = CommandType.StoredProcedure;
        //                        comando.Parameters.Add("@p_NombreUsuario", SqlDbType.VarChar, 15).Value = usuarioTxt.Value.Trim();
        //                        comando.Parameters.Add("@p_Contraseña", SqlDbType.VarChar, 20).Value = contraseñaTxt.Value.Trim();
        //                        comando.Parameters.Add("@p_Respuesta", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
        //                        comando.Parameters.Add("@p_Respuesta2", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
        //                        comando.ExecuteNonQuery();

        //                        string resultado = comando.Parameters["@p_Respuesta2"].Value.ToString();
        //                        bool contieneLetra = resultado.Contains("U") || resultado.Contains("P") || resultado.Contains("A");

        //                        if (contieneLetra)
        //                        {
        //                            esUsuarioAutenticado = Convert.ToString(comando.Parameters["@p_Respuesta"].Value.ToString());
        //                            esUsuarioAutenticado2 = Convert.ToString(comando.Parameters["@p_Respuesta2"].Value.ToString());
        //                        }
        //                        else
        //                        {
        //                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Usuario o Contraseña Incorrecta','warning')", true);
        //                        }
        //                    }
        //                    conexion.Close();
        //                }

        //                if (!string.IsNullOrEmpty(esUsuarioAutenticado) && !string.IsNullOrEmpty(esUsuarioAutenticado2))
        //                {
        //                    Thread tiempoThread = new Thread(new ThreadStart(ManejarTiempo));
        //                    tiempoThread.Start();
        //                    // Autenticado correctamente
        //                    Session["NombUsu"] = usuarioTxt.Value;
        //                    Session["Tipo"] = esUsuarioAutenticado2;
        //                    Session.Timeout = 30;
        //                    Response.Redirect(esUsuarioAutenticado + ".aspx");
        //                    try
        //                    {
        //                        // Marcar que el evento se ha ejecutado
        //                        eventoEjecutado = true;
        //                        // Detener el hilo de tiempo
        //                        tiempoThread.Abort();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        ILogE("BD-1", ex.Message); Console.WriteLine("Error al detener el hilo de tiempo: " + ex.Message);
        //                    }
        //                }
        //                else
        //                {
        //                    // No autenticado correctamente
        //                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Nombre de usuario o contraseña incorrectos!','warning')", true);
        //                }
        //            }
        //            else
        //            {
        //                CompruebaActC.Visible = true;
        //                login.Visible = false;
        //            }
        //        }
        //        else
        //        {
        //            //Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", "Swal.fire({ icon: 'error', title: 'Error', text: 'Llenar todos los campos!' });", true);
        //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Llenar todos los campos','warning')", true);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ILogE("B1D-", ex.Message); Console.WriteLine("Error: " + ex.Message);
        //    }

        //}

        private string IsValidUser(string tip)
        {

            string respuesta = "";
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                try
                {
                    conexion.Open();
                    // Crea un comando SQL Server
                    using (SqlCommand comando = new SqlCommand("TipoUsu", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add("@p_NombreUsuario", SqlDbType.VarChar).Value = usuarioTxt.Value.Trim();
                        // comando.Parameters.Add("@p_Tipo", SqlDbType.Int).Value = tip;
                        comando.Parameters.Add("@p_Respuesta", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                        comando.ExecuteNonQuery();

                        if (comando.Parameters["@p_Respuesta"].Value == null)
                        { }
                        else
                        {
                            respuesta = Convert.ToString(comando.Parameters["@p_Respuesta"].Value.ToString());
                        }
                        Session["NombUsu"] = usuarioTxt.Value;
                        Session.Timeout = 30;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            
            return respuesta;
        }

        private void ManejarTiempo()
        {
            try
            {
                int tiempoLimite = 60;
                for (int i = 0; i < tiempoLimite && !eventoEjecutado; i++)
                {
                    Thread.Sleep(1000); // Dormir durante 1 segundo
                }
            }
            catch (Exception ex)
            { ILogE("MT-D", ex.Message);   }
        }

        protected void ILogE(string codexE, string msgE)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand("REGISTRALOGE", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add("@p_Cod", SqlDbType.VarChar, 5).Value = codexE;
                        comando.Parameters.Add("@p_Desc", SqlDbType.VarChar, 100).Value = msgE;
                        comando.Parameters.Add("@p_usu", SqlDbType.VarChar, 15).Value = usuarioTxt.Value.Trim();
                        comando.ExecuteNonQuery();
                    }
                    conexion.Close();
                }                
            }
            catch (Exception ex) { Console.WriteLine("Error: "+ex.Message+""); }
            finally { }
        }

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            login.Visible = false;
            recuperaContraseña.Visible = true;
        }

        protected void btnCambiarC_Click(object sender, EventArgs e)
        {

            try
            {
                if (correoB.Value.Length > 8)
                {
                    using (SqlConnection conexion = new SqlConnection(connectionString))
                    {
                        conexion.Open();
                        using (SqlCommand comando = new SqlCommand("RECUPERAC", conexion))
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.Parameters.Add("@p_correoB", SqlDbType.VarChar, 30).Value = correoB.Value.Trim();
                            comando.Parameters.Add("@p_Respuesta", SqlDbType.Int, 100).Direction = ParameterDirection.Output;
                            comando.Parameters.Add("@p_Respuesta2", SqlDbType.VarChar, 130).Direction = ParameterDirection.Output;
                            comando.Parameters.Add("@p_Respuesta3", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                            comando.ExecuteNonQuery();
                            int resultado = Convert.ToInt32(comando.Parameters["@p_Respuesta"].Value);
                            string resultado2 = Convert.ToString(comando.Parameters["@p_Respuesta2"].Value);
                            string resultado3 = Convert.ToString(comando.Parameters["@p_Respuesta3"].Value);
                            if (resultado>0)
                            {
                                Task.Run(() => EnviarCorreo(resultado2, resultado3));

                                //EnviarCorreo(resultado2, resultado3);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Éxito!','Cambio de contraseña exitoso, revisar respuesta en correo!','success')", true);
                                login.Visible = true;
                                recuperaContraseña.Visible = false;
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Correo incorrecto','warning')", true);
                            }
                        }
                        conexion.Close();
                    }
                }
                else
                {
                     ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Llenar todos los campos','warning')", true);
                }

            }
            catch (Exception ex)
            {
                ILogE("BD1-", ex.Message); Console.WriteLine("Error: " + ex.Message);
            }


        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            login.Visible = true;
            recuperaContraseña.Visible = false;
        }

        public void EnviarCorreo(string nombre, string contraseñaN)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress("alertadesarrollo@banadesa.hn", "Desarrollo Aplicaciones TI");
                message.To.Add(new MailAddress(correoB.Value));
                message.Subject = "Creacion de Usuario Sistema Requerimientos TI";

                string body = "<body>" +
                    "<h1 style='color:White; background:green; text-align: center; font-family: cursive;'> Sistema de Requerimientos Informáticos! </h1>" +
                    "<h3 style='text-align: center; font-family: cursive;'> Estimado <b>" + nombre + "</b> </h3>" +
                    "<span> Se ha cambiado su Contraseña a: <b>" + contraseñaN + ".</b> </span>" +
                    "<span> Cualquier duda o consulta, no olvides ponerte en contacto con el departamento de Tecnología </span></br>" +
                    "</br></br><span> Saludos Cordiales </span>" +
                    "</body>";

                message.IsBodyHtml = true;
                message.Body = body;

                using (var client = new SmtpClient("correo.banadesa.hn", 587))
                {
                    client.Credentials = new NetworkCredential("alertadesarrollo@banadesa.hn", "MiQ-Tr9w!fr8S");
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
            catch (Exception ex)
            {
                ILogE("ECNU", ex.Message); Console.WriteLine("Error al enviar el correo electrónico: " + ex.Message);
            }
        }

        protected void btnActualizaC_Click(object sender, EventArgs e)
        {
            // Obtener los valores de los TextBoxes
            string contraseña1 = contra1.Value.Trim(); // Eliminar espacios al principio y al final
            string contraseña2 = contra2.Value.Trim(); // Eliminar espacios al principio y al final

            // Verificar si las contraseñas son iguales
            if (contraseña1 == contraseña2)
            {
                try
                {
                    using (SqlConnection conexion = new SqlConnection(connectionString))
                    {
                        conexion.Open();
                        using (SqlCommand comando = new SqlCommand("CAMBIOC", conexion))
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.Parameters.Add("@p_cont", SqlDbType.VarChar, 20).Value = contraseña2;
                            comando.Parameters.Add("@p_usu", SqlDbType.VarChar, 15).Value = usuarioTxt.Value.Trim();
                            comando.ExecuteNonQuery();
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Éxito!','Cambio de contraseña exitoso!','success')", true);
                        }
                        conexion.Close();
                        login.Visible = true;
                        recuperaContraseña.Visible = false;
                        CompruebaActC.Visible = false;
                    }
                }
                catch (Exception ex) {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Error al cambiar contraseña: " + ex.Message +"','warning')", true);
                }
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Advertencia!','Contraseñas no coinciden','warning')", true);
            }
        }

        protected bool ValidaC()
        {
            bool valido = true;
            try
            {
                    using (SqlConnection conexion = new SqlConnection(connectionString))
                    {
                        conexion.Open();
                        using (SqlCommand comando = new SqlCommand("VALIDAC", conexion))
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.Parameters.Add("@p_Contraseña", SqlDbType.VarChar, 20).Value = contraseñaTxt.Value.Trim();
                            comando.Parameters.Add("@p_Respuesta", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                            comando.ExecuteNonQuery();
                            string resultado = comando.Parameters["@p_Respuesta"].Value.ToString();
                            bool contieneLetra = resultado.Contains("OK") || resultado.Contains("SI");
                            if (contieneLetra)
                            {   valido = false;  }
                            else
                            {   valido = true;  }
                        }
                        conexion.Close();
                    }                
            }
            catch (Exception ex)
            {
                ILogE("V1-D", ex.Message); Console.WriteLine("Error: " + ex.Message);
            }
            return valido;
        }

        protected void btnCancelarA_Click(object sender, EventArgs e)
        {
            login.Visible = true;
            recuperaContraseña.Visible = false;
            CompruebaActC.Visible= false;
        }


        #region Code anterior

        //// Crear un hilo adicional para manejar el tiempo
        //Thread tiempoThread = new Thread(new ThreadStart(ManejarTiempo));
        //tiempoThread.Start();

        //string esUsuarioAutenticado = ""; string esUsuarioAutenticado2 = "";
        //try
        //{
        //    if (usuarioTxt.Value.Length > 4 && contraseñaTxt.Value.Length > 4)
        //    {
        //        //parte 2
        //        using (OracleConnection conexion = new OracleConnection(connectionString))
        //        {
        //            conexion.Open();
        //            using (OracleCommand comando = new OracleCommand("APPSBANADESA.AUTENTICAU2", conexion))
        //            {
        //                comando.CommandType = CommandType.StoredProcedure;
        //                comando.Parameters.Add("p_NombreUsuario", OracleDbType.Varchar2, 15).Value = usuarioTxt.Value.ToString().Trim();
        //                comando.Parameters.Add("p_Contraseña", OracleDbType.Varchar2, 20).Value = contraseñaTxt.Value.ToString().Trim();
        //                comando.Parameters.Add("p_Respuesta", OracleDbType.Varchar2, 100).Direction = ParameterDirection.Output;
        //                comando.Parameters.Add("p_Respuesta2", OracleDbType.Varchar2, 100).Direction = ParameterDirection.Output;
        //                comando.ExecuteNonQuery();
        //                if (comando.Parameters["p_Respuesta"].Value.ToString().Length == 0)
        //                { Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", "Swal.fire({ icon: 'error', title: 'Error', text: 'Error en respuesta!' });", true); return; }
        //                else { esUsuarioAutenticado = Convert.ToString(comando.Parameters["p_Respuesta"].Value.ToString()); esUsuarioAutenticado2 = Convert.ToString(comando.Parameters["p_Respuesta2"].Value.ToString()); }
        //            }
        //            conexion.Close();
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine("Error: " + ex.Message);
        //}

        //if (esUsuarioAutenticado.Length > 0 && esUsuarioAutenticado2.Length > 0)
        //{
        //    // Autenticado correctamente
        //    Session["NombUsu"] = usuarioTxt.Value;
        //    Session["Tipo"] = esUsuarioAutenticado2;
        //    Session.Timeout = 30;
        //    Response.Redirect(esUsuarioAutenticado + ".aspx");
        //}
        //else
        //{
        //    // No autenticado correctamente
        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "SweetAlert", "Swal.fire({ icon: 'error', title: 'Error', text: 'Nombre de usuario o contraseña incorrectos!' });", true);
        //}
        //try
        //{
        //    // Marcar que el evento se ha ejecutado
        //    eventoEjecutado = true;
        //    // Detener el hilo de tiempo
        //    tiempoThread.Abort();
        //}
        //catch (Exception ex) { }
        #endregion

    }
}