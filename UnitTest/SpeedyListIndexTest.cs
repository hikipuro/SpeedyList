using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hikipuro.Collections;
using System.Collections.Generic;

namespace UnitTest {
	using StringIndex = Dictionary<string, List<int>>;
	using IntIndex = Dictionary<int, List<int>>;

	[TestClass]
	public class SpeedyListIndexTest {

		class TestUserClass {
			public string Name = string.Empty;
			public int Value = 0;
		}

		[TestMethod, TestCategory("SpeedyListIndex")]
		public void Constructor() {
			var list1 = new List<int>();
			var index1 = new SpeedyListIndex<int>(list1);

			var list2 = new List<float>();
			var index2 = new SpeedyListIndex<float>(list2);

			var list3 = new List<string>();
			var index3 = new SpeedyListIndex<string>(list3);

			var list4 = new List<TestUserClass>();
			var index4 = new SpeedyListIndex<TestUserClass>(list4);

			bool catched = false;
			try {
				var index5 = new SpeedyListIndex<int>(null);
			} catch (ArgumentNullException) {
				catched = true;
			}
			Assert.IsTrue(catched);

			//List<float> list6 = new List<float>();
			//SpeedyListIndex<int> index6 = new SpeedyListIndex<int>(list2);

			var list7 = new List<int>();
			for (int i = 0; i < 10000; i++) {
				list7.Add(i);
			}
			var index7 = new SpeedyListIndex<int>(list7);
			Assert.IsFalse(index7.Contains(1));
			index7.Refresh();
			Assert.IsTrue(index7.Contains(1));
		}

		[TestMethod, TestCategory("SpeedyListIndex")]
		public void Add() {
			var list1 = new List<string>();
			var index1 = new SpeedyListIndex<string>(list1);
			var indices = TestUtility.GetPrivateField<StringIndex>(index1, "indices");

			index1.Add("test 1", 0);
			Assert.AreEqual(1, indices.Count);

			index1.Add(null, 10);
			Assert.AreEqual(1, indices.Count);

			index1.Add("test 2", 1);
			Assert.AreEqual(2, indices.Count);

			index1.Add("test 1", 2);
			Assert.AreEqual(2, indices.Count);
			Assert.AreEqual(2, indices["test 1"].Count);
			Assert.AreEqual(0, indices["test 1"][0]);
			Assert.AreEqual(2, indices["test 1"][1]);

			int count = 0;
			foreach (string key in indices.Keys) {
				switch (count) {
				case 0:
					Assert.AreEqual("test 1", key);
					break;
				case 1:
					Assert.AreEqual("test 2", key);
					break;
				}
				count++;
			}
			Assert.AreEqual(2, count);

			bool catched = false;
			try {
				var test = indices["test 1"][2];
			} catch (ArgumentOutOfRangeException) {
				catched = true;
			}
			Assert.IsTrue(catched);

			catched = false;
			try {
				var test = indices["test 1"][-1];
			} catch (ArgumentOutOfRangeException) {
				catched = true;
			}
			Assert.IsTrue(catched);

			index1.Clear();
			Assert.AreEqual(0, indices.Count);
		}

		[TestMethod, TestCategory("SpeedyListIndex")]
		public void Refresh() {
			var list1 = new List<string>();
			var index1 = new SpeedyListIndex<string>(list1);
			var indices = TestUtility.GetPrivateField<StringIndex>(index1, "indices");
			var nullIndex = TestUtility.GetPrivateField<List<int>>(index1, "nullIndex");

			Assert.AreEqual(0, indices.Count);
			Assert.AreEqual(0, nullIndex.Count);

			list1.Add("test 1");
			list1.Add("test 2");
			list1.Add("test 3");
			list1.Add(null);
			list1.Add("test 2");
			list1.Add("test 2");
			list1.Add(null);
			Assert.AreEqual(0, indices.Count);
			Assert.AreEqual(0, nullIndex.Count);

			index1.Refresh();
			Assert.AreEqual(3, indices.Count);
			Assert.AreEqual(2, nullIndex.Count);

			index1.Clear();
			Assert.AreEqual(0, indices.Count);
			Assert.AreEqual(0, nullIndex.Count);

			index1.Refresh();
			Assert.AreEqual(3, indices.Count);
			Assert.AreEqual(2, nullIndex.Count);

			bool catched = false;
			try {
				var test = indices["test 0"];
			} catch (KeyNotFoundException) {
				catched = true;
			}
			Assert.IsTrue(catched);

			Assert.AreEqual(1, indices["test 1"].Count);
			Assert.AreEqual(3, indices["test 2"].Count);
			Assert.AreEqual(1, indices["test 3"].Count);

			Assert.AreEqual(1, indices["test 2"][0]);
			Assert.AreEqual(4, indices["test 2"][1]);
			Assert.AreEqual(5, indices["test 2"][2]);

			Assert.AreEqual(3, nullIndex[0]);
			Assert.AreEqual(6, nullIndex[1]);

			var list2 = new List<int>();
			var index2 = new SpeedyListIndex<int>(list2);
			var indices2 = TestUtility.GetPrivateField<IntIndex>(index2, "indices");
			var nullIndex2 = TestUtility.GetPrivateField<List<int>>(index2, "nullIndex");
			list2.Add(1);
			list2.Add(2);
			list2.Add(3);
			index2.Refresh();
			Assert.AreEqual(3, indices2.Count);
			Assert.AreEqual(0, nullIndex2.Count);
		}

		[TestMethod, TestCategory("SpeedyListIndex")]
		public void Contains() {
			var list1 = new List<string>();
			var index1 = new SpeedyListIndex<string>(list1);
			var indices = TestUtility.GetPrivateField<StringIndex>(index1, "indices");
			var nullIndex = TestUtility.GetPrivateField<List<int>>(index1, "nullIndex");

			Assert.IsFalse(index1.Contains("test 1"));
			Assert.IsFalse(index1.Contains(null));

			index1.Add("test 1", 0);
			Assert.IsTrue(index1.Contains("test 1"));

			index1.Remove("test 1");
			Assert.IsFalse(index1.Contains("test 1"));

			index1.Remove("test 1");
			Assert.IsFalse(index1.Contains("test 1"));

			index1.Add("test 2", 1);
			Assert.IsFalse(index1.Contains("test 1"));
			Assert.IsTrue(index1.Contains("test 2"));

			index1.Add(null, 2);
			Assert.IsTrue(index1.Contains(null));

			index1.Add(null, 3);
			Assert.IsTrue(index1.Contains(null));

			index1.Remove(null);
			Assert.IsTrue(index1.Contains(null));

			index1.Remove(null);
			Assert.IsFalse(index1.Contains(null));

			index1.Remove(null);
			Assert.IsFalse(index1.Contains(null));
		}

		[TestMethod, TestCategory("SpeedyListIndex")]
		public void IndexOf() {
			var list1 = new List<string>();
			var index1 = new SpeedyListIndex<string>(list1);
			var indices = TestUtility.GetPrivateField<StringIndex>(index1, "indices");

			index1.Add("test 1", 1);
			index1.Add("test 1", 3);
			index1.Add("test 1", 5);
			index1.Add("test 2", 2);
			index1.Add("test 3", 4);
			index1.Add(null, 6);
			Assert.AreEqual(1, index1.IndexOf("test 1"));

			index1.Remove("test 1");
			Assert.IsTrue(index1.Contains("test 1"));
			Assert.AreEqual(3, index1.IndexOf("test 1"));

			index1.Remove("test 1");
			Assert.IsTrue(index1.Contains("test 1"));
			Assert.AreEqual(5, index1.IndexOf("test 1"));

			index1.Remove("test 1");
			Assert.IsFalse(index1.Contains("test 1"));
			Assert.AreEqual(-1, index1.IndexOf("test 1"));

			index1.Remove("test 1");
			Assert.IsFalse(index1.Contains("test 1"));
			Assert.AreEqual(-1, index1.IndexOf("test 1"));

			Assert.IsTrue(index1.Contains("test 2"));
			Assert.IsTrue(index1.Contains("test 3"));
			Assert.AreEqual(2, index1.IndexOf("test 2"));
			Assert.AreEqual(4, index1.IndexOf("test 3"));
			Assert.AreEqual(-1, index1.IndexOf("test 4"));

			Assert.AreEqual(6, index1.IndexOf(null));
			index1.Remove(null);
			Assert.AreEqual(-1, index1.IndexOf(null));
			index1.Remove(null);
			Assert.AreEqual(-1, index1.IndexOf(null));
		}

		[TestMethod, TestCategory("SpeedyListIndex")]
		public void Insert() {
			var list1 = new List<string>();
			var index1 = new SpeedyListIndex<string>(list1);
			var indices = TestUtility.GetPrivateField<StringIndex>(index1, "indices");
			var nullIndex = TestUtility.GetPrivateField<List<int>>(index1, "nullIndex");

			bool catched = false;
			try {
				index1.Insert(1, "test 1");
			} catch (IndexOutOfRangeException) {
				catched = true;
			}
			Assert.IsTrue(catched);

			catched = false;
			try {
				index1.Insert(-1, "test 1");
			} catch (IndexOutOfRangeException) {
				catched = true;
			}
			Assert.IsTrue(catched);

			list1.Insert(0, "test 1");
			index1.Insert(0, "test 1");

			list1.Insert(0, "test 2");
			index1.Insert(0, "test 2");

			list1.Insert(0, "test 3");
			index1.Insert(0, "test 3");

			list1.Insert(0, null);
			index1.Insert(0, null);

			list1.Insert(1, "test 1");
			index1.Insert(1, "test 1");

			list1.Insert(1, "test 2");
			index1.Insert(1, "test 2");

			Assert.AreEqual(3, indices.Count);
			Assert.AreEqual(1, nullIndex.Count);

			Assert.AreEqual(0, index1.IndexOf(null));
			Assert.AreEqual(2, index1.IndexOf("test 1"));
			Assert.AreEqual(3, index1.IndexOf("test 3"));
			Assert.AreEqual(1, index1.IndexOf("test 2"));

			Assert.AreEqual(2, indices["test 1"].Count);
			Assert.AreEqual(2, indices["test 1"][0]);
			Assert.AreEqual(5, indices["test 1"][1]);

			Assert.AreEqual(2, indices["test 2"].Count);
			Assert.AreEqual(1, indices["test 2"][0]);
			Assert.AreEqual(4, indices["test 2"][1]);

			Assert.AreEqual(1, indices["test 3"].Count);
			Assert.AreEqual(3, indices["test 3"][0]);

			Assert.AreEqual(1, nullIndex.Count);
			Assert.AreEqual(0, nullIndex[0]);

			catched = false;
			try {
				index1.Insert(7, "test 1");
			} catch (IndexOutOfRangeException) {
				catched = true;
			}
			Assert.IsTrue(catched);

			index1.Clear();
			Assert.AreEqual(0, indices.Count);
			Assert.AreEqual(0, nullIndex.Count);
		}

		[TestMethod, TestCategory("SpeedyListIndex")]
		public void Remove() {
			var list1 = new List<string>();
			var index1 = new SpeedyListIndex<string>(list1);
			var indices = TestUtility.GetPrivateField<StringIndex>(index1, "indices");
			var nullIndex = TestUtility.GetPrivateField<List<int>>(index1, "nullIndex");

			bool catched = false;
			try {
				index1.Insert(1, "test 1");
			} catch (IndexOutOfRangeException) {
				catched = true;
			}
			Assert.IsTrue(catched);

			catched = false;
			try {
				index1.Insert(-1, "test 1");
			} catch (IndexOutOfRangeException) {
				catched = true;
			}
			Assert.IsTrue(catched);

			list1.Insert(0, "test 1");
			index1.Insert(0, "test 1");

			list1.Insert(0, "test 2");
			index1.Insert(0, "test 2");

			list1.Insert(0, "test 3");
			index1.Insert(0, "test 3");

			list1.Insert(0, null);
			index1.Insert(0, null);

			list1.Insert(1, "test 1");
			index1.Insert(1, "test 1");

			list1.Insert(1, "test 2");
			index1.Insert(1, "test 2");

			Assert.AreEqual(3, indices.Count);
			Assert.AreEqual(1, nullIndex.Count);

			Assert.AreEqual(0, index1.IndexOf(null));
			Assert.AreEqual(2, index1.IndexOf("test 1"));
			Assert.AreEqual(3, index1.IndexOf("test 3"));
			Assert.AreEqual(1, index1.IndexOf("test 2"));

			Assert.AreEqual(2, indices["test 1"].Count);
			Assert.AreEqual(2, indices["test 1"][0]);
			Assert.AreEqual(5, indices["test 1"][1]);

			Assert.AreEqual(2, indices["test 2"].Count);
			Assert.AreEqual(1, indices["test 2"][0]);
			Assert.AreEqual(4, indices["test 2"][1]);

			Assert.AreEqual(1, indices["test 3"].Count);
			Assert.AreEqual(3, indices["test 3"][0]);

			Assert.AreEqual(1, nullIndex.Count);
			Assert.AreEqual(0, nullIndex[0]);

			catched = false;
			try {
				index1.Insert(7, "test 1");
			} catch (IndexOutOfRangeException) {
				catched = true;
			}
			Assert.IsTrue(catched);

			index1.Clear();
			Assert.AreEqual(0, indices.Count);
			Assert.AreEqual(0, nullIndex.Count);
		}
	}
}
