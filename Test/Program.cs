using System;
using ID3;

namespace Test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string[] a;
			var e = A.CargarEjemplos ("ejemplo.txt", out a);
			var t = A.ID3 (e, a, a.Length-1);
			A.ImprimeArbol (t);
		}
	}
}
