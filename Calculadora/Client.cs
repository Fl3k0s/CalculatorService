﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Calculadora.Models;
using System.Text.Json;
using System.Threading.Tasks;

namespace Calculadora
{
	public class Client
	{
		private static List<int> numbers;
		private static HttpClient client;
		private static string trackId;
		private static HttpClientHandler clientHandler = new HttpClientHandler();


		public static void Main(string[] args)
		{
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);
			//ask the user for him id
			Console.WriteLine("Tell me your id (if you don have, relax, can be empty)");
			trackId = Console.ReadLine();
			trackId = trackId.Trim();
			//add the id to the header that JSON
			client.DefaultRequestHeaders.Add("id", trackId);
			
			//the menu of the app
			Action();

			//if exist id, see the operations
			if (trackId.Length > 0)
				OperationsPerformed();

			Console.WriteLine("See you next time");
		}


		public async static void Action(){
			//comand who tell the action of the user
			string command;
			do
			{
				//show the comands and the use 
				Console.WriteLine("Introduce add to add the numbers");
				Console.WriteLine("Introduce sub to sub the numbers");
				Console.WriteLine("Introduce mult to mult the numbers");
				Console.WriteLine("Introduce div to div the numbers");
				Console.WriteLine("Introduce sqrt to have the Square of the numbers");
				Console.WriteLine("Introduce exit to finish the program");

				//leemos el comando que quiere el usuario
				command = Console.ReadLine().ToLower().Trim();

				//in the case of add or mult the client have entered 2 or more numbers
				//in the case of sub and div the client have entered 2 numbers
				//in the case of sqrt the client have entered only 1 number
				//once selected, do the operation
				switch (command)
				{
					case "add":
						ReadNumbers();
						Add();
						break;
					case "sub":
						ReadOnlyTwoNumbers();
						Sub();
						break;
					case "mult":
						ReadNumbers();
						Mult();
						break;
					case "div":
						ReadOnlyTwoNumbers();
						Div();
						break;
					case "sqrt":
						int n = ReadNumber();
						Square(n);
						break;
					case "exit":
						break;
					default:
						//if the user writes a command that does not correspond or writes a wrong command they will be informed
						Console.WriteLine("comand not exist, try again");
						break;						
				}	
			} while (!command.Equals("exit"));
		}

		//the add operation
		public async static void Add()
		{
			string url = $"http://localhost:5000/calculator/add";

			Adds add = new Adds
			{
				addens = numbers
			};

			
			var request = ApiCall(url, add);
			var result = JsonSerializer.Deserialize<Sum>(request.Result.ToString());
			Console.WriteLine(result.sum);
		}

		//the sub operation
		public async static void Sub(){
			string url = $"http://localhost:5000/calculator/sub";
			Sub sub = new Sub
			{
				minuend = numbers[0],
				subtrahen = numbers[1]
			};

			var request = ApiCall(url, sub);
			var result = JsonSerializer.Deserialize<Diference>(request.Result.ToString());
			Console.WriteLine(result.diference);
		}

		//the multiplication operation
		public async static void Mult(){
			string url = $"http://localhost:5000/calculator/mult";

			Factors fact = new Factors
			{
				factors = numbers
			};

			var request = ApiCall(url, fact);
			var result = JsonSerializer.Deserialize<Product>(await request);
			Console.WriteLine(result.product);
		}

		//the divide operation
		public async static void Div(){
			string url = $"http://localhost:5000/calculator/div";

			Div div = new Div
			{
				dividend = numbers[0],
				divisor = numbers[1]
			};

			var request = ApiCall(url, div);
			var result = JsonSerializer.Deserialize<DivResult>(request.Result.ToString());
			Console.WriteLine("quotient: "+ result.quotient);
			Console.WriteLine("remainder: " + result.remainder);
		}

		//the square operation
		public async static void Square(int n){
			string url = $"http://localhost:5000/calculator/sqrt";
			Sqrt sqrt = new Sqrt {
				number = n
			};
			var request = ApiCall(url, sqrt);
			var result = JsonSerializer.Deserialize<Square>(request.Result.ToString());
			Console.WriteLine(result.square);

		}

		//if the user have id see the operations whith this function
		public async static void OperationsPerformed(){
			string url = $"http://localhost:5000/calculator/operations";

			var response = client.GetAsync(url).Result;
			string responseString = "";
			responseString = response.Content.ReadAsStringAsync().Result.ToString();
			//if have operation see
			if (response.StatusCode == HttpStatusCode.OK)
			{
				var result = JsonSerializer.Deserialize<List<Operation>>(responseString);
				foreach (Operation o in result)
				{
					Console.WriteLine("--------------------");
					Console.WriteLine("Operation:"+o.operation);
					Console.WriteLine("Calculation:"+o.calculation);
					Console.WriteLine("Date:"+o.date);
				}
			}
			//else see the message error
			else
			{
				var result = JsonSerializer.Deserialize<List<Error>>(responseString);
				Console.WriteLine(response.RequestMessage);
				
			}
			
		}
		
		//call the api and return the result
		public async static Task<string> ApiCall(string url, Object o)
		{
			var json = JsonSerializer.Serialize(o);
			var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
			
			var response = await client.PostAsync(url, content);

			if (response.StatusCode == HttpStatusCode.OK)
			{
				var responseString = await response.Content.ReadAsStringAsync();
				return responseString;
			}
			else
			{
				Console.WriteLine(response.ToString());
				return "";
			}
		}

		//the function that read only two numbers (for div and sub)
		public static void ReadOnlyTwoNumbers(){
			numbers = new List<int>();
			//number to introduce in the list
			int n;
			
			do
			{
				try
				{
					//ask the number to user
					Console.WriteLine("Tell me the number who do you wish operate");
					n = int.Parse(Console.ReadLine());

					//add the number at list
					numbers.Add(n);
				}
				catch (Exception e)
				{
					//If the number is badly entered, we communicate it to the user
					Console.WriteLine("number badly entered");
					continue;
				}

			} while ((numbers.Count < 2));
		}

		//the function that read only one number (for square)
		public static int ReadNumber(){
			
			//number to introduce in the list
			int n;
			bool exit = false;
			while (!exit){
				try
				{
					//ask the number to user
					Console.WriteLine("Tell me the number who do you wish operate");
					n = int.Parse(Console.ReadLine());
					exit = true;

					return n;
				}
				catch (Exception e)
				{
					//If the number is badly entered, we communicate it to the user
					Console.WriteLine("number badly entered");
					continue;
				}
			}

			return 1;
		}

		//the function that read unlimited numbers (for sum and mult)
		public static void ReadNumbers(){
			//init the list of numbers in thew memory direction
			numbers = new List<int>();
			//number to introduce in the list
			int n;
			bool exit;
			do {
				//init the bool here because the user was introduce 1 number not exit in the last number
				exit = false;

				try{
					//ask the number to user
					Console.WriteLine("Tell me the number who do you wish operate");
					n = int.Parse(Console.ReadLine());
					
					//add the number at list
					numbers.Add(n);
				}catch(Exception e){
					//If the number is badly entered, we communicate it to the user
					Console.WriteLine("number badly entered");
					continue;
				}

				exit = ContinueValidate("¿Do you introduce another number? (yes/no)");
				
			} while ((numbers.Count < 2) || (!exit));
			
		}

		//chek if the user want's to continue or not
		public static bool ContinueValidate(string msg){
			bool validate = false;
			string cont;

			do{
				Console.WriteLine(msg);
				cont = Console.ReadLine();
			} while (!cont.ToLower().Equals("yes") && !cont.ToLower().Equals("no"));

			if (cont.Equals("no"))
				validate = true;

			return validate;
		}
	}
}
