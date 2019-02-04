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
			Get("merchant/{merchant}", parameters =>
			{
				var email = (string)parameters.userid;
				return redeemService.GetAllVouchers(email);
			});
			Post("/{voucher}/redeem", async (parameters, _) => {
				//here you can redeem a voucher
				var redeemDetails = this.Bind<Redemption>();
				//var userId = (int)parameters.userid;
				var redeemedVoucher = redeemService.RedeemVoucher(parameters.voucher, "");
				//var shoppingCartItems = await productcatalog.GetShoppingCartItems(productcatalogIds).ConfigureAwait(false);
				//shoppingCart.AddItems(shoppingCartItems, eventStore);
				//shoppingCartStore.Save(shoppingCart);
				return redeemedVoucher;
			});
			Post("/redemption", async (parameters, _) => {
				//here you can rollback  a voucher
				var redeemDetails = this.Bind<Redemption>();
				//var userId = (int)parameters.userid;
				//var redeemedVoucher = redeemService.RedeemVoucher(parameters.voucher, "");
				//var shoppingCartItems = await productcatalog.GetShoppingCartItems(productcatalogIds).ConfigureAwait(false);
				//shoppingCart.AddItems(shoppingCartItems, eventStore);
				//shoppingCartStore.Save(shoppingCart);
				return redeemDetails.code;
			});
			Post("/createCSV", async (parameters, _) => {
				//here you can rollback  a voucher
				var redeemDetails = this.Bind<Redemption>();
				//var userId = (int)parameters.userid;
				ExportToCSVService.Main(redeemService.GetAllVouchers(redeemDetails.Email).Result);
				//var shoppingCartItems = await productcatalog.GetShoppingCartItems(productcatalogIds).ConfigureAwait(false);
				//shoppingCart.AddItems(shoppingCartItems, eventStore);
				//shoppingCartStore.Save(shoppingCart);
				return redeemDetails;
			});
		}
	}
}
