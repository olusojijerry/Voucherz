using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Business.Logic.Services
{
	public class BaseService
	{
		private readonly IConfiguration _config;
		public BaseService(IConfiguration configuration)
		{
			this._config = configuration;
		}

		public IDbConnection Connection
		{
			get
			{
				return new SqlConnection(_config.GetConnectionString("MainConString"));
			}
		}
	}
}
