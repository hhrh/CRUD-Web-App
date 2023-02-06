using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace crudMe
{
	[Route("api/[controller]")]
	public class ValuesController : Controller
	{
		//JSON
		//ActionResult is a return type of a controller/action method.
		//action methods return models to views, file streams, redirect to other controllers, etc.

		[HttpGet]
		public ActionResult Get()
		{
			//string array (just a test)
			//string[] data = new string[] { "one", "two", "three" };
			//return Ok( new { data });

			MySQLRepository mySQLRepository = new();
			List<PersonModel> data = mySQLRepository.Read();
			//if it were just "return OK {data}", no json (or no label "data"?);
			//Ok function used to return successful result
			return Ok( new { data });
		}

		//we dont want some idiot playing with the MVC, so we need POST
		[HttpPost]
		public ActionResult Post()
		{
			var status = false;

			//if request method is POST, use Request.Form.
			//input is from headers
			//data is posted in the http request body.
			var mode = Request.Form["mode"];
			var name = Request.Form["name"];
			var age = Request.Form["age"];
			var personId = Request.Form["personId"];

			MySQLRepository mySQLRepository = new();
			List<PersonModel> data = new();

			//for return code, or if error, exception message
			var code = "";
			switch (mode) {

				case "create":
					//the params here match the args in each CRUD function in MySQLRepository.cs
					try {
						mySQLRepository.Create(name, Convert.ToInt32(age));

						code = ((int)ReturnCode.CREATE_SUCCESS).ToString();
						status = true;
					} catch (Exception ex) {
						code = ex.Message;	//will actually show msg in postman
					}
					break;

				case "read":
					try {
						data = mySQLRepository.Read();
						code = ((int)ReturnCode.READ_SUCCESS).ToString();
						status = true;
					} catch (Exception ex) {
						code = ex.Message;
					}
					break;

				case "update":
					try {
						mySQLRepository.Update(name, Convert.ToInt32(age), Convert.ToInt32(personId));
						code = ((int)ReturnCode.UPDATE_SUCCESS).ToString();
						status = true;
					} catch (Exception ex) {
						code = ex.Message;
					}
					break;

				case "delete":
					try {
						mySQLRepository.Delete(Convert.ToInt32(personId));
						code = ((int)ReturnCode.DELETE_SUCCESS).ToString();
						status = true;
					} catch (Exception ex) {
						code = ex.Message;
					}
					break;

				default:
					//error
					code = ((int)ReturnCode.ACCESS_DENIED_NO_MODE).ToString();
					break;
			}

			return Ok(new { status, code, data });
		}
	}
}

