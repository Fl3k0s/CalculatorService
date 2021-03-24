using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;


namespace Calculadora
{
	public class Client
	{
		private static List<int> numbers;
		private static readonly HttpClient client = new HttpClient();
		private static string trackId;
		public static void Main(string[] args)
		{
			//ask the user for him id
			Console.WriteLine("Tell me your id (if you don have, relax, can be empty)");
			trackId = Console.ReadLine();

			//the menu of the app
			Action();
		}


		public static void Action(){
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

				//Leemos los numeros con los que quiere operar el usuario
				

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
						Square();
						break;
					case "exit":
						Console.WriteLine("Good bye");
						break;
					default:
						//si el usuario escribe un comando que no corresponde o escribe un comando mal se le informara
						Console.WriteLine("comand not exist, try again");
						break;
				}
			} while (!command.Equals("exit"));
		}

		public static void Add()
		{
			string url = $"http://localhost:8080/calculator/add";


		}

		public static void Sub(){
			string url = $"http://localhost:8080/calculator/sub";
			
		}

		public static void Mult(){
			string url = $"http://localhost:8080/calculator/mult";
		}

		public static void Div(){
			string url = $"http://localhost:8080/calculator/div";
		}

		public static void Square(){
			string url = $"http://localhost:8080/calculator/pow";
		}

		

		
		/*public static HttpWebRequest RequestAPI(string url){
			var request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "POST";
			request.ContentType = "application/json";
			request.Accept = "application/json";
			
			return request;
		}*/

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
