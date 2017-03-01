using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;
using VoresCarlsberg.Web.Models;

namespace VoresCarlsberg.Application.Services
{
	public class LoginService
	{
		public bool LoginUser(LoginModel login)
		{
			var db = new PetaPoco.Database("myDB");

			var query = db.SingleOrDefault<EmployeeModel>(
				"SELECT * FROM Employees WHERE CAST(EmployeeNo AS nvarchar(50)) = @0 AND FirstName = @1",
				login.EmployeeNo, login.FirstName);

			return query != null;
		}
	}
}
