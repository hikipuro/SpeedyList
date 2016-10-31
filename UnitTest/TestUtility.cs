using System;
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
	}
}
