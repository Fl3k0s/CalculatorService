﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Models;
using Server.Service;

namespace Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PruebaController : ControllerBase
	{
		public PruebaController(IOperations o){

		}

		public static Sub sub = new Sub
		{
			minuend = 2,
			subtrahen = 4
		};

		public static Factors f = new Factors
		{
			factors = new List<int>()
		};
		
		
		[HttpGet]
		public async Task<Factors> Prueba(){
			f.factors.Add(1);
			f.factors.Add(2);
			f.factors.Add(3);
			return f;
		}

		[HttpPost("post")]
		public async Task<Sub> PruebaPost([FromBody] Sub suma){
			return suma;
		}
	}
}