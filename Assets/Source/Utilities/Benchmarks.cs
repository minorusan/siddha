using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Assets.Source.Utilities
{
    public static class Benchmark
    {
        private static Stopwatch _timer = new Stopwatch();

        public static void PerformBenchmark(Action action, out double[] timeLapses, out double midValue, int testCount)
        {
            timeLapses = new double[testCount];
            
            for (int i = 0; i < testCount; i++)
            {
                _timer.Start();
                action();
                _timer.Stop();
                timeLapses[i] = _timer.Elapsed.TotalMilliseconds;
                _timer.Reset();
            }
            midValue = timeLapses.Sum() / testCount;
        }
    }
}
