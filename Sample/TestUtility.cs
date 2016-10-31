using System;
using System.Collections.Generic;
using System.Text;

namespace SpeedyList.Sample {
	class TestUtility {
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

		static readonly string[] SizeSuffixes =
				   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
		public static string SizeSuffix(Int64 value) {
			if (value < 0) { return "-" + SizeSuffix(-value); }
			if (value == 0) { return "0.0 bytes"; }

			int mag = (int)Math.Log(value, 1024);
			decimal adjustedSize = (decimal)value / (1L << (mag * 10));

			return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
		}
		
		public static string FormatResult(string name, long time, long memory) {
			return string.Format(
				"{0}: {1} ms : Memory: {2}",
				name,
				time,
				SizeSuffix(memory)
			) + Environment.NewLine;
		}
	}
}
