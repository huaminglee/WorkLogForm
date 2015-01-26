using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace WorkLogForm.Service
{
    class TabelMonitor
    {
        /// <summary>
        /// 监控数据库中表的数据变化
        /// </summary>
        /// <param name="monitorSql">监控表查询语句</param>
        /// <param name="method">添加代理事件的代理类并委托给一个方法,例：new OnChangeEventHandler(dependency_OnChange);
        /// private static void dependency_OnChange(object sender, SqlNotificationEventArgs e)</param>
        public static void UpdateGrid(String monitorSql,OnChangeEventHandler method)
        {
            //依赖是基于某一张表的,而且查询语句只能是简单查询语句,不能带top或*,同时必须指定所有者,即类似[dbo].[]
            using (SqlCommand command = new SqlCommand(monitorSql, Connection.Connection.getSqlConnection()))
            {
                command.CommandType = CommandType.Text;
                SqlDependency dependency = new SqlDependency(command);
                dependency.OnChange += method;
                SqlDataReader sdr = command.ExecuteReader();
                sdr.Close();
            }
        }
    }
}
