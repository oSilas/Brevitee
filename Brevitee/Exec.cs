using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Brevitee
{
    public static class Exec
    {
        static volatile Dictionary<string, Thread> _threads;
        static Exec()
        {
            _threads = new Dictionary<string, Thread>();
        }

        public static NamedThread GetThread(string name)
        {
            if (_threads.ContainsKey(name))
            {
                return new NamedThread(name, _threads[name]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Create a NamedThread with the specified name to run
        /// the specified action when it is started.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static NamedThread SetThread(string name, Action action)
        {
            NamedThread thread = new NamedThread(name, action);
            _threads[name] = thread;

            return thread;
        }

        public static NamedThread Start(Action action)
        {
            return Start(Guid.NewGuid().ToString(), action);
        }

        public static NamedThread Start(string name, Action action)
        {
            Thread thread = new Thread(() =>
            {
                action();
            });

            if (_threads.ContainsKey(name))
            {
                try
                {
                    _threads[name].Abort();
                    _threads[name].Join(3000);
                }
                catch { }
                _threads[name] = thread;
            }
            else
            {
                _threads.Add(name, thread);
            }
            thread.Start();
            return new NamedThread(name, thread);
        }

        public static void Kill(string name)
        {
            if (_threads.ContainsKey(name))
            {
                Thread victim = _threads[name];
                if (victim.ThreadState == ThreadState.Running)
                {
                    try
                    {
                        victim.Abort();
                        victim.Join(3000);
                    }
                    catch { }
                }
                _threads.Remove(name);
            }
        }

        public static bool TakesTooLong<TResult>(this Func<TResult> function, Func<TResult, TResult> callBack, string threadName, int millisecondsToWait = 300)
        {
            return function.TakesTooLong(callBack, new TimeSpan(0, 0, 0, 0, millisecondsToWait), threadName);
        }

        public static bool TakesTooLong<TResult>(this Func<object, TResult> function, Func<TResult, TResult> callBack, string threadName, object state = null, int millisecondsToWait = 300)
        {
            return function.TakesTooLong(callBack, new TimeSpan(0, 0, 0, 0, millisecondsToWait), state, threadName);
        }

        /// <summary>
        /// Returns true if the specified function takes longer to execute than the specified secondsToWait.
        /// </summary>
        /// <typeparam name="TResult">The Type returned by the sepcified function, also the return and parameter type of the
        /// specified callBack.</typeparam>
        /// <param name="function">The function to execute and time</param>
        /// <param name="callBack">The callBack to execute when function completes</param>
        /// <param name="secondsToWait">The number of seconds to allow the function to execute before returning true</param>
        /// <returns>boolean</returns>
        public static bool TakesTooLong<TResult>(this Func<TResult> function, Func<TResult, TResult> callBack, int secondsToWait)
        {
            return function.TakesTooLong(callBack, new TimeSpan(0, 0, secondsToWait));
        }

        public static bool TakesTooLong<TResult>(this Func<TResult> function, Func<TResult, TResult> callBack, TimeSpan timeToWait, string threadName = null)
        {
            return TakesTooLong((o) =>
            {
                return function();
            }, callBack, timeToWait, null, threadName);
        }

        /// <summary>
        /// Executes the specified function in a separate thread waiting the specified timeToWait.  If
        /// the function is not done executing in the specified timeToWait 
        /// </summary>
        /// <typeparam name="TResult">The Type returned by the sepcified function, also the return and parameter type of the
        /// specified callBack.</typeparam>
        /// <param name="function">The function to execute and time</param>
        /// <param name="callBack">The callBack to execute when function completes</param>
        /// <param name="timeToWait">The ammount of time to allow the function to execute before returning true</param>
        /// <returns>boolean</returns>
        public static bool TakesTooLong<TResult>(this Func<object, TResult> function, Func<TResult, TResult> callBack, TimeSpan timeToWait, object state = null, string threadName = null)
        {
            // blocks thread until execution completion or timeToWait expires, see below .WaitOne()
            AutoResetEvent returnThreadController = new AutoResetEvent(false);

            int millisecondsToWait = (int)timeToWait.TotalMilliseconds;

            bool? tookTooLong = false;
            if (string.IsNullOrEmpty(threadName))
            {
                threadName = "Brevitee.Thread_".RandomString(8);
            }

            int suffix = 1;
            while (_threads.ContainsKey(threadName))
            {
                threadName = string.Format("{0}_{1}", threadName, suffix.ToString());
                suffix++;
            }

            Thread functionThread = new Thread(() =>
            {
                if (callBack != null)
                {
                    callBack(function(state));
                }
                else
                {
                    function(state);
                }
                returnThreadController.Set();
                _threads.Remove(threadName);
            });            
            
            functionThread.IsBackground = true;
            _threads.Add(threadName, functionThread); // make sure the thread doesn't get garbage collected
            // give the function a head start
            functionThread.Start();

            Timer timer = new Timer((o) =>
            {
                tookTooLong = true;
                returnThreadController.Set(); // execution took too long
            }, null, millisecondsToWait, Timeout.Infinite);

            returnThreadController.WaitOne();

            return tookTooLong.Value;
        }

        public static TimeSpan Time(this Action action)
        {
            DateTime start = DateTime.Now;
            action();
            DateTime end = DateTime.Now;
            return end.Subtract(start);
        }

        public static TimeSpan Time<TResult>(this Func<TResult> func, out TResult result)
        {
            return func.TimeExecution(out result);
        }

        /// <summary>
        /// Time the execution of the specified function returning a TimeSpan
        /// instance representing the ammount of time it took for the function
        /// to run
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static TimeSpan TimeExecution<TResult>(this Func<TResult> func, out TResult result)
        {
            DateTime start = DateTime.Now;
            result = func();
            DateTime end = DateTime.Now;
            return end.Subtract(start);
        }

        public static void InThread<TResult>(this Func<TResult> func, Func<TResult, TResult> callBack)
        {
            TakesTooLong<TResult>(func, callBack, 0);
        }

        public static Thread InThread(this ThreadStart method)
        {
            Thread thread = new Thread(method);
            thread.Start();
            return thread;
        }

        public static Thread InThread(this ParameterizedThreadStart method)
        {
            Thread thread = new Thread(method);
            thread.Start();
            return thread;
        }
    }
}
