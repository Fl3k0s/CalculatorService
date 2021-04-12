using System.Collections.Generic;


namespace Server.Service
{
	public interface IOperations
	{
		int Add(List<int> add);
		int Sub(int[] subs);
		int Mult(List<int> mults);
		int[] Div(int[] divs);
		int Sqrt(int sqrt);
	}
}
