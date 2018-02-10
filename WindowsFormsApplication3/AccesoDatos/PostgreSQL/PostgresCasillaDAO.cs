﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace FlyMail
{
    class PostgresCasillaDAO : ICasillaDAO
    {
        /// <summary>
        /// Conexión con la Base de Datos
        /// </summary>
        private NpgsqlConnection _conexion;

        /// <summary>
        /// Nueva CasillaDAO
        /// </summary>
        /// <param name="pConexion"></param>
        public PostgresCasillaDAO(NpgsqlConnection pConexion)
        {
            _conexion = pConexion;
        }

        /// <summary>
        /// Determina si existe o no el nombre de la Casilla. Se utiliza para comprobar que no existan dos nombres iguales
        /// </summary>
        /// <param name="pNombre">Nombre a buscar</param>
        /// <param name="idCuenta">ID de la Cuenta</param>
        /// <returns></returns>
        public bool nombreExistente(string pNombre, int idCuenta)
        {
            NpgsqlCommand comando = this._conexion.CreateCommand();
            comando.CommandText = "SELECT * FROM \"CasillaEmail\" WHERE nombre = '" + pNombre + "' and usuario = '" + idCuenta + "'";
            NpgsqlDataReader reader = comando.ExecuteReader();
            if (reader.Read())
                return true;
            else
                return false;
        }

        public List<string> listaNombres(int idCuenta)
        {
            NpgsqlCommand comando = this._conexion.CreateCommand();
            comando.CommandText = "SELECT nombre FROM \"CasillaEmail\" WHERE usuario = '" + idCuenta + "'";
            NpgsqlDataReader reader = comando.ExecuteReader();
            List<string> _listaNombre = new List<string>();
                //_listaNombre.Add(reader[0].ToString());
            while (reader.Read())
            {
                _listaNombre.Add(reader[0].ToString());
            }   
            return _listaNombre;
        }

        /// <summary>
        /// Busca y devuelve un Dirección de Correo a través del nombre
        /// </summary>
        /// <param name="pNombre"></param>
        /// <returns></returns>
        public string buscarDireccion(string pNombre)
        {
            NpgsqlCommand comando = this._conexion.CreateCommand();
            comando.CommandText = "SELECT \"direccionEmail\" FROM \"CasillaEmail\" WHERE nombre = '" + pNombre + "'";
            NpgsqlDataReader reader = comando.ExecuteReader();
            if (reader.Read())
            {
                return reader[0].ToString();
            }
            else
            {
                throw new DAOException("Nombre de Casilla no encontrado");
            }
        }

        /// <summary>
        /// Agrega una Nueva Casilla de Correo
        /// </summary>
        /// <param name="pCasilla">Casilla de Correo</param>
        /// <param name="pServicio">Servicio de Correo (GMAIL, YAHOO)</param>
        /// <param name="pUsuario">Número de Usuario</param>
        public void agregar(CasillaCorreo pCasilla, int pServicio, int pUsuario)
        {/*
            try
            {*/
                using (NpgsqlTransaction transaccion = this._conexion.BeginTransaction())
                {

                    NpgsqlCommand comando = this._conexion.CreateCommand();

                    comando.Transaction = transaccion;
                    comando.CommandText = "INSERT INTO \"CasillaEmail\"(nombre,\"contrasenaEmail\",servicio,usuario,\"direccionEmail\") VALUES(@nombre,@contrasenaEmail,@servicio,@usuario,@direccionEmail)";
                    comando.Parameters.AddWithValue("@nombre", pCasilla.Nombre);
                    comando.Parameters.AddWithValue("@contrasenaEmail", pCasilla.Contraseña);
                    comando.Parameters.AddWithValue("@servicio", pServicio);
                    comando.Parameters.AddWithValue("@usuario", pUsuario);
                    comando.Parameters.AddWithValue("@direccionEmail", pCasilla.Direccion);

                    comando.ExecuteNonQuery();

                    transaccion.Commit();
                }/*
            }
            catch (Exception e)
            {
                throw new DAOException("No se pudo agregar la Casilla", e);
            }*/
            
        }

        /// <summary>
        /// Modifica la dirección y la contraseña de la Casilla de Correo
        /// </summary>
        /// <param name="pCasilla"></param>
        public void modificar(CasillaCorreo pCasilla)
        {
            try
            {
                NpgsqlCommand comando = this._conexion.CreateCommand();
                comando.CommandText = "UPDATE \"CasillaEmail\" SET \"direccionEmail\"  = @direccion, \"contrasenaEmail\" = @contrasena WHERE nombre = '" + pCasilla.Nombre + "'";

                comando.Parameters.AddWithValue("@direccion", pCasilla.Direccion);
                comando.Parameters.AddWithValue("@contrasena", pCasilla.Contraseña);
                comando.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new DAOException("No se pudo modificar la Casilla", e);
            }
        }

        /// <summary>
        /// Modifica la dirección de la Casilla de Correo
        /// </summary>
        /// <param name="pCasilla"></param>
        public void modificarDireccion(CasillaCorreo pCasilla)
        {
            try
            {
                NpgsqlCommand comando = this._conexion.CreateCommand();
                comando.CommandText = "UPDATE \"CasillaEmail\" SET \"direccionEmail\"  = @direccion WHERE nombre = '" + pCasilla.Nombre + "'";

                comando.Parameters.AddWithValue("@direccion", pCasilla.Direccion);
                comando.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new DAOException("No se pudo modificar la Casilla", e);
            }
        }
    }
}
