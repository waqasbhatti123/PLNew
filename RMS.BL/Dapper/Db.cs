using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL.Dapper
{
    public class Db 
    {
        public Db()
        {
        }

        public IEnumerable<T> Get<T, P>(string query, P parameters, CommandType commandType)
        {
            using (IDbConnection _connection = new SqlConnection(Setting.ConnectionString))
            {
                IEnumerable<T> result = null;
                result = _connection.Query<T>(query, parameters, commandType: commandType, commandTimeout: Setting.CommandTimout);
                return result;
            }
        }

        public T GetById<T, P>(string query, P parameters, CommandType commandType)
        {
            using (IDbConnection _connection = new SqlConnection(Setting.ConnectionString))
            {
                IEnumerable<T> result = null;

                result = _connection.Query<T>(query, parameters, commandType: commandType, commandTimeout: Setting.CommandTimout);

                if (result != null && result.Count() > 0)
                {
                    return result.FirstOrDefault();
                }
                else
                {
                    return default(T);
                }
            }
        }

        public R Submit<T, R>(string query, T parameters, R response, CommandType commandType)
        {
            using (IDbConnection _connection = new SqlConnection(Setting.ConnectionString))
            {
                IEnumerable<R> result = null;
                result = _connection.Query<R>(query, parameters, commandType: commandType, commandTimeout: Setting.CommandTimout);

                if (result != null && result.Count() > 0)
                {
                    return result.FirstOrDefault();
                }
                else
                {
                    return default(R);
                }
            }
        }

        public int Submit<T>(string query, T parameters, CommandType commandType)
        {
            using (IDbConnection _connection = new SqlConnection(Setting.ConnectionString))
            {
                int result = 0;
                result = _connection.Execute(query, parameters, commandType: commandType, commandTimeout: Setting.CommandTimout);
                return result;
            }
        }
    }
}
