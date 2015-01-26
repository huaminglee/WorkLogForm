using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using WorkLogForm.CommonClass;

namespace WorkLogForm.Connection
{
    class Connection
    {
        private static Configuration cfg = null;

        public static Configuration getConfiguration()
        {
            if (cfg == null)
            {
                cfg = new Configuration();
                string ip = IniReadAndWrite.IniReadValue("connect", "ip");
                string id = IniReadAndWrite.IniReadValue("connect", "id");
                string pwd = IniReadAndWrite.IniReadValue("connect", "pwd");
                string db = IniReadAndWrite.IniReadValue("connect", "db");
                cfg.SetProperty("connection.connection_string", "UID=" + Securit.DeDES(id) + ";PWD=" + Securit.DeDES(pwd) + ";Database=" + Securit.DeDES(db) + ";server=" + Securit.DeDES(ip));
                cfg.AddAssembly("ClassLibrary");
            }
            return cfg;
        }

        private static SqlConnection connection = null;

        public static SqlConnection getSqlConnection()
        {
            if (connection == null || connection.State == ConnectionState.Closed)
            {
                string ip = IniReadAndWrite.IniReadValue("connect", "ip");
                string id = IniReadAndWrite.IniReadValue("connect", "id");
                string pwd = IniReadAndWrite.IniReadValue("connect", "pwd");
                string db = IniReadAndWrite.IniReadValue("connect", "db");
                connection = new SqlConnection("UID=" + Securit.DeDES(id) + ";PWD=" + Securit.DeDES(pwd) + ";Database=" + Securit.DeDES(db) + ";server=" + Securit.DeDES(ip));
            }
            if (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                }
                catch
                {
                    throw;
                }
            }
            return connection;
        }

    }
}
