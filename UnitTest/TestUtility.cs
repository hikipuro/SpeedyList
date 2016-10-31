using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnitTest {
	class TestUtility {
		public static T GetPrivateField<T>(object obj, string name) {
			Type type = obj.GetType();
			FieldInfo fieldInfo = type.GetField(
				name,
				BindingFlags.Instance | BindingFlags.NonPublic
			);
			return (T)fieldInfo.GetValue(obj);
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
	}
}
