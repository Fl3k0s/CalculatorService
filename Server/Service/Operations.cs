using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Service
{
	public class Operations : IOperations
	{
		public int Add(List<int> add)
		{
			int sum = 0;

			foreach (int n in add)
				sum += n;

			return sum;
		}

		public int[] Div(int[] divs)
		{
			int[] numbers = new int[2];
			numbers[0] = (int)(divs[0] / divs[1]);
			numbers[1] = divs[0] % divs[1];
			return numbers ;
		}



		public int Mult(List<int> mults)
		{
			int mult = 0;

			foreach (int n in mults)
				mult *= n;

			return mult;
		}

		public int Sqrt(int sqrt)
		{
			return (int)Math.Sqrt(sqrt) ;
		}

		public int Sub(int[] subs)
		{
			return subs[0] - subs[1];
		}
	}
}
