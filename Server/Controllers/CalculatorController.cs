using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Service;
using Server.Models;

namespace Server.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CalculatorController : ControllerBase
	{
		private readonly IOperations _operations;
		private static ListOfOperations listOfOperations = new ListOfOperations();
		private static Dictionary<string, ListOfOperations> dictionaryOfOperations = new Dictionary<string, ListOfOperations>();

		public CalculatorController(IOperations operations)
		{
			_operations = operations;
		}

		[HttpPost("add")]
		public async Task<IActionResult> Add([FromBody] Adds numbers, [FromHeader]string id){
			try{
				
				int sum = _operations.Add(numbers.addens);
				Sum add = new Sum
				{
					sum = sum
				};

				if (id.Length > 0)
				{
					Operation op = GetOperation(numbers.addens, sum, " + ");
					listOfOperations.operaciones.Add(op);
					if (!dictionaryOfOperations.ContainsKey(id))
					{
						dictionaryOfOperations.Add(id, new ListOfOperations());
					}
					dictionaryOfOperations[id].operaciones.Add(op);
				}

				return Ok(add);
			}catch(Exception e){
				return BadRequest(GenerateBadRequest());
			}
		}

		[HttpPost("sub")]
		public async Task<IActionResult> Sub([FromBody] Sub sub, [FromHeader] string id)
		{
			int[] numbers = { sub.minuend,sub.subtrahen};
			try{
				int result = _operations.Sub(numbers);
				Diference dif = new Diference
				{
					diference = result
				};
				if (id.Length > 0)
				{
					Operation op = GetOperation(numbers.ToList<int>(), result, " - ");
					listOfOperations.operaciones.Add(op);
					if (!dictionaryOfOperations.ContainsKey(id))
					{
						dictionaryOfOperations.Add(id, new ListOfOperations());
					}
					dictionaryOfOperations[id].operaciones.Add(op);
				}
				return Ok(dif);
			}catch(Exception e){
				return BadRequest(GenerateBadRequest());
			}
		}

		[HttpPost("mult")]
		public async Task<IActionResult> Mult([FromBody] Factors factors, [FromHeader] string id)
		{
			try{
				int result = _operations.Mult(factors.factors);
				Product product = new Product
				{
					product = result
				};
				
				if (id.Length > 0)
				{
					Operation op = GetOperation(factors.factors, result, " * ");
					listOfOperations.operaciones.Add(op);
					if (!dictionaryOfOperations.ContainsKey(id))
					{
						dictionaryOfOperations.Add(id, new ListOfOperations());
					}
					dictionaryOfOperations[id].operaciones.Add(op);
				}

				return Ok(product);
			}catch(Exception e){
				return BadRequest(GenerateBadRequest());
			}
		}

		[HttpPost("div")]
		public async Task<IActionResult> Div([FromBody] Div div, [FromHeader] string id)
		{
			int[] numbers = { div.dividend, div.divisor };
			try
			{
				int[] result = _operations.Div(numbers);
				DivResult divResult = new DivResult
				{
					 quotient= result[0],
					 remainder = result[1]
				};

				if (id.Length > 0)
				{
					var cadena =numbers[0]+ "/" + numbers[1] + "= quotient:" + result[0] + ", remainder:"+ result[1];
					Operation op = new Operation
					{
						operation = "div",
						calculation = cadena,
						date = DateTime.Now

					};

					if (!dictionaryOfOperations.ContainsKey(id))
					{
						dictionaryOfOperations.Add(id, new ListOfOperations());
					}
					dictionaryOfOperations[id].operaciones.Add(op);
				}

				return Ok(divResult);
			}
			catch (Exception e)
			{
				return BadRequest(GenerateBadRequest());
			}
		}

		[HttpPost("sqrt")]
		public async Task<IActionResult> Sqrt([FromBody] Sqrt sqrt, [FromHeader] string id)
		{
			int number =sqrt.number;
			try
			{
				int result = _operations.Sqrt(number);
				Square square = new Square
				{
					square = result
				};

				if (id.Length > 0)
				{
					var cadena = "square(" + number + ")="+result;
					Operation op = new Operation
					{
						operation = "square",
						calculation = cadena,
						date = DateTime.Now

					};
					if (!dictionaryOfOperations.ContainsKey(id))
					{
						dictionaryOfOperations.Add(id, new ListOfOperations());
					}
					dictionaryOfOperations[id].operaciones.Add(op);
				}

				return Ok(square);
			}
			catch (Exception e)
			{
				return BadRequest(GenerateBadRequest());
			}
		}

		[HttpGet("operations")]
		public async Task<IActionResult> GetAllOperationForTheUser([FromHeader]string id){
			try
			{
				return Ok(dictionaryOfOperations[id].operaciones);
			}catch(Exception e){
				return BadRequest(GenerateBadRequest());
			}
		}

		public Operation GetOperation(List<int> num, int sum, string sign)
		{
			string cadena = "";

			foreach (int n in num)
			{
				cadena += n +" " +sign + " ";
			}
			cadena = cadena.Substring(0, cadena.Length - 3) + "= " + sum;

			Operation op = new Operation
			{
				operation = "sum",
				calculation = cadena,
				date = DateTime.Now
			};

			return op;
		}

		public Error GenerateBadRequest(){
			return new Error
			{
				errorCode = "InternalError",
				errorStatus = 400,
				errorMessage = "Unable to process request..."
			};
		}

		public Error GenerateInternalError(){
			return new Error
			{
				errorCode = "InternalError",
				errorStatus = 500,
				errorMessage = "An unexpected error condition was triggered which made impossible to fulfill the request. Please try again or contact support."
			};
		}
	}
}
