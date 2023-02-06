using System;
namespace crudMe
{
	//represents the model to manipulate.
	//also represents a row in the db
	//these can be accessed to be read, write, etc.
	public class PersonModel
	{
		public int PersonId { get; set; }
		public string Name { get; set; }
		public int Age { get; set; }
	}
}

