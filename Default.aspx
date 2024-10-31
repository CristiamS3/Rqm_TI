<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RQMTI.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Requerimientos TI</title>
    <link rel="shortcut icon" href="../img/logo.png" />

    <!-- Referencia al archivo CSS de Bootstrap -->
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Styles/sweetalert.css" rel="stylesheet" />
    <script src="Scripts/sweetalert.min.js"></script>

    <style>
        #cuerpo{
            /*background-image: linear-gradient(
                to bottom,
                rgba(255, 255, 0, 0.5),
                rgba(0, 0, 255, 0.5)
              ), url("../img/bnd.png");*/
                background-image: linear-gradient(
                to bottom,
                rgba(0, 255, 0, 0.5),
                rgba(0, 0, 255, 0.5)
              ),url('../img/bnd.png');
                background-size: cover; /* La imagen cubrirá toda la pantalla */
                background-position: center; /* La imagen se centrará */
                background-attachment: fixed; /* La imagen se mantendrá fija */
           
        }
        #imgLogo{
            justify-content:center;
            
        }        
    </style>
<script>
    // Función para establecer la altura de la capa de transparencia
    function setTransparencyHeight() {
        // Obtener la altura total del documento
        var documentHeight = Math.max(
            document.body.scrollHeight,
            document.body.offsetHeight,
            document.documentElement.clientHeight,
            document.documentElement.scrollHeight,
            document.documentElement.offsetHeight
        );

        // Establecer la altura de la capa de transparencia
        var transparency = document.querySelector('.transparencia');
        if (transparency) {
            transparency.style.height = documentHeight + 'px';
        }
    }

    // Llamar a la función cuando se carga la página y cuando cambia el tamaño de la ventana
    window.onload = setTransparencyHeight;
    window.onresize = setTransparencyHeight;
</script>
</head>
<body id="cuerpo">

<section class="vh-100">
  <div class="container py-5 h-100">
    <div class="row d-flex justify-content-center align-items-center h-100">
      <div class="col col-xl-10">
        <div class="card" style="border-radius: 1rem;">
          <div class="row g-0">
            <div class="col-md-6 col-lg-5 d-flex justify-content-center align-items-center">
              <img src="../img/logo.png" alt="login form" id="imgLogo" class="img-fluid" style="border-radius: 1rem 0 0 1rem; max-width: 100%;" />
            </div>
            <div class="col-md-6 col-lg-7 d-flex align-items-center">
              <div class="card-body p-4 p-lg-5 text-black">

                <form id="form1" runat="server">

                  <div class="d-flex align-items-center mb-3 pb-1">
                    <i class="fas fa-cubes fa-2x me-3" style="color: #ff6219;"></i>
                    <span class="h1 fw-bold mb-0 text-success">Portal de Requerimientos</span>
                  </div>


                    <!-- Div contenedor Login -->
                    <div id="login" runat="server" visible="true" >

                        
                          <h5 class="fw-normal mb-3 pb-3" style="letter-spacing: 1px;">Pantalla de ingreso</h5>

                          <div class="form-outline mb-4">
                            <input type="text" id="usuarioTxt" class="form-control form-control-lg" onkeypress="return evitarEspacios(event)" min="3"  maxlength="15" runat="server" />
                            <label class="form-label" for="form2Example17">Usuario</label>
                          </div>

                          <div class="form-outline mb-4">
                            <input type="password" id="contraseñaTxt" class="form-control form-control-lg" onkeypress="return evitarEspacios(event)" min="3" maxlength="20" runat="server" />
                            <label class="form-label" for="form2Example27">Contraseña</label>
                          </div>

                          <div class="pt-1 mb-4">
                            <asp:Button class="btn btn-success btn-lg btn-block" id="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Ingresar" />
                          </div>

                          <div class="pt-1 mb-4">
                              <asp:LinkButton ID="btnMostrar" runat="server" OnClick="btnMostrar_Click" CssClass="badge rounded-pill m-1 text-dark">No recuerda contraseña?</asp:LinkButton>
                          </div>

                    </div>


                     <!-- Recuperar contraseña -->
                    <div id="recuperaContraseña" runat="server" visible="false">
                        <h5 class="fw-normal mb-3 pb-3" style="letter-spacing: 1px;">Recuperar contraseña</h5>
                        <div class="form-outline mb-4">
                          <input type="text" id="correoB" class="form-control form-control-lg" onkeypress="return evitarEspacios2(event)" maxlength="25" runat="server" />
                          <label class="form-label" for="form2Example27">Ingrese su correo:</label>
                        </div>

                        <div class="pt-1 mb-4">
                          <asp:Button class="btn btn-success btn-lg btn-block" id="btnCambiarC" runat="server" OnClick="btnCambiarC_Click" Text="Cambiar Contraseña" />
                          <asp:Button class="btn btn-danger btn-lg btn-block" id="btnCancelar" runat="server" OnClick="btnCancelar_Click" Text="Cancelar" />
                        </div>
                    </div>

                     <!-- Cambiar contraseña -->
                    <div id="CompruebaActC" runat="server" visible="false">
                        <h5 class="fw-normal mb-3 pb-3" style="letter-spacing: 1px;">Cambiar contraseña</h5>
                        <div class="form-outline mb-4">
                          <input type="text" id="contra1" class="form-control form-control-lg" onkeypress="return evitarEspacios2(event)" maxlength="20" runat="server" />
                          <label class="form-label" for="form2Example27">Ingrese su contraseña:</label>
                        </div>
                        <div class="form-outline mb-4">
                          <input type="text" id="contra2" class="form-control form-control-lg" onkeypress="return evitarEspacios2(event)" maxlength="20" runat="server" />
                          <label class="form-label" for="form2Example27">Confirme su contraseña:</label>
                        </div>

                        <div class="pt-1 mb-4">
                          <asp:Button class="btn btn-success btn-lg btn-block" id="btnActualizaC" runat="server" OnClick="btnActualizaC_Click" Text="Cambiar Contraseña" />
                          <asp:Button class="btn btn-danger btn-lg btn-block" id="btnCancelarA" runat="server" OnClick="btnCancelarA_Click" Text="Cancelar" />
                        </div>
                    </div>


                </form>

              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</section>

    <!-- Referencia al archivo JS de Bootstrap (asegúrate de agregar jQuery antes de este) -->
<script src="~/Scripts/jquery.min.js"></script>
<script src="~/Scripts/bootstrap.min.js"></script>
<script>
    function evitarEspacios(e) {
        if (e.which === 32) {
            e.preventDefault();
            return false;
        }
    }
    function evitarEspacios2(e) {
        if (e.which === 32) {
            e.preventDefault();
            return false;
        }
    }
    function DesactivarBoton() {
        document.getElementById("<%=btnLogin.ClientID %>").disabled = true;
    }
    //window.onbeforeunload = DesactivarBoton;
</script>
</body>
</html>
