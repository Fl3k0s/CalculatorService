using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
	public class Error
	{
		public string errorCode { get; set; }
		public int errorStatus { get; set; }
		public string errorMessage { get; set; }

	}
}
