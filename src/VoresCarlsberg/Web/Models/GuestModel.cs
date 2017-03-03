using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace VoresCarlsberg.Web.Models
{
	public class GuestModel
	{
		public bool IsAttending { get; set; }
		public int EmployeeNo { get; set; }
		public string PhoneNo { get; set; }

		public string BusOut { get; set; }
		public string BusHome { get; set; }

		public string FavoriteBeer { get; set; }
		public string SelectedHobbies { get; set; }
		public string OtherHobby { get; set; }
		public string BandSong { get; set; }

		public string Allergies { get; set; }

		public string ColleagueName { get; set; }
		public string ColleagueDivision { get; set; }
		public string ColleagueDescription { get; set; }


		public DateTime Created { get; set; }
		public DateTime Edited { get; set; }

		// ----------------------------------------------------

		[ResultColumn]
		public string FullName { get; set; }

		[ResultColumn]
		public string Position { get; set; }

		[ResultColumn]
		public string PersonnelSubarea { get; set; }

		[ResultColumn]
		public string Location { get; set; }

	}
}
