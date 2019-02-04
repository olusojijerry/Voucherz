using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Business.Logic.Domain;
using Business.Logic.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using voucher.redeems.Domain;
using voucher.redeems.Models;

namespace voucher.redeems.Controllers
{
	[Produces("application/json")]
	[ApiController]
	[EnableCors()]
	public class RedemptionController : ControllerBase
	{
		private readonly IRedeemService _service;
		public RedemptionController(IRedeemService service)
		{
			_service = service;
		}
		[Route("")]
		[Route("voucherz")]
		[Route("voucherz/FrontDetails")]
		[HttpGet]
		public async Task<IActionResult> FrontDetails([FromBody]RedemptionDTO redemption)
		{
			try
			{
				var response = await _service.FrontEndDetails(redemption.Email);

				if (response == null)
					return new NotFoundObjectResult(new { Status = "Not Found", Description = "Redemption", Output = new { Result = "" } });
				else
					return new OkObjectResult(new { Status = "Details Created", Description = "Redemption", response });
			}
			catch (Exception ex)
			{
				//new NotFoundObjectResult();
				//new 

				//return new JsonResult(new response("Failed", "Redemption", null));
				return new BadRequestObjectResult(new { Status = "Bad Request", Error = ex.Message });
			}

		}
		[Route("voucherz/GetRedeemedDiscountVouchers")]
		[HttpGet]
		public async Task<IActionResult> GetRedeemedDiscountVouchers([FromBody]RedemptionDTO redemption)
		{
			try
			{
				var discountVouchers = await _service.GetAllRedeemedDiscountVouchers(redemption.Email);

				if (discountVouchers == null)
					return new NotFoundObjectResult(new { Status = "No Discount Voucher has been Created", Description = "Redemption", Output = new { Result = "" } });
				else
					return new OkObjectResult(new { Status = "Vouchers Retrieve", Description = "Redemption", discountVouchers });

			}
			catch (Exception ex)
			{
				return new BadRequestObjectResult(new { Status = "Bad Request", Error = ex.Message });
			}
		}
		[Route("voucherz/GetRedeemedValueVouchers")]
		[HttpGet]
		public async Task<IActionResult> GetRedeemedValueVouchers([FromBody]RedemptionDTO redemption)
		{
			try
			{
				var ValueVouchers = await _service.GetAllRedeemedValueVouchers(redemption.Email);

				if (ValueVouchers == null)
					return new NotFoundObjectResult(new { Status = "No Discount Voucher has been Created", Description = "Redemption", Output = new { Result = "" } });
				else
					return new OkObjectResult(new { Status = "Vouchers Retrieve", Description = "Redemption", ValueVouchers });

			}
			catch (Exception ex)
			{
				return new BadRequestObjectResult(new { Status = "Bad Request", Error = ex.Message });
			}
		}
		[Route("voucherz/GetRedeemedGiftVouchers")]
		[HttpGet]
		public async Task<IActionResult> GetRedeemedGiftVouchers([FromBody]RedemptionDTO redemption)
		{
			try
			{
				var GiftVouchers = await _service.GetAllRedeemGiftVouchers(redemption.Email);

				if (GiftVouchers == null)
					return new NotFoundObjectResult(new { Status = "No Discount Voucher has been Created", Description = "Redemption", Output = new { Result = "" } });
				else
					return new OkObjectResult(new { Status = "Vouchers Retrieve", Description = "Redemption", GiftVouchers });

			}
			catch (Exception ex)
			{
				return new BadRequestObjectResult(new { Status = "Bad Request", Error = ex.Message });
			}
		}
		[Route("voucherz/GetRedeemedVouchers")]
		[HttpGet]
		public async Task<IActionResult> GetRedeemedVouchers([FromBody]RedemptionDTO redemption)
		{
			try
			{
				var RedeemedVouchers = await _service.GetAllRedeemVouchers(redemption.Email);

				if (RedeemedVouchers == null)
					return new NotFoundObjectResult(new { Status = "No Discount Voucher has been Created", Description = "Redemption", Output = new { Result = "" } });
				else
					return new OkObjectResult(new { Status = "Vouchers Retrieve", Description = "Redemption", RedeemedVouchers });

			}
			catch (Exception ex)
			{
				return new BadRequestObjectResult(new { Status = "Bad Request", Error = ex.Message });
			}
		}
		[Route("voucherz/CreateCSV")]
		[HttpPost]
		public async Task<IActionResult> CreateCSV(string email)
		{
			try
			{
				await _service.createCsvFile(email);
				return new OkObjectResult(new { Status = "File Created", Description = "Redemption" });

			}
			catch (Exception ex)
			{
				return new BadRequestObjectResult(new { Status = "Bad Request", Error = ex.Message });
			}
		}
		[Route("voucherz/RedeemVoucher")]
		[HttpPost]
		public async Task<IActionResult> RedeemVoucher([FromBody]RedemptionDTO redemption)
		{
			
			try
			{
				//check if the voucher exist in redemption database if yes then its a gift voucher
				//if gift voucher then check its details to redeem
				var exist = await _service.ExecuteExist(redemption.VoucherCode);
				if (exist > 0)
				{
					var RedeemedDetail = _service.GetVoucher(redemption.VoucherCode).Result;

					int result = await _service.NextRedeemVoucher(RedeemedDetail, redemption.Amount);

					if(result > 0)
						return new OkObjectResult(new
						{
							Status = string.Format("Code {0} redeemed successfully", redemption.VoucherCode),
							Description = "Redemption", RedeemedDetail
						});
					return new OkObjectResult(new
					{
						Status = string.Format("Code {0} Can't be redeemed with this amount",
						redemption.VoucherCode),
						Description = "Redemption",
						RedeemedDetail
					});
				}
				else
				{
					//Get the voucher details from the voucher creation microservice
					var RedeemedDetails = await _service.GetResponse(redemption.VoucherCode);
					//check if voucher details is null
					if (RedeemedDetails == null)
						return new NotFoundObjectResult(new { Status = "Voucher is nil", Description = "Redemption", Output = new { Result = "" } });

					int result = await _service.RedeemVoucher(RedeemedDetails, redemption.Amount);
					if (result > 0)
						return new OkObjectResult(new
						{
							Status = string.Format("Code {0} redeemed successfully", redemption.VoucherCode),
							Description = "Redemption",
							RedeemedDetails
						});
					return new OkObjectResult(new
					{
						Status = string.Format("Code {0} Can't be redeemed with this amount",
						redemption.VoucherCode),
						Description = "Redemption",
						RedeemedDetails
					});
					//redemption.VoucherCode, redemption.Email, redemption.Amount);
				}


				//RedeemedDetails = await _service.RedeemVoucher(
				//redemption.VoucherCode, redemption.Email, redemption.Amount);


				//Redeem your voucher here




			}
			catch (Exception ex)
			{
				return new BadRequestObjectResult(new { Status = "Bad Request", Error = ex.Message });
			}
		}
	}
}
