using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Business.Logic.Domain
{
	public class redeemCode
	{
		public string code { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string VoucherCode { get; set; }
		[Required]
		public string CodeStatus { get; set; }
		public int? TimesRedeemed { get; set; }
		public bool RedeemedStatus { get; set; }
		public double VoucherType { get; set; }
	}
}
