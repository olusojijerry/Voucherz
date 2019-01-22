using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Logic.Domain;
using Business.Logic.Services;
using Nancy;
using Nancy.ModelBinding;

namespace voucherz.redeem
{
	public class redeemVoucherModule : NancyModule
	{
		public redeemVoucherModule(IRedeemService redeemService) : base("/voucherz")
		{
			Get("/{voucher}", parameters => 
			{
				var code = (string)parameters.voucher;
				return redeemService.GetVoucher(code);
			});
			Get("/{merchant}", parameters =>
			{
				var email = (string)parameters.userid;
				return redeemService.GetAllVouchers(email);
			});
			Post("/{voucher}/redeem", async (parameters, _) => {
				//here you can redeem a voucher
				var redeemDetails = this.Bind<redeemCode>();
				//var userId = (int)parameters.userid;
				var redeemedVoucher = redeemService.RedeemVoucher(redeemDetails, "");
				//var shoppingCartItems = await productcatalog.GetShoppingCartItems(productcatalogIds).ConfigureAwait(false);
				//shoppingCart.AddItems(shoppingCartItems, eventStore);
				//shoppingCartStore.Save(shoppingCart);
				return redeemedVoucher;
			});
			Post("/{voucher}/rollback", async (parameters, _) => {
				//here you can rollback  a voucher
				var redeemDetails = this.Bind<redeemCode>();
				//var userId = (int)parameters.userid;
				var redeemedVoucher = redeemService.RedeemVoucher(redeemDetails, "");
				//var shoppingCartItems = await productcatalog.GetShoppingCartItems(productcatalogIds).ConfigureAwait(false);
				//shoppingCart.AddItems(shoppingCartItems, eventStore);
				//shoppingCartStore.Save(shoppingCart);
				return redeemedVoucher;
			});
		}
	}
}
