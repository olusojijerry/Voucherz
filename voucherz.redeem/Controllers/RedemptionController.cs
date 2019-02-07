using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Logic.Services;
using Microsoft.AspNetCore.Mvc;
using voucherz.redeem.Model;

namespace voucherz.redeem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RedemptionController : ControllerBase
	{
		private readonly IRedeemService _service;
		public RedemptionController(IRedeemService service)
		{
			_service = service;
		}
		// GET api/values
		[HttpGet("details/{email}")]
		public async Task<IActionResult> FrontDetails(string email)
		{
			try
			{
				var response = await _service.FrontEndDetails(email);

				if (response == null)
					return new NotFoundObjectResult(new { Status = "Not Found", Description = "Redemption", Output = new { Result = "" } });
				else
					return new OkObjectResult(new { Status = "Details Created", Description = "Redemption", response });
			}
			catch (Exception ex)
			{
				return new BadRequestObjectResult(new { Status = "Bad Request", Error = ex.Message });
			}

		}

		// GET api/redemption/getdiscountvouchers/{email}
		[HttpGet("getdiscountvouchers/{email}")]
		public async Task<IActionResult> GetRedeemedDiscountVouchers(string email)
		{
			try
			{
				var discountVouchers = await _service.GetAllRedeemedDiscountVouchers(email);

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

		// get
		[HttpGet("redeemvaluevoucher/{email}")]
		public async Task<IActionResult> GetRedeemedValueVouchers(string email)
		{
			try
			{
				var ValueVouchers = await _service.GetAllRedeemedValueVouchers(email);

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

		// get
		[HttpGet("getgiftvoucher/{email}")]
		public async Task<IActionResult> GetRedeemedGiftVouchers(string email)
		{
			try
			{
				var GiftVouchers = await _service.GetAllRedeemGiftVouchers(email);

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

		// get
		[HttpGet("getallvouchers/{email}")]
		public async Task<IActionResult> GetRedeemedVouchers(string email)
		{
			try
			{
				var RedeemedVouchers = await _service.GetAllRedeemVouchers(email);

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

		[HttpPost("redeems")]
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

					if (result > 0)
						return new OkObjectResult(new
						{
							Status = string.Format("Code {0} redeemed successfully", redemption.VoucherCode),
							Description = "Redemption",
							RedeemedDetail
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
						return new NotFoundObjectResult(new { Status = "Voucher has been redeemed or does not exit;", Description = "Redemption", Output = new { Result = "" } });

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
				}
				
			}
			catch (Exception ex)
			{
				return new BadRequestObjectResult(new { Status = "Bad Request", Error = ex.Message });
			}
		}
	}
}
