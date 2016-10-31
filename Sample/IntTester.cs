using Hikipuro.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SpeedyList.Sample {
	class IntTester {
		public string Start() {
			List<int> list = new List<int>();
			for (int i = 0; i < 20000; i++) {
				list.Add(i / 1);
			}
			Shuffle(list);

			SpeedyList<int> speedyList = new SpeedyList<int>(list);

			StringBuilder text = new StringBuilder();
			text.AppendLine("[int test]");
			text.AppendLine();
			text.AppendLine("Generic List =====================");

			long time = 0;

			///*
			time = Benchmark.Start(() => {
				AddTest(new List<int>());
			}, 2);
			text.AppendLine("AddTest: " + time + " ms");

			time = Benchmark.Start(() => {
				InsertTest(new List<int>());
			}, 2);
			text.AppendLine("InsertTest: " + time + " ms");

			time = Benchmark.Start(() => {
				IndexOfTest(list);
			}, 2);
			text.AppendLine("IndexOfTest: " + time + " ms");

			time = Benchmark.Start(() => {
				ContainsTest(list);
			}, 2);
			text.AppendLine("ContainsTest: " + time + " ms");

			time = Benchmark.Start(() => {
				RemoveTest(list);
			}, 1);
			text.AppendLine("RemoveTest: " + time + " ms");
			//*/


			text.AppendLine();
			text.AppendLine("SpeedyList =======================");

			time = Benchmark.Start(() => {
				AddTest(new SpeedyList<int>());
			}, 2);
			text.AppendLine("AddTest: " + time + " ms");

			time = Benchmark.Start(() => {
				InsertTest(new SpeedyList<int>());
			}, 2);
			text.AppendLine("InsertTest: " + time + " ms");

			time = Benchmark.Start(() => {
				IndexOfTest(speedyList);
			}, 2);
			text.AppendLine("IndexOfTest: " + time + " ms");

			time = Benchmark.Start(() => {
				ContainsTest(speedyList);
			}, 2);
			text.AppendLine("ContainsTest: " + time + " ms");

			time = Benchmark.Start(() => {
				RemoveTest(speedyList);
			}, 1);
			text.AppendLine("RemoveTest: " + time + " ms");

			return text.ToString();
		}

		public void Shuffle<T>(IList<T> list) {
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

		public void AddTest(IList list) {
			Random random = new Random((int)DateTime.Now.Ticks);
			int length = 100000;
			for (int i = 0; i < length; i++) {
				list.Add(random.Next(length));
			}
		}

		public void InsertTest(IList list) {
			Random random = new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < 20000; i++) {
				int index = random.Next(list.Count);
				list.Insert(index, i);
			}
		}

		public void IndexOfTest(IList list) {
			long total = 0;
			int length = list.Count;
			for (int i = 0; i < length; i++) {
				total += list.IndexOf(i);
			}
			Console.WriteLine("IndexOfTest total: " + total);
		}

		public void ContainsTest(IList list) {
			long total = 0;
			for (int i = 0; i < list.Count; i++) {
				if (list.Contains(i)) {
					total++;
				}
			}
			Console.WriteLine("ContainsTest total: " + total);
		}

		public void RemoveTest(IList list) {
			Random random = new Random((int)DateTime.Now.Ticks);
			int length = list.Count;
			for (int i = 0; i < length; i++) {
				list.Remove(i);
			}
			Console.WriteLine("list.count: " + list.Count);
		}
	}
}
