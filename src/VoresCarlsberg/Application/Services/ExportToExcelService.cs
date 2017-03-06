using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing;
using PetaPoco;
using VoresCarlsberg.Web.Models;

namespace VoresCarlsberg.Application.Services
{
	public class ExportToExcelService
	{
		private Database _db;

		public ExportToExcelService()
		{
			_db = new PetaPoco.Database("myDB");
		}

		// ---------------------------------------------------------------------------

		public Stream Generate()
		{
			var guests =
				_db.Query<GuestModel>(
					"SELECT * FROM Guests INNER JOIN (SELECT EmployeeNo, FullName, Position, PersonnelSubarea, Location FROM Employees) AS tmp1 ON Guests.EmployeeNo = tmp1.EmployeeNo ORDER BY Created ASC");


			var dt = new DataTable();

			dt.Columns.Add("Meldt til?");
			dt.Columns.Add("Medarbejdernr.");
			dt.Columns.Add("Navn");
			dt.Columns.Add("Stilling");
			dt.Columns.Add("Afdeling");
			dt.Columns.Add("Lokation");

			dt.Columns.Add("Tlf.");
			dt.Columns.Add("Bus ud");
			dt.Columns.Add("Bus hjem");
			dt.Columns.Add("Hvad drikker du helst?");
			dt.Columns.Add("Hobby 1");
			dt.Columns.Add("Hobby 2");
			dt.Columns.Add("Sang til bandet");
			dt.Columns.Add("Allergier");
			//dt.Columns.Add("Kollega navn");
			//dt.Columns.Add("Kollega afdeling");
			//dt.Columns.Add("Kollega beskrivelse");

			dt.Columns.Add("Oprettet");
			dt.Columns.Add("Redigeret");

			foreach (var guest in guests)
			{
				var otherKeyword = "Andet";
				var hobbiesString = guest.SelectedHobbies.Replace(otherKeyword, guest.OtherHobby);
				var hobbies = hobbiesString.Split(',');
				//var hobby1 = hobbies.Any() ? hobbies[0] : "";
				//var hobby2 = hobbies.Count() > 1 && hobbies[1] != otherKeyword ? hobbies[1] : guest.OtherHobby;
				var hobby1 = hobbies.Any() ? hobbies[0] : "";
				var hobby2 = hobbies.Count() > 1 ? hobbies[1] : "";

				dt.Rows.Add(
					guest.IsAttending,
					guest.EmployeeNo, 
					guest.FullName,
					guest.Position,
					guest.PersonnelSubarea,
					guest.Location,
					guest.PhoneNo,
					guest.BusOut,
					guest.BusHome,
					guest.FavoriteBeer,
					hobby1,
					hobby2,
					guest.BandSong,
					guest.Allergies,
					//guest.ColleagueName,
					//guest.ColleagueDivision,
					//guest.ColleagueDescription,
					guest.Created,
					guest.Edited
				);
			}

			var filePath = HttpContext.Current.Server.MapPath("/tmp/tilmeldinger.xslx");
			var fi = new FileInfo(filePath);
			if (fi.Exists)
			{
				fi.Delete();
			}

			using (var pck = new ExcelPackage(fi))
			{
				//Create the worksheet
				ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Tilmeldinger");

				//Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
				ws.Cells["A1"].LoadFromDataTable(dt, true);

				pck.Save();
				return pck.Stream;
			}

			//var excelPackage = new ExcelPackage();
			//var sheet = excelPackage.Workbook.Worksheets.Add("Tilmeldinger");
			//var ws = excelPackage.Workbook.Worksheets[1];
			//ws.Name = "Tilmeldinger";

			//sheet.Cells["A1"].Value = "Medarbejdernr.";
			////sheet.Cells["B1"].Value = "Navn";
			////sheet.Cells["C1"].Value = "Stilling";

			////sheet.Cells["A1"].LoadFromDataTable(dt, true);
			//excelPackage.Save();

			//return excelPackage;
			
		}
		
	}
}
