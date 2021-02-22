using System;
using SqlSugar;

namespace FytSugar.Builder
{
    public class SugarInstance
    {

        public SqlSugarClient GetInstance(BuilderConnection model)
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "server="+model.Ip+",port="+model.Port+";database="+ model.DbName +";uid="+ model.Name +";pwd="+ model.PassWord +";charset='utf8';SslMode=None",
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });

            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql);
            };
            return db;
        }
    }
}
