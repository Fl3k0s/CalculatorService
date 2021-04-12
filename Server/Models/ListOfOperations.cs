using System.Collections.Generic;


namespace Server.Models
{
	public class ListOfOperations
	{
		public List<Operation> operaciones { get; set; }

		public ListOfOperations(){
			operaciones = new List<Operation>();
		}
	}
}
