using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using NLog;

using Calculadora.Models;


namespace Calculadora
{
	public class Client
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private static StreamWriter sw = new StreamWriter("log.txt");
		private static List<int> _numbers;
		private static HttpClient client;
		private static string TrackId;
		private static HttpClientHandler ClientHandler = new HttpClientHandler();

		/// <summary>
		/// Call the api and return the result
		/// </summary>
		private async static Task<string> ApiCall(string url, Object o)
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
				return string.Empty;
			}
		}

		#region operations
		/// <summary>
		/// The add operation
		/// </summary>
		private static void Add()
		{
			var url = "http://localhost:5000/calculator/add";

			var add = new Adds
			{
				addens = _numbers
			};


			var request = ApiCall(url, add);
			var result = JsonSerializer.Deserialize<Sum>(request.Result.ToString());
			Console.WriteLine(result.sum);
		}

		/// <summary>
		/// The sub operation
		/// </summary>
		private static void Sub()
		{
			var url = "http://localhost:5000/calculator/sub";
			var sub = new Sub
			{
				minuend = _numbers[0],
				subtrahen = _numbers[1]
			};

			var request = ApiCall(url, sub);
			var result = JsonSerializer.Deserialize<Diference>(request.Result.ToString());
			Console.WriteLine(result.diference);
		}

		/// <summary>
		/// The multiplication operation
		/// </summary>
		private static void Mult()
		{
			var url = "http://localhost:5000/calculator/mult";

			var fact = new Factors
			{
				factors = _numbers
			};

			var request = ApiCall(url, fact);
			var result = JsonSerializer.Deserialize<Product>(request.Result.ToString());
			Console.WriteLine(result.product);
		}

		/// <summary>
		/// The divide operation
		/// </summary>
		private static void Div()
		{
			var url = "http://localhost:5000/calculator/div";

			var div = new Div
			{
				dividend = _numbers[0],
				divisor = _numbers[1]
			};

			var request = ApiCall(url, div);
			var result = JsonSerializer.Deserialize<DivResult>(request.Result.ToString());
			Console.WriteLine("quotient: " + result.quotient);
			Console.WriteLine("remainder: " + result.remainder);
		}

		/// <summary>
		/// The square operation
		/// </summary>
		private static void Square(int n)
		{
			var url = "http://localhost:5000/calculator/sqrt";
			var sqrt = new Sqrt
			{
				number = n
			};
			var request = ApiCall(url, sqrt);
			var result = JsonSerializer.Deserialize<Square>(request.Result.ToString());
			Console.WriteLine(result.square);

		}

		#endregion

		/// <summary>
		/// Chek if the user want's to continue or not
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		private static bool ContinueValidate(string msg)
		{
			var validate = false;
			string cont;

			do
			{
				Console.WriteLine(msg);
				cont = Console.ReadLine();
			} while (!cont.ToLower().Equals("yes") && !cont.ToLower().Equals("no"));

			if (cont.Equals("no"))
				validate = true;

			return validate;
		}

		#region readNumbers

		/// <summary>
		/// The function that read only one number (for square)
		/// </summary>
		/// <returns></returns>
		private static int ReadNumber()
		{

			//number to introduce in the list
			int n;
			bool exit = false;
			while (!exit)
			{
				try
				{
					//ask the number to user
					Console.WriteLine("Tell me the number who do you wish operate");
					n = int.Parse(Console.ReadLine());
					exit = true;

					return n;
				}
				catch (FormatException e)
				{

					sw.Write(e);
					sw.Flush();
					//If the number is badly entered, we communicate it to the user
					Console.WriteLine("number badly entered");

					continue;
				}
			}

			return 1;
		}

		/// <summary>
		/// The function that read only two numbers (for div and sub)
		/// </summary>
		private static void ReadOnlyTwoNumbers()
		{
			_numbers = new List<int>();
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
					_numbers.Add(n);
				}
				catch (FormatException e)
				{
					sw.Write(e);
					sw.Flush();
					//If the number is badly entered, we communicate it to the user
					Console.WriteLine("number badly entered");
					continue;
				}

			} while ((_numbers.Count < 2));
		}

		/// <summary>
		/// The function that read unlimited numbers (for sum and mult)
		/// </summary>
		private static void ReadNumbers()
		{
			//init the list of numbers in thew memory direction
			_numbers = new List<int>();
			//number to introduce in the list
			int n;
			bool exit;
			do
			{
				//init the bool here because the user was introduce 1 number not exit in the last number
				exit = false;

				try
				{
					//ask the number to user
					Console.WriteLine("Tell me the number who do you wish operate");
					n = int.Parse(Console.ReadLine());

					//add the number at list
					_numbers.Add(n);
				}
				catch (FormatException e)
				{
					sw.Write(e);
					sw.Flush();
					//If the number is badly entered, we communicate it to the user
					Console.WriteLine("number badly entered");
					continue;
				}

				exit = ContinueValidate("¿Do you introduce another number? (yes/no)");

			} while ((_numbers.Count < 2) || (!exit));

		}

		#endregion

		/// <summary>
		/// If the user have id see the operations whith this function
		/// </summary>
		private static void OperationsPerformed()
		{
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
					Console.WriteLine("Operation:" + o.operation);
					Console.WriteLine("Calculation:" + o.calculation);
					Console.WriteLine("Date:" + o.date);
				}
			}
			//else see the message error
			else
			{
				var result = JsonSerializer.Deserialize<List<Error>>(responseString);
				Console.WriteLine(response.RequestMessage);

			}

		}

		/// <summary>
		/// Is the menu of aplication
		/// </summary>
		private static void Action()
		{
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

		public static void Main(string[] args)
		{
			try
			{
				Logger.Info("Hello world");
				System.Console.ReadKey();
				
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Goodbye cruel world");
			}
			LogManager.Shutdown();
			ClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(ClientHandler);
			//ask the user for him id.
			Console.WriteLine("Tell me your id (if you don have, relax, can be empty)");
			TrackId = Console.ReadLine();
			TrackId = TrackId.Trim();

			//add the id to the header that JSON
			client.DefaultRequestHeaders.Add("id", TrackId);

			//the menu of the app
			Action();

			//if exist id, see the operations
			if (TrackId != "")
				OperationsPerformed();

			//close the log file
			sw.Close();
			Console.WriteLine("See you next time");
		}

	}
}