using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRedemption.Domain
{
	public class RedemptionDTO
	{
		public string VoucherCode { get; set; }
		public string Email { get; set; }
		public double Amount { get; set; }
	}
}
