using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoresCarlsberg.Web.Models
{
	public class GuestModel
	{
		public string Name { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public bool Newsletter { get; set; }
		public string DealerName { get; set; }
		public string DealerEmail { get; set; }
		public string PostalCode { get; set; }
		public string Description { get; set; }
	}
}
