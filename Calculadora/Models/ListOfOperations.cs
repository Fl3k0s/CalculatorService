using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


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
