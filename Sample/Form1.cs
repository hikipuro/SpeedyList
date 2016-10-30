using Hikipuro.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace SpeedyList {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void buttonTest_Click(object sender, EventArgs e) {
			List<int> list = new List<int>();
			for (int i = 0; i < 20000; i++) {
				list.Add(i);
			}
			Shuffle(list);

			SpeedyList<int> speedyList = new SpeedyList<int>(list);

			long time = Benchmark((i) => {
				IndexOfTest(list);
			}, 2);
			Console.WriteLine("IndexOfTest: " + time + " ms");

			time = Benchmark((i) => {
				IndexOfTest(speedyList);
			}, 2);
			Console.WriteLine("IndexOfTest: " + time + " ms");

			time = Benchmark((i) => {
				ContainsTest(list);
			}, 2);
			Console.WriteLine("ContainsTest: " + time + " ms");

			time = Benchmark((i) => {
				ContainsTest(speedyList);
			}, 2);
			Console.WriteLine("ContainsTest: " + time + " ms");
		}

		public static void Shuffle<T>(IList<T> list) {
			Random random = new Random();
			int n = list.Count;
			while (n > 1) {
				n--;
				int k = random.Next(n + 1);
				T tmp = list[k];
				list[k] = list[n];
				list[n] = tmp;
			}
		}

		private void IndexOfTest(IList list) {
			long total = 0;
			for (int i = 0; i < list.Count; i++) {
				total += list.IndexOf(i);
			}
			Console.WriteLine("total: " + total);
		}

		private void ContainsTest(IList list) {
			long total = 0;
			for (int i = 0; i < list.Count; i++) {
				if (list.Contains(i)) {
					total++;
				}
			}
			Console.WriteLine("total: " + total);
		}

		/// <summary>
		/// 実行時間を計測する.
		/// </summary>
		/// <param name="act"></param>
		/// <param name="iterations"></param>
		/// <returns></returns>
		private static long Benchmark(Action<int> act, int iterations) {
			if (iterations <= 0) {
				return 0;
			}
			GC.Collect();
			act.Invoke(1); // run once outside of loop to avoid initialization costs
			Stopwatch sw = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++) {
				act.Invoke(1);
			}
			sw.Stop();
			return sw.ElapsedMilliseconds / iterations;
		}
	}
}
