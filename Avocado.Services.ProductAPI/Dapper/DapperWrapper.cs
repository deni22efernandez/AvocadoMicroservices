using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Services.ProductAPI.Dapper
{
	public class DapperWrapper : IDapperWrapper
	{
		public async Task<int> ExecuteAsync(IDbConnection cnn, string sql, int commandType, object param=null)
		{
			return commandType == 1 ?
				await cnn.ExecuteAsync(sql, param, commandType: CommandType.Text) :
				await cnn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
			
		}
		public async Task<IEnumerable<T>> QueryAsync<T>(IDbConnection cnn, string sql, int commdType, object param = null)
		{
			return commdType == 1 ?
				await cnn.QueryAsync<T>(sql, param, commandType: CommandType.Text) :
				await cnn.QueryAsync<T>(sql, param, commandType: CommandType.StoredProcedure);

		}
		public IEnumerable<T> Query<T>(IDbConnection cnn, string sql, int commdType, object param)
		{
			return commdType == 1 ?
				 cnn.Query<T>(sql, param, commandType: CommandType.Text) :
				 cnn.Query<T>(sql, param, commandType: CommandType.StoredProcedure);

		}



	}
}
