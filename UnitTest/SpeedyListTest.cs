using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hikipuro.Collections;
using System.Collections.Generic;
using System.Text;

namespace UnitTest {
	[TestClass]
	public class SpeedyListTest {

		class TestUserClass /* : IEquatable<TestUserClass> */ {
			public string Name = string.Empty;
			public int Value = 0;
			public override int GetHashCode() {
				if (Name == null) {
					return 0;
				}
				int nameHash = Name.GetHashCode();
				int valueHash = Value.GetHashCode();
				return nameHash ^ valueHash;
			}
			/*
			public bool Equals(TestUserClass other) {
				if (Name != other.Name) {
					return false;
				}
				return Value == other.Value;
			}
			//*/
			/*
			public override bool Equals(object obj) {
				var other = obj as TestUserClass;
				if (other != null) {
					return false;
				}
				if (Name != other.Name) {
					return false;
				}
				return Value == other.Value;
			}
			//*/
			public override string ToString() {
				var text = new StringBuilder();
				text.Append("Name: ");
				text.Append(Name);
				text.Append(", Value: ");
				text.Append(Value);
				return text.ToString();
			}
		}

		/// <summary>
		/// コンストラクタの呼び出しテスト.
		/// </summary>
		[TestMethod, TestCategory("SpeedyList")]
		public void Constructor() {
			var list1 = new SpeedyList<int>();
			var list2 = new SpeedyList<string>();
			var list3 = new SpeedyList<TestUserClass>();

			var list4 = new SpeedyList<int>(10);

			var list = new List<int>();
			var list5 = new SpeedyList<int>(list);

			for (int i = 0; i < 100; i++) {
				list.Add(i);
			}
			list5 = new SpeedyList<int>(list);
			Assert.AreEqual(100, list5.Count);
		}

		/// <summary>
		/// int 型の Add() 呼び出しテスト.
		/// </summary>
		[TestMethod, TestCategory("SpeedyList")]
		public void AddInt() {
			const int LoopCount = 10000;
			var list = new List<int>();
			var speedyList = new SpeedyList<int>();

			for (int i = 0; i < LoopCount; i++) {
				list.Add(i);
				speedyList.Add(i);
			}
			Assert.AreEqual(LoopCount, list.Count);
			Assert.AreEqual(list.Count, speedyList.Count);

			for (int i = 0; i < LoopCount; i++) {
				Assert.AreEqual(list[i], speedyList[i]);
			}
		}

		/// <summary>
		/// string 型の Add() 呼び出しテスト.
		/// </summary>
		[TestMethod, TestCategory("SpeedyList")]
		public void AddString() {
			const int LoopCount = 10000;
			var list = new List<string>();
			var speedyList = new SpeedyList<string>();

			for (int i = 0; i < LoopCount; i++) {
				string value = i.ToString();
				list.Add(value);
				speedyList.Add(value);
			}
			Assert.AreEqual(LoopCount, list.Count);
			Assert.AreEqual(list.Count, speedyList.Count);

			for (int i = 0; i < LoopCount; i++) {
				Assert.AreEqual(list[i], speedyList[i]);
			}
		}

		/// <summary>
		/// ユーザ定義クラスの Add() 呼び出しテスト.
		/// </summary>
		[TestMethod, TestCategory("SpeedyList")]
		public void AddClass() {
			const int LoopCount = 10000;
			var list = new List<TestUserClass>();
			var speedyList = new SpeedyList<TestUserClass>();
			var random = new Random();

			for (int i = 0; i < LoopCount; i++) {
				var value = new TestUserClass();
				value.Name = i.ToString();
				value.Value = random.Next(LoopCount);
				list.Add(value);
				speedyList.Add(value);
			}
			Assert.AreEqual(LoopCount, list.Count);
			Assert.AreEqual(list.Count, speedyList.Count);

			for (int i = 0; i < LoopCount; i++) {
				Assert.AreEqual(list[i], speedyList[i]);
			}
		}

		/// <summary>
		/// Clear() 呼び出しテスト.
		/// </summary>
		[TestMethod, TestCategory("SpeedyList")]
		public void Clear() {
			const int LoopCount = 10000;
			var list = new List<int>();
			var speedyList = new SpeedyList<int>();

			for (int i = 0; i < LoopCount; i++) {
				list.Add(i);
				speedyList.Add(i);
			}
			list.Clear();
			speedyList.Clear();
			Assert.AreEqual(0, speedyList.Count);

			for (int i = 0; i < LoopCount; i++) {
				list.Add(i);
				speedyList.Add(i);
			}

			for (int i = 0; i < LoopCount; i++) {
				Assert.AreEqual(list[i], speedyList[i]);
			}
		}

		/// <summary>
		/// int 型の Contains() 呼び出しテスト.
		/// </summary>
		[TestMethod, TestCategory("SpeedyList")]
		public void ContainsInt() {
			const int LoopCount = 1000;
			var list = new List<int>();
			var speedyList = new SpeedyList<int>();
			var random = new Random();

			for (int i = 0; i < LoopCount; i++) {
				int value = random.Next(LoopCount);
				list.Insert(list.Count, value);
				speedyList.Insert(speedyList.Count, value);
			}
			Assert.AreEqual(LoopCount, list.Count);
			Assert.AreEqual(list.Count, speedyList.Count);

			for (int i = 0; i < LoopCount; i++) {
				Assert.AreEqual(list[i], speedyList[i]);
			}

			int total1 = 0;
			int total2 = 0;
			for (int i = 0; i < LoopCount; i++) {
				if (list.Contains(i)) {
					total1++;
				}
				if (speedyList.Contains(i)) {
					total2++;
				}
			}
			Assert.AreEqual(total1, total2);
		}

		/// <summary>
		/// string 型の Contains() 呼び出しテスト.
		/// </summary>
		[TestMethod, TestCategory("SpeedyList")]
		public void ContainsString() {
			const int LoopCount = 1000;
			var list = new List<string>();
			var speedyList = new SpeedyList<string>();
			var random = new Random();

			for (int i = 0; i < LoopCount; i++) {
				string value = random.Next(LoopCount).ToString();
				list.Insert(list.Count, value);
				speedyList.Insert(speedyList.Count, value);
			}
			Assert.AreEqual(LoopCount, list.Count);
			Assert.AreEqual(list.Count, speedyList.Count);

			for (int i = 0; i < LoopCount; i++) {
				Assert.AreEqual(list[i], speedyList[i]);
			}

			int total1 = 0;
			int total2 = 0;
			for (int i = 0; i < LoopCount; i++) {
				string value = i.ToString();
				if (list.Contains(value)) {
					total1++;
				}
				if (speedyList.Contains(value)) {
					total2++;
				}
			}
			Assert.AreEqual(total1, total2);
		}

		/// <summary>
		/// ユーザ定義クラスの Contains() 呼び出しテスト.
		/// </summary>
		[TestMethod, TestCategory("SpeedyList")]
		public void ContainsClass() {
			const int LoopCount = 1000;
			var list = new List<TestUserClass>();
			var speedyList = new SpeedyList<TestUserClass>();
			var random = new Random();

			for (int i = 0; i < LoopCount; i++) {
				var value = new TestUserClass();
				value.Name = i.ToString();
				value.Value = i;
				list.Insert(list.Count, value);
				speedyList.Insert(speedyList.Count, value);
			}
			Assert.AreEqual(LoopCount, list.Count);
			Assert.AreEqual(list.Count, speedyList.Count);

			for (int i = 0; i < LoopCount; i++) {
				Assert.AreEqual(list[i], speedyList[i]);
			}

			int total1 = 0;
			int total2 = 0;
			for (int i = 0; i < LoopCount; i++) {
				var value = new TestUserClass();
				value.Name = i.ToString();
				value.Value = i;
				if (list.Contains(value)) {
					total1++;
				}
				if (speedyList.Contains(value)) {
					total2++;
				}
			}
			Console.WriteLine("total: " + total1);

			/*
			for (int i = 0; i < speedyList.Count; i++) {
				Console.WriteLine(speedyList[i]);
			}
			//*/
			Assert.AreEqual(total1, total2);
		}

		/// <summary>
		/// CopyTo() 呼び出しテスト.
		/// </summary>
		[TestMethod, TestCategory("SpeedyList")]
		public void CopyTo() {
			const int LoopCount = 1000;
			var list = new List<int>();
			var speedyList = new SpeedyList<int>();
			var random = new Random();

			for (int i = 0; i < LoopCount; i++) {
				int index = random.Next(list.Count);
				int value = random.Next(LoopCount) + 1;
				list.Insert(index, value);
				speedyList.Insert(index, value);
			}
			Assert.AreEqual(LoopCount, list.Count);
			Assert.AreEqual(list.Count, speedyList.Count);

			var array1 = new int[LoopCount];
			var array2 = new int[LoopCount];
			list.CopyTo(array1, 0);
			speedyList.CopyTo(array2, 0);

			for (int i = 0; i < LoopCount; i++) {
				Assert.AreNotEqual(0, array1[i]);
				Assert.AreEqual(array1[i], array2[i]);
			}
		}

		/// <summary>
		/// IEnumerator のテスト.
		/// </summary>
		[TestMethod, TestCategory("SpeedyList")]
		public void Enumerator() {
			const int LoopCount = 1000;
			var list = new List<int>();
			var speedyList = new SpeedyList<int>();
			var random = new Random();

			for (int i = 0; i < LoopCount; i++) {
				int index = random.Next(list.Count);
				int value = random.Next(LoopCount) + 1;
				list.Insert(index, value);
				speedyList.Insert(index, value);
			}
			Assert.AreEqual(LoopCount, list.Count);
			Assert.AreEqual(list.Count, speedyList.Count);

			int total1 = 0;
			int total2 = 0;
			foreach (int i in list) {
				total1 += i;
			}
			foreach (int i in speedyList) {
				total2 += i;
			}
			Assert.AreNotEqual(0, total1);
			Assert.AreEqual(total1, total2);
		}

		/// <summary>
		/// IndexOf() 呼び出しテスト.
		/// </summary>
		[TestMethod, TestCategory("SpeedyList")]
		public void IndexOf() {
			const int LoopCount = 1000;
			var list = new List<int>();
			var speedyList = new SpeedyList<int>();
			var random = new Random();

			for (int i = 0; i < LoopCount; i++) {
				int index = random.Next(list.Count);
				int value = random.Next(LoopCount) + 1;
				list.Insert(index, value);
				speedyList.Insert(index, value);
			}
			Assert.AreEqual(LoopCount, list.Count);
			Assert.AreEqual(list.Count, speedyList.Count);

			for (int i = 0; i < LoopCount; i++) {
				int index1 = list.IndexOf(i);
				int index2 = speedyList.IndexOf(i);
				Assert.AreEqual(index1, index2);
			}
		}

		/// <summary>
		/// Insert() 呼び出しテスト.
		/// </summary>
		[TestMethod, TestCategory("SpeedyList")]
		public void Insert() {
			const int LoopCount = 1000;
			var list = new List<int>();
			var speedyList = new SpeedyList<int>();
			var random = new Random();

			for (int i = 0; i < LoopCount; i++) {
				int index = random.Next(list.Count);
				int value = random.Next(LoopCount) + 1;
				list.Insert(index, value);
				speedyList.Insert(index, value);
			}
			for (int i = 0; i < LoopCount; i++) {
				int index = random.Next(list.Count);
				int value = random.Next(LoopCount) + 1;
				list.Insert(index, value);
				speedyList.Insert(index, value);
			}
			Assert.AreEqual(LoopCount * 2, list.Count);
			Assert.AreEqual(list.Count, speedyList.Count);

			list.Clear();
			speedyList.Clear();
			for (int i = 0; i < LoopCount; i++) {
				int index = random.Next(list.Count);
				int value = random.Next(LoopCount) + 1;
				list.Insert(index, value);
				speedyList.Insert(index, value);
			}
			Assert.AreEqual(LoopCount, list.Count);
			Assert.AreEqual(list.Count, speedyList.Count);
		}

		/// <summary>
		/// Remove() 呼び出しテスト.
		/// </summary>
		[TestMethod, TestCategory("SpeedyList")]
		public void Remove() {
			const int TestCount = 10;
			const int LoopCount = 1248;
			var random = new Random();

			for (int n = 0; n < TestCount; n++) {
				var list = new List<int>();
				var speedyList = new SpeedyList<int>();

				for (int i = 0; i < LoopCount; i++) {
					int index = random.Next(list.Count);
					list.Insert(index, i);
					speedyList.Insert(index, i);
				}
				Assert.AreEqual(LoopCount, list.Count);
				Assert.AreEqual(list.Count, speedyList.Count);

				for (int i = 0; i < LoopCount / 2; i++) {
					list.Remove(i);
					speedyList.Remove(i);
				}
				Assert.AreEqual(LoopCount / 2, list.Count);
				Assert.AreEqual(list.Count, speedyList.Count);

				for (int i = 0; i < LoopCount / 2; i++) {
					Assert.AreEqual(list[i], speedyList[i]);
					int index1 = list.IndexOf(i);
					int index2 = speedyList.IndexOf(i);
					Assert.AreEqual(index1, index2);
				}
			}
		}

		/// <summary>
		/// RemoveAt() 呼び出しテスト.
		/// </summary>
		[TestMethod, TestCategory("SpeedyList")]
		public void RemoveAt() {
			const int TestCount = 10;
			const int LoopCount = 1248;
			var random = new Random();

			for (int n = 0; n < TestCount; n++) {
				var list = new List<int>();
				var speedyList = new SpeedyList<int>();

				for (int i = 0; i < LoopCount; i++) {
					int index = random.Next(list.Count);
					list.Insert(index, i);
					speedyList.Insert(index, i);
				}
				Assert.AreEqual(LoopCount, list.Count);
				Assert.AreEqual(list.Count, speedyList.Count);

				for (int i = 0; i < LoopCount / 2; i++) {
					int index = random.Next(list.Count);
					list.RemoveAt(index);
					speedyList.RemoveAt(index);
				}
				Assert.AreEqual(LoopCount / 2, list.Count);
				Assert.AreEqual(list.Count, speedyList.Count);

				for (int i = 0; i < LoopCount / 2; i++) {
					Assert.AreEqual(list[i], speedyList[i]);
					int index1 = list.IndexOf(i);
					int index2 = speedyList.IndexOf(i);
					Assert.AreEqual(index1, index2);
				}

				for (int i = 0; i < LoopCount / 2; i++) {
					int index = random.Next(list.Count);
					int index1 = list.IndexOf(index);
					int index2 = speedyList.IndexOf(index);
					Assert.AreEqual(index1, index2);
				}
			}
		}

		/// <summary>
		/// int 型の複雑な状況のテスト.
		/// </summary>
		[TestMethod, TestCategory("SpeedyList")]
		public void ComplexTestInt() {
			const int TestCount = 20;
			const int LoopCount = 999;
			var random = new Random();

			for (int n = 0; n < TestCount; n++) {
				var list = new List<int>();
				var speedyList = new SpeedyList<int>();

				for (int i = 0; i < 1000; i++) {
					list.Add(i);
					speedyList.Add(i);
				}
				Assert.AreNotEqual(0, list.Count);
				Assert.AreEqual(list.Count, speedyList.Count);

				for (int i = 0; i < LoopCount; i++) {
					int method = random.Next(4);
					int index = random.Next(list.Count);
					int value = random.Next(LoopCount);
					switch (method) {
					case 0:
						list.Add(value);
						speedyList.Add(value);
						break;
					case 1:
						list.Insert(index, value);
						speedyList.Insert(index, value);
						break;
					case 2:
						list.Remove(value);
						speedyList.Remove(value);
						break;
					case 3:
						list.RemoveAt(index);
						speedyList.RemoveAt(index);
						break;
					default:
						Assert.IsTrue(false);
						break;
					}
				}
				Assert.AreNotEqual(0, list.Count);
				Assert.AreEqual(list.Count, speedyList.Count);

				for (int i = 0; i < list.Count; i++) {
					Assert.AreEqual(list[i], speedyList[i]);
					int index1 = list.IndexOf(i);
					int index2 = speedyList.IndexOf(i);
					Assert.AreEqual(index1, index2);
				}
			}
		}

		/// <summary>
		/// string 型の複雑な状況のテスト.
		/// </summary>
		[TestMethod, TestCategory("SpeedyList")]
		public void ComplexTestString() {
			const int TestCount = 20;
			const int LoopCount = 999;
			var random = new Random();

			for (int n = 0; n < TestCount; n++) {
				var list = new List<string>();
				var speedyList = new SpeedyList<string>();

				for (int i = 0; i < 1000; i++) {
					var value = i.ToString();
					list.Add(value);
					speedyList.Add(value);
				}
				Assert.AreNotEqual(0, list.Count);
				Assert.AreEqual(list.Count, speedyList.Count);

				for (int i = 0; i < LoopCount; i++) {
					int method = random.Next(6);
					int index = random.Next(list.Count);
					var value = random.Next(LoopCount).ToString();
					switch (method) {
					case 0:
						list.Add(value);
						speedyList.Add(value);
						break;
					case 1:
						list.Insert(index, value);
						speedyList.Insert(index, value);
						break;
					case 2:
						list.Remove(value);
						speedyList.Remove(value);
						break;
					case 3:
						list.RemoveAt(index);
						speedyList.RemoveAt(index);
						break;
					case 4:
						list.Insert(index, null);
						speedyList.Insert(index, null);
						break;
					case 5:
						list.Remove(null);
						speedyList.Remove(null);
						break;
					default:
						Assert.IsTrue(false);
						break;
					}
				}
				Assert.AreNotEqual(0, list.Count);
				Assert.AreEqual(list.Count, speedyList.Count);

				for (int i = 0; i < list.Count; i++) {
					Assert.AreEqual(list[i], speedyList[i]);
					var value = i.ToString();
					int index1 = list.IndexOf(value);
					int index2 = speedyList.IndexOf(value);
					Assert.AreEqual(index1, index2);
				}
			}
		}

		/// <summary>
		/// ユーザ定義クラスの複雑な状況のテスト.
		/// </summary>
		[TestMethod, TestCategory("SpeedyList")]
		public void ComplexTestClass() {
			const int TestCount = 20;
			const int LoopCount = 999;
			var random = new Random();

			for (int n = 0; n < TestCount; n++) {
				var list = new List<TestUserClass>();
				var speedyList = new SpeedyList<TestUserClass>();

				for (int i = 0; i < 1000; i++) {
					var value = new TestUserClass();
					value.Name = i.ToString();
					value.Value = random.Next(LoopCount);
					list.Add(value);
					speedyList.Add(value);
				}
				Assert.AreNotEqual(0, list.Count);
				Assert.AreEqual(list.Count, speedyList.Count);

				for (int i = 0; i < LoopCount; i++) {
					int method = random.Next(6);
					int index = random.Next(list.Count);
					var value = new TestUserClass();
					value.Name = i.ToString();
					value.Value = random.Next(LoopCount);
					switch (method) {
					case 0:
						list.Add(value);
						speedyList.Add(value);
						break;
					case 1:
						list.Insert(index, value);
						speedyList.Insert(index, value);
						break;
					case 2:
						list.Remove(value);
						speedyList.Remove(value);
						break;
					case 3:
						list.RemoveAt(index);
						speedyList.RemoveAt(index);
						break;
					case 4:
						list.Insert(index, null);
						speedyList.Insert(index, null);
						break;
					case 5:
						list.Remove(null);
						speedyList.Remove(null);
						break;
					default:
						Assert.IsTrue(false);
						break;
					}
				}
				Assert.AreNotEqual(0, list.Count);
				Assert.AreEqual(list.Count, speedyList.Count);

				for (int i = 0; i < list.Count; i++) {
					Assert.AreEqual(list[i], speedyList[i]);
					var value = new TestUserClass();
					value.Name = i.ToString();
					value.Value = random.Next(LoopCount);
					int index1 = list.IndexOf(value);
					int index2 = speedyList.IndexOf(value);
					Assert.AreEqual(index1, index2);
				}
			}
		}
	}
}
