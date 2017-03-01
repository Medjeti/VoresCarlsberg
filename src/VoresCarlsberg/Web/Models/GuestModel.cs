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

		public string Bus { get; set; }

		public string FavoriteBeer { get; set; }
		public string SelectedHobbies { get; set; }
		public string OtherHobby { get; set; }
		public string BandSong { get; set; }

		public string Allergies { get; set; }

		public string ColleagueName { get; set; }
		public string ColleagueDivision { get; set; }

		public DateTime Created { get; set; }
		public DateTime Edited { get; set; }

		// ----------------------------------------------------

		[ResultColumn]
		public string FullName { get; set; }
	
	}
}
