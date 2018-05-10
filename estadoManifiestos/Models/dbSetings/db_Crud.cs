//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Data;
//using Oracle.DataAccess.Client;
//using estadoManifiestos.Models;

//namespace estadoManifiestos.Models.dbSetings
//{
//    public class db_Crud
//    {
//        //***********************************************************
//        private OracleConnection ora_Connection;
//        private OracleCommand comando;
//        private string cadena;

//        //***********************************************************
//        public OracleConnection Ora_Connection
//        {
//            get { return ora_Connection; }
//            set { ora_Connection = value; }
//        }
//        public string Cadena
//        {
//            get { return cadena; }
//            set { cadena = value; }
//        }
//        //Constructor de la clase
//        public db_Crud()
//        {
//            //cargar la clase qe contiene los parametros de conexion
//            db_Param _dbParam = new db_Param();
//            string _servidor, _dbname, _usuario, _contrasena;
//            _servidor = _dbParam.servidor;
//            _dbname = _dbParam.dbName;
//            _usuario = _dbParam.usuario;
//            _contrasena = _dbParam.constraseña;
//            Cadena = @"User Id=" + _usuario + ";Password="
//               + _contrasena + ";Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST="
//                + _servidor + ")(PORT=1521))(CONNECT_DATA=(SID=" + _dbname + ")));";
//        }

//        public bool Conectar()
//        {
//            bool ok = false;
//            try
//            {
//                if (Ora_Connection == null)
//                {
//                    // Fijamos la cadena de conexión de la base de datos.
//                    Ora_Connection = new OracleConnection();
//                    Ora_Connection.ConnectionString = Cadena;
//                    Ora_Connection.Open();
//                    ok = true;
//                }
//            }

//            catch (Exception)
//            {
//                Desconectar();
//                ok = false;
//            }
//            return ok;

//        }
//        public bool Desconectar()
//        {
//            try
//            {
//                // Cerramos la conexion
//                if (ora_Connection != null)
//                {
//                    if (ora_Connection.State != ConnectionState.Closed)
//                    {
//                        ora_Connection.Close();
//                    }
//                }
//                // Liberamos su memoria.
//                ora_Connection.Dispose();
//                return false;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }

//        public DataTable CargarListaRecientes()
//        {
//            DataTable listaRecientes = new DataTable();
//            bool preguntar = Conectar();
//            if (preguntar)
//            {

//                //OracleDataReader respuesta;

//                string consulta = @"SELECT ORCA_NUMERO_NB,TO_CHAR(ORCA_FECCREA_DT, 'dd/mm/yyyy hh24:mi:ss') fecha FROM (SELECT ORCA_NUMERO_NB,ORCA_FECCREA_DT FROM ordenes_cargue WHERE TRUNC(ORCA_FECCREA_DT) >= TRUNC(sysdate-3)ORDER BY ORCA_FECCREA_DT DESC)WHERE rownum <= 10 ORDER BY ORCA_FECCREA_DT ASC";

//                comando = new OracleCommand(consulta, Ora_Connection);
//                //OracleTransaction transaccion;
//                //transaccion = Ora_Connection.BeginTransaction(IsolationLevel.ReadCommitted);//bloquear los dato tomados para no generar errores
//                //comando.Transaction = transaccion;
//                try
//                {
//                    //respuesta = comando.ExecuteReader();
//                    //transaccion.Commit();
//                    listaRecientes.Load(comando.ExecuteReader());
//                    Desconectar();
                    


//                }
//                catch (Exception)
//                {
//                    //transaccion.Rollback();
//                    Desconectar();
//                    return listaRecientes;
//                    throw;
//                }
//            }
//            return listaRecientes;
//        }

//    }
//}