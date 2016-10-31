using Hikipuro.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SpeedyList.Sample {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		class TestClass : IComparable<TestClass> {
			public int Index = 0;
			public int InsertedAt = 0;

			public TestClass(int index, int insertedAt = 0) {
				Index = index;
				InsertedAt = insertedAt;
			}

			int IComparable<TestClass>.CompareTo(TestClass other) {
				return Index - other.Index;
			}
		}

		private void Test() {
			List<TestClass> list = new List<TestClass>();
			for (int i = 0; i < 100; i++) {
				list.Add(new TestClass(i));
			}
			Shuffle(list);
			list.Sort();
			for (int i = 0; i < 100; i++) {
				Console.WriteLine(list[i].Index);
			}
		}

		private void TestInt() {
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
				//speedyList.Clear();
				//InsertTest(speedyList);
				InsertTest(new SpeedyList<int>());
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

			textBox.Text = text.ToString();
		}

		private void TestString() {
			List<string> list = new List<string>();
			for (int i = 0; i < 20000; i++) {
				list.Add((i / 1).ToString());
			}
			Shuffle(list);

			SpeedyList<string> speedyList = new SpeedyList<string>(list);

			StringBuilder text = new StringBuilder();
			text.AppendLine("[string test]");
			text.AppendLine();
			text.AppendLine("Generic List =====================");

			long time = 0;

			///*
			time = Benchmark.Start(() => {
				AddTestString(new List<string>());
			}, 2);
			text.AppendLine("AddTest: " + time + " ms");

			time = Benchmark.Start(() => {
				InsertTestString(new List<string>());
			}, 2);
			text.AppendLine("InsertTest: " + time + " ms");

			time = Benchmark.Start(() => {
				IndexOfTestString(list);
			}, 2);
			text.AppendLine("IndexOfTest: " + time + " ms");

			time = Benchmark.Start(() => {
				ContainsTestString(list);
			}, 2);
			text.AppendLine("ContainsTest: " + time + " ms");

			time = Benchmark.Start(() => {
				RemoveTestString(list);
			}, 1);
			text.AppendLine("RemoveTest: " + time + " ms");
			//*/


			text.AppendLine();
			text.AppendLine("SpeedyList =======================");

			time = Benchmark.Start(() => {
				AddTestString(new SpeedyList<string>());
			}, 2);
			text.AppendLine("AddTest: " + time + " ms");

			time = Benchmark.Start(() => {
				//speedyList.Clear();
				//InsertTest(speedyList);
				InsertTestString(new SpeedyList<string>());
			}, 2);
			text.AppendLine("InsertTest: " + time + " ms");
			//Shuffle(speedyList);

			time = Benchmark.Start(() => {
				IndexOfTestString(speedyList);
			}, 2);
			text.AppendLine("IndexOfTest: " + time + " ms");

			time = Benchmark.Start(() => {
				ContainsTestString(speedyList);
			}, 2);
			text.AppendLine("ContainsTest: " + time + " ms");

			time = Benchmark.Start(() => {
				RemoveTestString(speedyList);
			}, 1);
			text.AppendLine("RemoveTest: " + time + " ms");

			textBox.Text = text.ToString();
		}

		private void buttonTest_Click(object sender, EventArgs e) {
			//Test();
			//return;
			TestInt();
			//TestString();
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

		private void AddTestString(IList list) {
			Random random = new Random();
			int length = 100000;
			for (int i = 0; i < length; i++) {
				list.Add(random.Next(length).ToString());
			}
		}

		private void InsertTestString(IList list) {
			Random random = new Random();
			for (int i = 0; i < 20000; i++) {
				int index = random.Next(list.Count);
				list.Insert(index, i.ToString());
				//Console.WriteLine(index);
			}
		}

		private void IndexOfTestString(IList list) {
			Random random = new Random();
			long total = 0;
			int length = list.Count;
			for (int i = 0; i < length; i++) {
				total += list.IndexOf(random.Next(length).ToString());
			}
			Console.WriteLine("IndexOfTest total: " + total);
		}

		private void ContainsTestString(IList list) {
			long total = 0;
			for (int i = 0; i < list.Count; i++) {
				if (list.Contains(i.ToString())) {
					total++;
				}
			}
			Console.WriteLine("ContainsTest total: " + total);
		}

		private void RemoveTestString(IList list) {
			Random random = new Random();
			int length = list.Count;
			for (int i = 0; i < length; i++) {
				//Console.WriteLine(i);
				list.Remove(i.ToString());
				if (i % 7 == 0) {
					list.Insert(random.Next(list.Count), i.ToString());
				}
			}
			Console.WriteLine("list.count: " + list.Count);
		}

		private void AddTest(IList list) {
			Random random = new Random((int)DateTime.Now.Ticks);
			int length = 100000;
			for (int i = 0; i < length; i++) {
				list.Add(random.Next(length));
			}
		}

		private void InsertTest(IList list) {
			Random random = new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < 20000; i++) {
				int index = random.Next(list.Count);
				list.Insert(index, i);
				//Console.WriteLine(index);
			}
		}

		private void IndexOfTest(IList list) {
			//Thread.Sleep(100);
			Console.WriteLine((int)DateTime.Now.Ticks);
			Random random = new Random((int)DateTime.Now.Ticks);
			long total = 0;
			int length = list.Count;
			for (int i = 0; i < length; i++) {
				total += list.IndexOf(random.Next(length));
			}
			Console.WriteLine("IndexOfTest total: " + total);
		}

		private void ContainsTest(IList list) {
			long total = 0;
			for (int i = 0; i < list.Count; i++) {
				if (list.Contains(i)) {
					total++;
				}
			}
			Console.WriteLine("ContainsTest total: " + total);
		}

		private void RemoveTest(IList list) {
			Random random = new Random((int)DateTime.Now.Ticks);
			int length = list.Count;
			for (int i = 0; i < length; i++) {
				//Console.WriteLine(i);
				list.Remove(i);
				if (i % 7 == 0) {
					list.Insert(random.Next(list.Count), i);
				}
			}
			Console.WriteLine("list.count: " + list.Count);
		}
	}
}
