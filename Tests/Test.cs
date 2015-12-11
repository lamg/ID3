using NUnit.Framework;
using NUnit.Core;
using System;
using ID3;

namespace Tests
{
	[TestFixture ()]
	public class Test
	{		
		string[][] init(out string[] atr){
			
			return A.CargarEjemplos ("ejemplo.txt", out atr);
		}

		[Test ()]
		public void TestMejorAtributo ()
		{
			string[] at;
			var e = init (out at);
			string[] vs;
			int a = A.MejorAtributo (e, at.Length-1, at.Length-1, out vs);
			Assert.AreEqual (a, 2);
		}

		[Test()]
		public void TestSub(){
			string[] atrs;
			var e = init (out atrs);
			var n = A.Sub (e, 2, "2");
			Assert.AreEqual (n.Length, 1);
		}

		[Test()]
		public void TestTree(){
			string[] a;
			var e = init (out a);
			var t = A.ID3 (e, a, a.Length-1);
			A.ImprimeArbol (t);
		}


	}
}

