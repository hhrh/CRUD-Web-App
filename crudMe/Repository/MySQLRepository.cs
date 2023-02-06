using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

///	file contains all crud functions.
///	contains configuration object with appsettings.json in it
///	gets the connectionstring section of that json
///	
/// for each crud function:
/// a MySqlConnection is open using the connection string
/// a MySqlCommand is made using a SQL code string, and the connection string
/// addwithvalue changes the "placeholders"(starts with @) in the sql code, with a value.
/// the changes get executed and commited. (then disposed)



//connect mySQL (nuget)
namespace crudMe
{
	public class MySQLRepository
	{
		private string _connectionString { get; set; }

		public MySQLRepository()
		{
			IConfiguration configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddEnvironmentVariables()
				.Build();
			//.AddJsonFile loads appsettings.json into configuration object (extracts each property and value)
			//.AddEnviromentVariables is useful for cross-platform / contatainor deployments
			//it is loaded after addjsonfile, any duplicate keys will replace values

			Console.WriteLine("$Contents of Default Property: {Default}"); //test

			_connectionString = configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
			//returns config subsection key, use : to get child
		}

		public void Create(string name, int age) {

			//create skeleton for code (For Create, Read, Update, Delete)

			MySqlTransaction mySqlTransaction = null;

			//establish connection, uses connection string to provide info for connecting
			//"using" ensures that connection object is disposed after scope
			using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
				try {

					connection.Open(); //opens db connection
					mySqlTransaction = connection.BeginTransaction();

					string SQL = "INSERT INTO person VALUES (null, @name, @age);"; //SQL code
					MySqlCommand mySqlCommand = new MySqlCommand(SQL, connection);

					//add params for new MySqlCommand obj:
					mySqlCommand.Parameters.AddWithValue("@name", name); //(name of param, value), returns SqlParameter
					mySqlCommand.Parameters.AddWithValue("@age", age);
					mySqlCommand.ExecuteNonQuery(); //used to execute queries that dont retrieve data (like update, insert, delete), returns num of rows effected

					mySqlTransaction.Commit(); //commit, update record after it has been changed
					mySqlCommand.Dispose(); //clear some memory?

				} catch (MySqlException ex) {
					System.Diagnostics.Debug.WriteLine(ex.Message); //throw exception
				}
			}
		}

		public List<PersonModel> Read() { //list allows finding, sorting, search, manipulating a list of objs
			List<PersonModel> personModels = new();


			//a transaction is a unit of work performed against a data set.
			//reading from data does not change anything in the data, so mySqlTransaction is not needed:
			//MySqlTransaction mySqlTransaction = null;

			//establish connection, uses connection string to provide info for connecting
			//"using" ensures that connection object is disposed after scope
			using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
				try {

					connection.Open(); //opens db connection
					string SQL = "SELECT * FROM person;"; //SQL code
					MySqlCommand mySqlCommand = new MySqlCommand(SQL, connection);

					using (var reader = mySqlCommand.ExecuteReader()) { //execute reader: execute a command and return a set of rows from the db
						//returns SqlDataReader obj (reader)

						while (reader.Read()) {

							//it all depends, for ease, we just send all data as string.
							//web api may translate to "" or numbers only

							//adding to personModels List

							personModels.Add(new PersonModel() {
								Name = reader["name"].ToString(), //tostring gets a string
								Age = Convert.ToInt32(reader["age"]),
								PersonId = Convert.ToInt32(reader["personId"])
							});
						}
					}

					//add params for new MySqlCommand obj:
					//none

					//mySqlTransaction.Commit(); //commit, update record after it has been changed
					mySqlCommand.Dispose(); //clear some memory?

				} catch (MySqlException ex) {
					System.Diagnostics.Debug.WriteLine(ex.Message); //throw exception
				}
			}

			return personModels;
		}

		public void Update(string name, int age, int personId) {

			MySqlTransaction mySqlTransaction = null;

			//establish connection, uses connection string to provide info for connecting
			//"using" ensures that connection object is disposed after scope
			using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
				try {

					connection.Open(); //opens db connection
					mySqlTransaction = connection.BeginTransaction();

					string SQL = "UPDATE person SET name = @name, age = @age WHERE personId = @personId;"; //SQL code
					MySqlCommand mySqlCommand = new MySqlCommand(SQL, connection);

					//add params for new MySqlCommand obj:
					mySqlCommand.Parameters.AddWithValue("@name", name); //(name of param, value), returns SqlParameter
					mySqlCommand.Parameters.AddWithValue("@age", age);
					mySqlCommand.Parameters.AddWithValue("@personId", personId); //(name of param, value), returns SqlParameter

					mySqlCommand.ExecuteNonQuery(); //used to execute queries that dont retrieve data (like update, insert, delete), returns num of rows effected

					mySqlTransaction.Commit(); //commit, update record after it has been changed
					mySqlCommand.Dispose(); //clear some memory?

				} catch (MySqlException ex) {
					System.Diagnostics.Debug.WriteLine(ex.Message); //throw exception
				}
			}

		}

		public void Delete(int personId) {

			MySqlTransaction mySqlTransaction = null;

			//establish connection, uses connection string to provide info for connecting
			//"using" ensures that connection object is disposed after scope
			using (MySqlConnection connection = new MySqlConnection(_connectionString)) {
				try {

					connection.Open(); //opens db connection
					mySqlTransaction = connection.BeginTransaction();

					string SQL = "DELETE FROM person WHERE personId = @personId;"; //SQL code
					MySqlCommand mySqlCommand = new MySqlCommand(SQL, connection);

					//add params for new MySqlCommand obj:
					mySqlCommand.Parameters.AddWithValue("@personId", personId); //(name of param, value), returns SqlParameter

					mySqlCommand.ExecuteNonQuery(); //used to execute queries that dont retrieve data (like update, insert, delete), returns num of rows effected

					mySqlTransaction.Commit(); //commit, update record after it has been changed
					mySqlCommand.Dispose(); //clear some memory?

				} catch (MySqlException ex) {
					System.Diagnostics.Debug.WriteLine(ex.Message); //throw exception
				}
			}

		}
	}
}

