using Business.Logic.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Logic.Services
{
	public interface IRedeemService
	{
		Task<redeemCode> RedeemVoucher(redeemCode redeem, string storeProc);
		Task GetVoucher(string VoucherCode, params string[] parameters);
		Task <IEnumerable<redeemCode>> GetAllVouchers(string merchant);
	}
}
