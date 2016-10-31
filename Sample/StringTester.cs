using Hikipuro.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SpeedyList.Sample {
	class StringTester {
		public string Start() {
			List<string> list = new List<string>();
			for (int i = 0; i < 20000; i++) {
				list.Add((i / 1).ToString());
			}
			TestUtility.Shuffle(list);

			SpeedyList<string> speedyList = new SpeedyList<string>(list);

			StringBuilder text = new StringBuilder();
			text.AppendLine("[string test]");
			text.AppendLine();
			text.AppendLine("Generic List =====================");

			long time = 0;
			long start = 0;
			long end = 0;

			///*
			time = Benchmark.Start(() => {
				start = GC.GetTotalMemory(false);
				end = AddTest(new List<string>());
			}, 1);
			text.AppendLine("AddTest: " + time + " ms");

			time = Benchmark.Start(() => {
				InsertTest(new List<string>());
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
				AddTest(new SpeedyList<string>());
			}, 2);
			text.AppendLine("AddTest: " + time + " ms");

			time = Benchmark.Start(() => {
				//speedyList.Clear();
				//InsertTest(speedyList);
				InsertTest(new SpeedyList<string>());
			}, 2);
			text.AppendLine("InsertTest: " + time + " ms");
			//Shuffle(speedyList);

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

		public long AddTest(IList list) {
			Random random = new Random();
			int length = 100000;
			for (int i = 0; i < length; i++) {
				list.Add(random.Next(length).ToString());
			}
			return GC.GetTotalMemory(false);
		}

		public void InsertTest(IList list) {
			Random random = new Random();
			for (int i = 0; i < 20000; i++) {
				int index = random.Next(list.Count);
				list.Insert(index, i.ToString());
			}
		}

		public void IndexOfTest(IList list) {
			Random random = new Random();
			long total = 0;
			int length = list.Count;
			for (int i = 0; i < length; i++) {
				total += list.IndexOf(random.Next(length).ToString());
			}
			Console.WriteLine("IndexOfTest total: " + total);
		}

		public void ContainsTest(IList list) {
			long total = 0;
			for (int i = 0; i < list.Count; i++) {
				if (list.Contains(i.ToString())) {
					total++;
				}
			}
			Console.WriteLine("ContainsTest total: " + total);
		}

		public void RemoveTest(IList list) {
			Random random = new Random();
			int length = list.Count;
			for (int i = 0; i < length; i++) {
				list.Remove(i.ToString());
			}
			Console.WriteLine("list.count: " + list.Count);
		}

	}
}
