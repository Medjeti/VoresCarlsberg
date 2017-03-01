using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using PetaPoco;
using VoresCarlsberg.Web.Models;

namespace VoresCarlsberg.Application.Services
{
	public class GuestService
	{
		private Database _db;

		public GuestService()
		{
			_db = new PetaPoco.Database("myDB");
		}

		// ---------------------------------------------------------------------------

		public GuestModel GetGuest(LoginModel login)
		{
			var guest = _db.SingleOrDefault<GuestModel>(
				"SELECT * FROM Guests WHERE EmployeeNo = @0",
				login.EmployeeNo);

			return guest;
		}

		// ---------------------------------------------------------------------------

		[Authorize]
		public void SaveGuest(GuestModel guest)
		{
			// Guest exists?
			var existingGuest = _db.SingleOrDefault<GuestModel>(
				"SELECT * FROM Guests WHERE EmployeeNo = @0",
				guest.EmployeeNo);

			// Not attending? Only update IsAttending bit

			guest.Edited = DateTime.Now;
			//guest.Created = DateTime.Now;

			if (existingGuest != null)
			{
				if (guest.IsAttending)
				{
					_db.Update("Guests", "EmployeeNo", guest);
				}
				else
				{
					_db.Update("Guests", "EmployeeNo", new {IsAttending = false}, guest.EmployeeNo);
				}
			}
			else
			{
				guest.Created = DateTime.Now;
				_db.Insert("Guests", "EmployeeNo", guest);
			}
		}
	}
}
