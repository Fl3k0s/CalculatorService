using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using NLog;

using Server.Service;
using Server.Models;

namespace Server.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CalculatorController : ControllerBase
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private static StreamWriter sw = new StreamWriter("log.txt");
		private readonly IOperations _operations;
		private static ListOfOperations listOfOperations = new ListOfOperations();
		private static ConcurrentDictionary<string, ListOfOperations> dictionaryConcurrent = new ConcurrentDictionary<string, ListOfOperations>();
		private Operation GetOperation(List<int> num, int sum, string sign, string operation)
		{
			var cadena = "";

			foreach (int n in num)
			{
				cadena += n + " " + sign + " ";
			}
			cadena = cadena.Substring(0, cadena.Length - 3) + "= " + sum;

			var op = new Operation
			{
				operation = operation,
				calculation = cadena,
				date = DateTime.Now
			};

			return op;
		}

		private Error GenerateBadRequest()
		{
			return new Error
			{
				errorCode = "InternalError",
				errorStatus = 400,
				errorMessage = "Unable to process request..."
			};
		}

		private Error GenerateInternalError()
		{
			return new Error
			{
				errorCode = "InternalError",
				errorStatus = 500,
				errorMessage = "An unexpected error condition was triggered which made impossible to fulfill the request. Please try again or contact support."
			};
		}

		//create instace of the service
		public CalculatorController(IOperations operations)
		{
			_operations = operations;
		}

		//the function who have the add operation
		[HttpPost("add")]
		public async Task<IActionResult> Add([FromBody] Adds numbers, [FromHeader] string id)
		{
			if (id == null)
				id = "";

			try
			{
				//this call a function who have the code that operation and return the result
				var sum = _operations.Add(numbers.addens);
				//create the Sum object
				var add = new Sum
				{
					sum = sum
				};

				//if the user have id, save the operation
				if (id.Length > 0)
				{
					var op = GetOperation(numbers.addens, sum, " + ", "sum");
					listOfOperations.operaciones.Add(op);

					if (!dictionaryConcurrent.ContainsKey(id))
					{
						dictionaryConcurrent.TryAdd(id, new ListOfOperations());
					}
					dictionaryConcurrent[id].operaciones.Add(op);
				}
				return Ok(add);
			}
			catch (Exception e)
			{
				sw.Write(e);
				sw.Flush();
				sw.Close();
				//if have an error return bad request
				return BadRequest(GenerateBadRequest());
			}
		}

		//the function who have the sub operation
		[HttpPost("sub")]
		public async Task<IActionResult> Sub([FromBody] Sub sub, [FromHeader] string id)
		{
			//save the two numbers in this array to send in the sub function
			int[] numbers = { sub.minuend, sub.subtrahen };

			if (id == null)
				id = "";

			try
			{
				//this call a function who have the code that operation and return the result
				var result = _operations.Sub(numbers);

				//create the Diference object
				var dif = new Diference
				{
					diference = result
				};

				//if the user have id, save the operation
				if (id.Length > 0)
				{
					var op = GetOperation(numbers.ToList<int>(), result, " - ", "sub");
					listOfOperations.operaciones.Add(op);

					if (!dictionaryConcurrent.ContainsKey(id))
					{
						dictionaryConcurrent.TryAdd(id, new ListOfOperations());
					}
					dictionaryConcurrent[id].operaciones.Add(op);
				}
				return Ok(dif);
			}
			catch (NullReferenceException e)
			{
				sw.Write(e);
				sw.Flush();
				sw.Close();
				//if have an error return bad request
				return BadRequest(GenerateBadRequest());
			}
		}

		[HttpPost("mult")]
		public async Task<IActionResult> Mult([FromBody] Factors factors, [FromHeader] string id)
		{
			if (id == null)
				id = "";

			try
			{
				//this call a function who have the code that operation and return the result
				var result = _operations.Mult(factors.factors);

				//create the Product object
				var product = new Product
				{
					product = result
				};

				//if the user have id, save the operation
				if (id.Length > 0)
				{
					var op = GetOperation(factors.factors, result, " * ", "mult");
					listOfOperations.operaciones.Add(op);

					if (!dictionaryConcurrent.ContainsKey(id))
					{
						dictionaryConcurrent.TryAdd(id, new ListOfOperations());
					}
					dictionaryConcurrent[id].operaciones.Add(op);
				}

				return Ok(product);
			}
			catch (NullReferenceException e)
			{
				sw.Write(e);
				sw.Flush();
				sw.Close();
				//if have an error return bad request
				return BadRequest(GenerateBadRequest());
			}
		}

		[HttpPost("div")]
		public async Task<IActionResult> Div([FromBody] Div div, [FromHeader] string id)
		{
			if (id == null)
				id = "";

			int[] numbers = { div.dividend, div.divisor };
			try
			{
				//this call a function who have the code that operation and return the result
				int[] result = _operations.Div(numbers);

				//create the Division object
				var divResult = new DivResult
				{
					quotient = result[0],
					remainder = result[1]
				};

				//if the user have id, save the operation
				if (id.Length > 0)
				{
					var cadena = numbers[0] + "/" + numbers[1] + "= quotient:" + result[0] + ", remainder:" + result[1];
					var op = new Operation
					{
						operation = "div",
						calculation = cadena,
						date = DateTime.Now

					};

					if (!dictionaryConcurrent.ContainsKey(id))
					{
						dictionaryConcurrent.TryAdd(id, new ListOfOperations());
					}
					dictionaryConcurrent[id].operaciones.Add(op);
				}

				return Ok(divResult);
			}
			catch (NullReferenceException e)
			{
				sw.Write(e);
				sw.Flush();
				sw.Close();
				//if have an error return bad request
				return BadRequest(GenerateBadRequest());
			}
		}

		[HttpPost("sqrt")]
		public async Task<IActionResult> Sqrt([FromBody] Sqrt sqrt, [FromHeader] string id)
		{
			if (id == null)
				id = "";

			var number = sqrt.number;
			try
			{
				//this call a function who have the code that operation and return the result
				int result = _operations.Sqrt(number);

				//create the Square object
				var square = new Square
				{
					square = result
				};

				//if the user have id, save the operation
				if (id.Length > 0)
				{
					var cadena = "square(" + number + ")=" + result;
					var op = new Operation
					{
						operation = "square",
						calculation = cadena,
						date = DateTime.Now

					};

					if (!dictionaryConcurrent.ContainsKey(id))
					{
						dictionaryConcurrent.TryAdd(id, new ListOfOperations());
					}
					dictionaryConcurrent[id].operaciones.Add(op);
				}

				return Ok(square);
			}
			catch (NullReferenceException e)
			{
				sw.Write(e);
				sw.Flush();
				sw.Close();
				//if have an error return bad request
				return BadRequest(GenerateBadRequest());
			}
		}

		[HttpGet("operations")]
		public async Task<IActionResult> GetAllOperationForTheUser([FromHeader] string id)
		{
			try
			{
				//return the operation that the user have this id
				return Ok(dictionaryConcurrent[id].operaciones);
			}
			catch (NullReferenceException e)
			{
				sw.Write(e);
				sw.Flush();
				sw.Close();
				//if have an error return bad request
				return BadRequest(GenerateBadRequest());
			}
		}


	}
}
