using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using VoresCarlsberg.Web.Models;

namespace VoresCarlsberg.Application.Services
{
	public class GuestService
	{
		public GuestModel GetGuest(LoginModel login)
		{
			var db = new PetaPoco.Database("myDB");

			var guest = db.SingleOrDefault<GuestModel>(
				"SELECT * FROM Guests WHERE EmployeeNo = @0",
				login.EmployeeNo);

			return guest;
		}

		// ---------------------------------------------------------------------------

		[Authorize]
		public void SaveGuest(GuestModel guest)
		{
			var db = new PetaPoco.Database("myDB");

			// Guest exists?
			var existingGuest = db.SingleOrDefault<GuestModel>(
				"SELECT * FROM Guests WHERE EmployeeNo = @0",
				guest.EmployeeNo);

			guest.Edited = DateTime.Now;

			if (existingGuest != null)
			{
				db.Update("Guests", "EmployeeNo", guest);
			}
			else
			{
				guest.Created = DateTime.Now;
				db.Insert("Guests", "EmployeeNo", guest);
			}
		}
	}
}
