using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace LAP_API.Modules
{
    public class Database
    {
        private SqlConnection conn;
        public bool status;

        public Database()
        {
            status = Connection();
        }

        private bool Connection()
        {
            string host = "(localdb)\\ProjectsV13";
            string user = "root";
            string password = "1234";
            string db = "Lap";

            string connStr = string.Format("server={0};uid={1};password={2};database={3};", host, user, password, db);
            SqlConnection conn = new SqlConnection(connStr);

            try
            {
                conn.Open();
                this.conn = conn;
                return true;
            }
            catch
            {
                conn.Close();
                this.conn = null;
                return false;
            }
        }

        public bool ConnectionClose()
        {
            try
            {
                conn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool NonQuery(string sql, Hashtable ht)
        {
            if (status)
            {
                try
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandText = sql;
                    comm.Connection = conn;
                    comm.CommandType = CommandType.StoredProcedure;
                    foreach (DictionaryEntry data in ht)
                    {
                        comm.Parameters.AddWithValue(data.Key.ToString(), data.Value);
                    }
                    comm.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
                return false;
        }
        public SqlDataReader Reader2(string sql, Hashtable ht)
        {
            if (status)
            {
                try
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandText = sql;
                    comm.Connection = conn;
                    comm.CommandType = CommandType.StoredProcedure;
                    foreach (DictionaryEntry data in ht)
                    {
                        comm.Parameters.AddWithValue(data.Key.ToString(), data.Value);
                    }
                    return comm.ExecuteReader();
                }
                catch
                {
                    return null;
                }
            }
            else
                return null;
        }
        public SqlDataReader Reader(string sql)
        {
            if (status)
            {
                try
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandText = sql;
                    comm.Connection = conn;
                    comm.CommandType = CommandType.StoredProcedure;
                    return comm.ExecuteReader();
                }
                catch
                {
                    return null;
                }
            }
            else
                return null;
        }

        public bool ReaderClose(SqlDataReader reader)
        {
            try
            {
                reader.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
