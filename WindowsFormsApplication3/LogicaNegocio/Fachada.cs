﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlyMail
{
    public class Fachada
    {
        #region Definición
        private static Fachada instancia;

        private int _idCuentaLogeado;

        public static Fachada Instancia
        {
            get
            {
                if (instancia == null)
                    instancia = new Fachada();
                return instancia;
            }
        }

        public int IDCuentaLogeado
        {
            get { return _idCuentaLogeado; }
            set { _idCuentaLogeado = value; }
        }
        #endregion
        #region Métodos para Cuenta
        public void agregarCuenta(Cuenta pCuenta)
        {
            DAOFactory factory = DAOFactory.Instancia();

            try
            {
                factory.IniciarConexion();
                ICuentaDAO _cuentaDAO = factory.cuentaDAO;
                _cuentaDAO.agregar(pCuenta);
            }
            catch (DAOException)
            {
                factory.RollBack();
            }
            finally
            {
                factory.FinalizarConexion();
            }
        }

        public bool nombreExistenteCuenta(string pNombre)
        {
            DAOFactory factory = DAOFactory.Instancia();

            try
            {
                factory.IniciarConexion();
                ICuentaDAO _cuentaDAO = factory.cuentaDAO;
                return _cuentaDAO.nombreExistente(pNombre);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                factory.FinalizarConexion();
            }
        }

        public int verificarCuenta(Cuenta pCuenta)
        {
            DAOFactory factory = DAOFactory.Instancia();

            try
            {
                factory.IniciarConexion();
                ICuentaDAO _cuentaDAO = factory.cuentaDAO;
                //Console.WriteLine(_cuentaDAO.verificarCuenta(pNombre, pContraseña));
                return _cuentaDAO.verificarCuenta(pCuenta);
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                factory.FinalizarConexion();
            }
        }

        public bool verificarContraseña(String pContraseña)
        {
            DAOFactory factory = DAOFactory.Instancia();

            try
            {
                factory.IniciarConexion();
                ICuentaDAO _cuentaDAO = factory.cuentaDAO;
                return _cuentaDAO.verificarContraseña(this.IDCuentaLogeado,pContraseña);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                factory.FinalizarConexion();
            }
        }

        public string obtenerNombreCuenta()
        {
            DAOFactory factory = DAOFactory.Instancia();

            try
            {
                factory.IniciarConexion();
                ICuentaDAO _cuentaDAO = factory.cuentaDAO;
                return _cuentaDAO.obtenerNombreCuenta(this.IDCuentaLogeado);
            }
            catch (Exception)
            {
                return string.Empty;
            }
            finally
            {
                factory.FinalizarConexion();
            }
        }

        public string obtenerContrasenaCuenta()
        {
            DAOFactory factory = DAOFactory.Instancia();

            try
            {
                factory.IniciarConexion();
                ICuentaDAO _cuentaDAO = factory.cuentaDAO;
                return _cuentaDAO.obtenerContraseñaCuenta(this.IDCuentaLogeado);
            }
            catch (Exception)
            {
                return string.Empty;
            }
            finally
            {
                factory.FinalizarConexion();
            }
        }

        public void modificarNombreCuenta(Cuenta pCuenta)
        {
            DAOFactory factory = DAOFactory.Instancia();

            try
            {
                factory.IniciarConexion();
                ICuentaDAO _cuentaDAO = factory.cuentaDAO;
                _cuentaDAO.modificarNombre(this.IDCuentaLogeado, pCuenta);
            }
            catch (DAOException)
            {
                factory.RollBack();
            }
            finally
            {
                factory.FinalizarConexion();
            }
        }

        public void modificarContraseñaCuenta(string pContraseña)
        {
            DAOFactory factory = DAOFactory.Instancia();

            try
            {
                factory.IniciarConexion();
                ICuentaDAO _cuentaDAO = factory.cuentaDAO;
                _cuentaDAO.modificarContraseña(this.IDCuentaLogeado,pContraseña);
            }
            catch (DAOException)
            {
                factory.RollBack();
            }
            finally
            {
                factory.FinalizarConexion();
            }
        }
        #endregion
        #region Métodos para Casilla

        /// <summary>
        /// Determina si existe o no un nombre de Casilla para un usuario (Cuenta) logeado
        /// </summary>
        /// <param name="pNombre">Nombre de la Casilla de correo</param>
        /// <returns>true si el nombre existe, false en caso contrario. Devuelve false y un aviso en caso de error</returns>
        public bool NombreExistenteCasilla(string pNombre)
        {
            DAOFactory factory = DAOFactory.Instancia();

            try
            {
                factory.IniciarConexion();
                ICasillaDAO _casillaDAO = factory.casillaCorreoDAO;
                return _casillaDAO.NombreExistente(pNombre,this.IDCuentaLogeado);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            finally
            {
                factory.FinalizarConexion();
            }
        }

        /// <summary>
        /// Devuelve la dirección de correo electrónico a partir del nombre de la Casilla. Se recomienda primero determinar si existe el nombre.
        /// </summary>
        /// <param name="pNombreCasilla"></param>
        /// <returns>String conteniendo la dirección buscada. Cadena vacía en caso de error</returns>
        public string ObtenerDireccionCasilla(string pNombreCasilla)
        {
            DAOFactory factory = DAOFactory.Instancia();

            try
            {
                factory.IniciarConexion();
                ICasillaDAO _casillaDAO = factory.casillaCorreoDAO;
                return _casillaDAO.BuscarDireccion(pNombreCasilla);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return string.Empty;
            }
            finally
            {
                factory.FinalizarConexion();
            }
        }

        /// <summary>
        /// Inserta una nueva Casilla en la base de datos, para el usuario actual
        /// </summary>
        /// <param name="pCasilla"></param>
        /// <param name="pServicio"></param>
        public bool AgregarCasilla(CasillaCorreo pCasilla, int pServicio)
        {
            DAOFactory factory = DAOFactory.Instancia();

            try
            {
                factory.IniciarConexion();
                ICasillaDAO _casillaDAO = factory.casillaCorreoDAO;
                _casillaDAO.Agregar(pCasilla,pServicio, _idCuentaLogeado);
                return true;
            }
            catch (DAOException)
            {
                factory.RollBack();
                return false;
            }
            catch(Npgsql.PostgresException)
            {
                factory.RollBack();
                MessageBox.Show("La dirección de Correo Electrónico tiene un formato incorrecto");
                return false;
            }
            finally
            {
                factory.FinalizarConexion();
            }
        }

        /// <summary>
        /// Modifica una Casilla de correo. El campo Contraseña es opcional y puede dejarse vacio para no modificarla
        /// </summary>
        /// <param name="pCasilla"></param>
        public bool ModificarCasilla(CasillaCorreo pCasilla)
        {
            DAOFactory factory = DAOFactory.Instancia();

            try
            {
                factory.IniciarConexion();
                ICasillaDAO _casillaDAO = factory.casillaCorreoDAO;
                _casillaDAO.Modificar(pCasilla, this.IDCuentaLogeado);
                return true;
            }
            catch (DAOException)
            {
                factory.RollBack();
                return false;
            }
            catch (Exception e)
            {
                factory.RollBack();
                MessageBox.Show(e.Message);
                return false;
            }
            finally
            {
                factory.FinalizarConexion();
            }
        }

        /// <summary>
        /// Elimina una Casilla de Correo del usuario logeado, a través de su nombre
        /// </summary>
        /// <param name="pNombreCasilla"></param>
        public bool EliminarCasilla(string pNombreCasilla)
        {
            DAOFactory factory = DAOFactory.Instancia();

            try
            {
                factory.IniciarConexion();
                ICasillaDAO casillaDAO = factory.casillaCorreoDAO;
                casillaDAO.Eliminar(pNombreCasilla, this.IDCuentaLogeado);
                return true;
            }
            catch(DAOException e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            finally
            {
                factory.FinalizarConexion();
            }
        }

        /// <summary>
        /// Obtiene todos los nombres de Casillas del usuario logeado
        /// </summary>
        /// <returns></returns>
        public List<string> ObtenerNombreCasillas()
        {
            DAOFactory factory = DAOFactory.Instancia();
            List<string> _listaNombres = new List<string>();

            try
            {
                
                factory.IniciarConexion();
                ICasillaDAO _casillaDAO = factory.casillaCorreoDAO;
                _listaNombres = _casillaDAO.ListaNombres(this.IDCuentaLogeado);
                return _listaNombres;
            }
            catch (Exception)
            {
                _listaNombres.Clear();
                return _listaNombres;
            }
            finally
            {
                factory.FinalizarConexion();
            }
        }
        #endregion
        #region Métodos para Servicio
        public int obtenerIdServicio(string pProveedor)
        {
            DAOFactory factory = DAOFactory.Instancia();

            try
            {
                factory.IniciarConexion();
                IServicioDAO _servicioDAO = factory.servicioDAO;
                return _servicioDAO.obtenerId(pProveedor);
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                factory.FinalizarConexion();
            }
        }

        public List<string> obtenerProveedorServicio()
        {
            DAOFactory factory = DAOFactory.Instancia();
            List<string> _listaProveedor = new List<string>();

            try
            {

                factory.IniciarConexion();
                IServicioDAO _servicioDAO = factory.servicioDAO;
                _listaProveedor = _servicioDAO.listaServicio();
                return _listaProveedor;
            }
            catch (Exception)
            {
                return _listaProveedor;
            }
            finally
            {
                factory.FinalizarConexion();
            }
        }
        #endregion
    }
}
