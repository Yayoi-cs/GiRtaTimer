using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfApp.Model {
    
    public class timerController {
        public readonly static Stopwatch rltStopWatch = new Stopwatch();
        public readonly static Stopwatch basicStopWatch = new Stopwatch();
        public static bool rltState;
        public static bool basicState;
        public static void _reset() {
            basicState = false;
            rltState = false;
            rltStopWatch.Reset();
            basicStopWatch.Reset();
        }
        public static void _start() {
            basicState = true;
            rltState = true;
            rltStopWatch.Start();
            basicStopWatch.Start();
        }
        public static void _restart() {
            basicState = true;
            rltState = true;
            rltStopWatch.Restart();
            basicStopWatch.Restart();
        }
        public static void _stop() {
            basicState = false;
            rltState = false;
            rltStopWatch.Stop();
            basicStopWatch.Stop();
        }
        public static void _lStop() {
            rltStopWatch.Stop();
            rltState=false;
        }
        public static void _lStart() {
            rltStopWatch.Start();
            rltState = true;
        }
    }
}
