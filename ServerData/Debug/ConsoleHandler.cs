using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CommonLibrary.Debug
{
    public class ConsoleHandler
    {
        private List<Tuple<String, Func<Object>>> basic = new List<Tuple<String, Func<Object>>>();
        private Dictionary<String, Tuple<DateTime, int>> events = new Dictionary<String, Tuple<DateTime, int>>();
        private string debug = "";
        private Timer printTimer = new Timer(100);

        private object internalLock = new object();

        /// <summary>
        /// Set up with information to show, explaining string, and value in a tuple.
        /// </summary>
        public ConsoleHandler()
        {
            printTimer.Elapsed += printTimer_Elapsed;
            printTimer.Start();
        }

        /// <summary>
        /// Called by timer ten times per second.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            List<String> toBeRemoved = new List<string>();
            lock (internalLock)
            {
                foreach (KeyValuePair<String, Tuple<DateTime, int>> kvp in events)
                {
                    if (kvp.Value.Item1.AddMilliseconds(kvp.Value.Item2) < DateTime.Now)
                        toBeRemoved.Add(kvp.Key);
                }
                foreach (string s in toBeRemoved)
                {
                    events.Remove(s);
                }
            }
            Print();
        }

        /// <summary>
        /// Adds information about an event, with a timer for when it should be hidden again.
        /// </summary>
        /// <param name="information"></param>
        /// <param name="timer">in seconds</param>
        public void AddEventInformation(string information, int timer)
        {
            lock (internalLock)
            {
                string str = information;
                while (events.ContainsKey(str))
                {
                    str = "_" + str;
                }
                events.Add(str, new Tuple<DateTime, int>(DateTime.Now, timer * 1000));
            }
        }

        /// <summary>
        /// Adds information about an event, with a timer for when it should be hidden again. Default 5 sec.
        /// </summary>
        /// <param name="information"></param>
        public void AddEventInformation(string information)
        {
            lock (internalLock)
            {
                AddEventInformation(information, 5);
            }
        } 

        /// <summary>
        /// Adds set to the default information to print.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="getter"></param>
        public void AddBasicInformation(String str, Func<Object> getter)
        {
            lock (internalLock)
            {
                basic.Add(new Tuple<String, Func<Object>>(str, getter));
            }
        }

        /// <summary>
        /// Prints basic information and a debugstring if present.
        /// </summary>
        private void Print()
        {
            lock (internalLock)
            {
                Console.Clear();
                Console.WriteLine("Information: ");
                foreach (Tuple<String, Func<Object>> t in basic)
                {
                    Console.WriteLine(String.Format("{0,-40} {1,0}", t.Item1, ((t.Item2() == null) ? "null" : t.Item2().ToString())));
                }
                if (events.Count > 0)
                {
                    Console.WriteLine("\nEvents: ");
                    foreach (KeyValuePair<String, Tuple<DateTime, int>> kvp in events)
                    {
                        Console.WriteLine(kvp.Key.TrimStart('_'));
                    }
                }
                if (debug != "")
                {
                    Console.WriteLine("\nDebug: \n" + debug);
                }
            }
        }

        /// <summary>
        /// Set the debug string to something new.
        /// </summary>
        /// <param name="debugString"></param>
        public void SetDebugString(string debugString)
        {
            lock (internalLock)
            {
                debug = debugString;
            }
        }
        
        /// <summary>
        /// Resets the debug string to default value (removes it from print()).
        /// </summary>
        public void ResetDebugString()
        {
            lock (internalLock)
            {
                debug = "";
            }
        }
    }
}
