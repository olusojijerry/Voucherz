using Business.Logic.Domain;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Business.Logic.Services
{
	public class redeemService: BaseService, IRedeemService
	{
		public redeemService(IConfiguration configuration): base(configuration)
		{

		}

		public async Task <IEnumerable<redeemCode>> GetAllVouchers(string merchant)
		{
			using (var conn = this.Connection)
			{
				string spName = "spGetRedeemedVoucher";
				conn.Open();

				return await conn.QueryAsync<redeemCode>(spName,
					new { merchant },
					commandType: CommandType.StoredProcedure);

				
			};
		}

		public async Task GetVoucher(string storeProcedure, params string[] parameters)
		{
			using (var conn = this.Connection)
			{
				//string spName = stor;// "spGetRedeemedVoucher";
				conn.Open();
				
				await conn.ExecuteAsync(storeProcedure,
					new { parameters },
					commandType: CommandType.StoredProcedure);
				
			};
		}

		public async Task<redeemCode> RedeemVoucher(redeemCode redeem, string storeProc)
		{
			using (var conn = this.Connection)
			{
				//string spName = stor;// "spGetRedeemedVoucher";
				conn.Open();

				var disabled = await conn.QueryAsync<redeemCode>(storeProc,
					new { redeem.code, redeem.CodeStatus },
					commandType: CommandType.StoredProcedure).ConfigureAwait(false);

				return redeem;
			};
		}


		//public async Task RedeemVoucher(redeemCode redeem, string storeProc)
		//{
		//	using (var conn = this.Connection)
		//	{
		//		conn.Open();

		//		await conn.ExecuteAsync(storeProc,
		//			new { redeem.VoucherCode, redeem.Email, redeem.CodeStatus, redeem.TimesRedeemed, redeem.RedeemedStatus  },
		//			commandType: CommandType.StoredProcedure).ConfigureAwait(false);
		//	}
		//}


	}
}
