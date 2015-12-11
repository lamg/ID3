using System;
using System.Collections.Generic;
using D = System.Diagnostics.Debug;
using System.IO;

namespace ID3
{
	public class A
	{
		public static Tree<string> ID3(string[][] ejemplos, string[] atrs, int obj){
			Tree <string> raiz;
			if (TodosEj (ejemplos, obj, "+")) {
				raiz = new Tree<string> ("+", null);
			} else if (TodosEj (ejemplos, obj, "-")) {
				raiz = new Tree<string> ("-", null);
			} else if (atrs.Length == 0) {
				var r = Mayoria (ejemplos, obj);
				raiz = new Tree<string> (r, null);
			} else {
				string[] vs;
				int mejAtr = MejorAtributo (ejemplos, atrs.Length, obj, out vs);
				var hs = new Tree<string>[vs.Length];
				for (int i = 0; i != vs.Length; i++) {
					var ev = Sub (ejemplos, mejAtr, vs [i]);
					if (ev.Length == 0) {
						var m = Mayoria (ejemplos, obj);
						hs[i] = new Tree<string> (m, null);
					} else {
						var l = new List<string> (atrs);
						l.RemoveAt (mejAtr);
						hs[i] = ID3 (ev, l.ToArray(), obj-1);
					}
				}
				raiz = new Tree<string> (atrs [mejAtr], hs);
			}
			return raiz;
		}

		public static string[][] Sub(string[][] ejemplos, int atr, string v){
			var l = new List<string[]> ();
			for (int i = 0; i != ejemplos.Length; i++) {
				D.Assert (0 <= atr && atr < ejemplos [i].Length);
				if (ejemplos[i][atr] == v) {
					var n = new List<string> (ejemplos [i]);
					n.RemoveAt (atr);
					l.Add (n.ToArray());
				}
			}
			return l.ToArray();
		}

		static bool TodosEj(string[][] ejemplos, int obj, string atr){
			bool r = true;
			for (int i = 0; r && i != ejemplos.Length; i++) {
				r = ejemplos [i] [obj] == atr;
			}
			return r;
		}

		static int ContarAtrValObj(string[][] ejemplos, int obj, int atr, string v, string ve){
			int c = 0;
			for (int i = 0; i != ejemplos.Length; i++) {
				if (ejemplos[i][atr] == ve && ejemplos[i][obj] == v) {
					c++;
				}
			}
			return c;
		}

		static string Mayoria(string[][] ejemplos, int obj){
			string[] vals = new string[]{ "-", "+" };
			int[] cs = new int[vals.Length];
			int m = 0;
			for (int i = 0; i != ejemplos.Length; i++) {
				int j;
				bool e = false;
				for (j = 0; !e && j != vals.Length;) {
					e = vals [j] == ejemplos [i] [obj];
					if (!e) {
						j++;
					}
				}
				// {e=true}
				cs[j]++;
				// {cs tiene la cuenta de la cantidad de cada elemento de vals que
				// ha aparecido hasta el momento en la columna definida por el indice obj}
				if (cs[j] > cs[m]) {
					m = j;
				}
				// {m es el indice del valor de vals que mas aparece
				// en la columna de atributos objetivo}
			}
			return vals [m];
		}

		static double EntropiaVal(int p, int n, int t){
			int d = p + n;
			double a = (double)p/(double)d, b = (double)n/(double)d;
			double f = (double)d / (double)t;
			double r = f*(fact (a) + fact (b));
			return r;
		}

		static double fact(double x){
			D.Assert (x >= 0);
			double r;
			if (x == 0) {
				r = 0;
			} else {
				r = -x * Math.Log (x, 2);
			}
			return r;
		}

		static double EntropiaAtr(string[][] ejemplos, int atr, int obj, out string[] vs){
			vs = ValsAtr (ejemplos, atr);
			double g = 0;
			for (int i = 0; i != vs.Length; i++) {
				int p = ContarAtrValObj (ejemplos, obj, atr, "+", vs [i]);
				int n = ContarAtrValObj (ejemplos, obj, atr, "-", vs [i]);
				g += EntropiaVal (p, n,ejemplos.Length);
			}
			return g;
		}

		static string[] ValsAtr(string[][] ejemplos, int atr){
			var ls = new List<string> ();
			for (int i = 0; i != ejemplos.Length; i++) {
				if (!ls.Contains(ejemplos[i][atr])) {
					ls.Add (ejemplos [i] [atr]);
				}
			}
			return ls.ToArray ();
		}

		public static int MejorAtributo(string[][]ejemplos, int atrs, int obj, out string[] vs){
			int atr = -1;
			double m = double.MaxValue;
			vs = null;
			for (int i = 0; i != atrs; i++) {
				if (i != obj) {
					string[] nvs;
					double g = EntropiaAtr (ejemplos, i, obj, out nvs);
					if (g < m) {
						atr = i;
						m = g;
						vs = nvs;
					}	
				}
			}
			return atr;
		}

		public static void ImprimeArbol(Tree<string> t){
			Console.Write ("({0} ",t.Val);
			for (int i = 0; t.Hs != null && i != t.Hs.Length; i++) {
				ImprimeArbol (t.Hs [i]);
			}
			Console.Write(")");
		}

		public static string[][] CargarEjemplos(string filename, out string[] atrs){
			// {filename es el nombre de un archivo correctamente formateado}
			string[][] ej = null;
			atrs = null;
			if (File.Exists(filename)) {
				var f = File.OpenText (filename);
				int n;
				bool ok = int.TryParse(f.ReadLine (),out n);;
				if (ok && n >= 0) {
					atrs = f.ReadLine ().Split (' ');
					n--;
					ej = new string[n][];
					for (int i = 0; i != n; i++) {
						ej [i] = f.ReadLine ().Split (' ');
					}
				}
				f.Close ();
			}
			return ej;
		}
	}

	public class Tree<T>{
		T val;
		Tree<T>[] hs;

		public Tree (T val, Tree<T>[] hs)
		{
			this.hs = hs;
			this.val = val;
		}

		public T Val {
			get {
				return val;
			}
		}

		public Tree<T>[] Hs {
			get {
				return hs;
			}
		}
	}
}