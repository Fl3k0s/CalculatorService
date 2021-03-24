﻿using Microsoft.AspNetCore.Http;
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

		public CalculatorController(IOperations operations)
		{
			_operations = operations;
		}

		[HttpPost("add")]
		public IActionResult Add([FromBody] List<int> numbers){
			try{
				int sum = _operations.Add(numbers);
				Sum add = new Sum
				{
					sum = sum
				};
				return Ok(add);
			}catch(Exception e){
				return BadRequest(GenerateBadRequest());
			}
			
		}

		[HttpPost("sub")]
		public IActionResult Sub([FromBody] Sub sub){
			int[] numbers = { sub.minuend,sub.subtrahen};
			try{
				int result = _operations.Sub(numbers);
				Diference dif = new Diference
				{
					diference = result
				};
				return Ok(dif);
			}catch(Exception e){
				return BadRequest(GenerateBadRequest());
			}
		}
		[HttpPost("mult")]
		public IActionResult Mult([FromBody] Factors factors){
			List<int> numbers = factors.factors;

			try{
				int result = _operations.Mult(numbers);
				Product product = new Product
				{
					product = result
				};
				return Ok(product);
			}catch(Exception e){
				return BadRequest(GenerateBadRequest());
			}
		}

		[HttpPost("div")]
		public IActionResult Div([FromBody] Div div)
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
				return Ok(divResult);
			}
			catch (Exception e)
			{
				return BadRequest(GenerateBadRequest());
			}
		}
		[HttpPost("sqrt")]
		public IActionResult Sqrt([FromBody] Sqrt sqrt){
			int number =sqrt.number;
			try
			{
				int result = _operations.Sqrt(number);
				Square square = new Square
				{
					square = result
				};
				return Ok(square);
			}
			catch (Exception e)
			{
				return BadRequest(GenerateBadRequest());
			}
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
