using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Services.ProductAPI.Dapper
{
	public interface IDapperWrapper
	{
		Task<int> ExecuteAsync(IDbConnection cnn, string sql, int commandType, object param=null);
		Task<IEnumerable<T>> QueryAsync<T>(IDbConnection cnn, string sql, int commandType, object param = null);
		IEnumerable<T> Query<T>(IDbConnection cnn, string sql, int commdType, object param);
	}
}
