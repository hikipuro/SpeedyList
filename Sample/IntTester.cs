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
			TestUtility.Shuffle(list);

			SpeedyList<int> speedyList = new SpeedyList<int>(list);

			StringBuilder text = new StringBuilder();
			text.AppendLine("[int test]");
			text.AppendLine();
			text.AppendLine("Generic List =====================");

			long time = 0;
			long start = 0;
			long end = 0;

			///*
			time = Benchmark.Start(() => {
				start = GC.GetTotalMemory(true);
				end = AddTest(new List<int>());
			}, 1);
			text.Append(TestUtility.FormatResult("AddTest", time, end - start));

			time = Benchmark.Start(() => {
				start = GC.GetTotalMemory(true);
				end = InsertTest(new List<int>());
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
				end = AddTest(new SpeedyList<int>());
			}, 1);
			text.Append(TestUtility.FormatResult("AddTest", time, end - start));

			time = Benchmark.Start(() => {
				start = GC.GetTotalMemory(true);
				end = InsertTest(new SpeedyList<int>());
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
			Random random = new Random((int)DateTime.Now.Ticks);
			int length = 100000;
			for (int i = 0; i < length; i++) {
				list.Add(random.Next(length));
				//list.Add(i);
			}
			long memory = GC.GetTotalMemory(true);
			GC.KeepAlive(list);
			return memory;
		}

		public long InsertTest(IList list) {
			Random random = new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < 20000; i++) {
				int index = random.Next(list.Count);
				list.Insert(index, i);
			}
			long memory = GC.GetTotalMemory(true);
			GC.KeepAlive(list);
			return memory;
		}

		public long IndexOfTest(IList list) {
			long total = 0;
			int length = list.Count;
			for (int i = 0; i < length; i++) {
				total += list.IndexOf(i);
			}
			Console.WriteLine("IndexOfTest total: " + total);
			long memory = GC.GetTotalMemory(true);
			GC.KeepAlive(list);
			return memory;
		}

		public long ContainsTest(IList list) {
			long total = 0;
			for (int i = 0; i < list.Count; i++) {
				if (list.Contains(i)) {
					total++;
				}
			}
			Console.WriteLine("ContainsTest total: " + total);
			long memory = GC.GetTotalMemory(true);
			GC.KeepAlive(list);
			return memory;
		}

		public long RemoveTest(IList list) {
			//Random random = new Random((int)DateTime.Now.Ticks);
			int length = list.Count;
			for (int i = 0; i < length; i++) {
				list.Remove(i);
			}
			Console.WriteLine("list.count: " + list.Count);
			long memory = GC.GetTotalMemory(true);
			GC.KeepAlive(list);
			return memory;
		}
	}
}
