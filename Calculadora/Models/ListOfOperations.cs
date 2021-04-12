using System.Collections.Generic;

namespace Calculadora.Models
{
	public class ListOfOperations
	{
		public List<Operation> operaciones { get; set; }

		public ListOfOperations(){
			operaciones = new List<Operation>();
		}

	}
}
