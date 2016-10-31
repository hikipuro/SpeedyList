using System;
using System.Diagnostics;

namespace SpeedyList.Sample {
	class Benchmark {
		public delegate void Action();

		/// <summary>
		/// 実行時間を計測する.
		/// </summary>
		/// <param name="act"></param>
		/// <param name="iterations"></param>
		/// <returns></returns>
		public static long Start(Action act, int iterations) {
			if (iterations <= 0) {
				return 0;
			}
			GC.Collect();
			//act.Invoke(); // run once outside of loop to avoid initialization costs
			Stopwatch sw = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++) {
				act.Invoke();
			}
			sw.Stop();
			return sw.ElapsedMilliseconds / iterations;
		}
	}
}
