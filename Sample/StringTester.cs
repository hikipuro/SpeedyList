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
				start = GC.GetTotalMemory(true);
				end = AddTest(new List<string>());
			}, 1);
			text.Append(TestUtility.FormatResult("AddTest", time, end - start));

			time = Benchmark.Start(() => {
				start = GC.GetTotalMemory(true);
				end = InsertTest(new List<string>());
			}, 1);
			text.Append(TestUtility.FormatResult("InsertTest", time, end - start));

			time = Benchmark.Start(() => {
				start = GC.GetTotalMemory(true);
				end = IndexOfTest(list);
			}, 1);
			text.Append(TestUtility.FormatResult("IndexOfTest", time, end - start));

			time = Benchmark.Start(() => {
				start = GC.GetTotalMemory(true);
				end = ContainsTest(list);
			}, 1);
			text.Append(TestUtility.FormatResult("ContainsTest", time, end - start));

			time = Benchmark.Start(() => {
				start = GC.GetTotalMemory(true);
				end = RemoveTest(list);
			}, 1);
			text.Append(TestUtility.FormatResult("RemoveTest", time, end - start));
			//*/


			text.AppendLine();
			text.AppendLine("SpeedyList =======================");

			time = Benchmark.Start(() => {
				start = GC.GetTotalMemory(true);
				end = AddTest(new SpeedyList<string>());
			}, 1);
			text.Append(TestUtility.FormatResult("AddTest", time, end - start));

			time = Benchmark.Start(() => {
				start = GC.GetTotalMemory(true);
				end = InsertTest(new SpeedyList<string>());
			}, 1);
			text.Append(TestUtility.FormatResult("InsertTest", time, end - start));

			time = Benchmark.Start(() => {
				start = GC.GetTotalMemory(true);
				end = IndexOfTest(speedyList);
			}, 1);
			text.Append(TestUtility.FormatResult("IndexOfTest", time, end - start));

			time = Benchmark.Start(() => {
				start = GC.GetTotalMemory(true);
				end = ContainsTest(speedyList);
			}, 1);
			text.Append(TestUtility.FormatResult("ContainsTest", time, end - start));

			time = Benchmark.Start(() => {
				start = GC.GetTotalMemory(true);
				end = RemoveTest(speedyList);
			}, 1);
			text.Append(TestUtility.FormatResult("RemoveTest", time, end - start));

			return text.ToString();
		}

		public long AddTest(IList list) {
			Random random = new Random();
			int length = 100000;
			for (int i = 0; i < length; i++) {
				list.Add(random.Next(length).ToString());
			}
			long memory = GC.GetTotalMemory(true);
			GC.KeepAlive(list);
			return memory;
		}

		public long InsertTest(IList list) {
			Random random = new Random();
			for (int i = 0; i < 20000; i++) {
				int index = random.Next(list.Count);
				list.Insert(index, i.ToString());
			}
			long memory = GC.GetTotalMemory(true);
			GC.KeepAlive(list);
			return memory;
		}

		public long IndexOfTest(IList list) {
			Random random = new Random();
			long total = 0;
			int length = list.Count;
			for (int i = 0; i < length; i++) {
				total += list.IndexOf(random.Next(length).ToString());
			}
			Console.WriteLine("IndexOfTest total: " + total);
			long memory = GC.GetTotalMemory(true);
			GC.KeepAlive(list);
			return memory;
		}

		public long ContainsTest(IList list) {
			long total = 0;
			for (int i = 0; i < list.Count; i++) {
				if (list.Contains(i.ToString())) {
					total++;
				}
			}
			Console.WriteLine("ContainsTest total: " + total);
			long memory = GC.GetTotalMemory(true);
			GC.KeepAlive(list);
			return memory;
		}

		public long RemoveTest(IList list) {
			Random random = new Random();
			int length = list.Count;
			for (int i = 0; i < length; i++) {
				list.Remove(i.ToString());
			}
			Console.WriteLine("list.count: " + list.Count);
			long memory = GC.GetTotalMemory(true);
			GC.KeepAlive(list);
			return memory;
		}

	}
}
