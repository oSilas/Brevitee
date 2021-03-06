using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brevitee.Logging;
using System.Reflection;

namespace Brevitee.Automation
{
    /// <summary>
    /// A set of workers that are run in sequence
    /// </summary>
    public class Job: Loggable
    {
        Dictionary<string, IWorker> _workers;
        public Job(string name)
        {
            this.Name = name;
            this.WorkQueue = new Queue<IWorker>();
            this._workers = new Dictionary<string, IWorker>();
        }

        public Job(JobConf conf)
            : this(conf.Name)
        {
            this.Conf = conf;
            conf.WorkerFiles.Each(confFile =>
            {
                WorkerConf workerConf = WorkerConf.Load(confFile);
                this.AddWorker(workerConf.CreateWorker(this));
            });
        }

        public JobConf Conf
        {
            get;
            private set;
        }

        public event EventHandler WorkStateSet;
        protected void OnWorkStateSet()
        {
            if (WorkStateSet != null)
            {
                WorkStateSet(this, new WorkStateEventArgs(CurrentWorkState));
            }
        }

        object _addLock = new object();
        public void AddWorker(IWorker worker)
        {
            lock(_addLock)
            {

                if (_workers.ContainsKey(worker.Name))
                {
                    throw new InvalidOperationException("Worker with the name {0} has already been added"._Format(worker.Name));
                }
                
                worker.Job = this;

                _workers.Add(worker.Name, worker);
            }
        }

        public string Name { get; set; }
        protected Queue<IWorker> WorkQueue { get; private set; }

        WorkState _workState;
        public WorkState CurrentWorkState
        {
            get
            {
                return _workState;
            }
            set
            {
                _workState = value;
                OnWorkStateSet();
            }
        }

        public IWorker this[string workerName]
        {
            get
            {
                IWorker result = null;
                if (_workers.ContainsKey(workerName))
                {
                    result = _workers[workerName];
                }

                return result;
            }            
        }

        public string[] WorkerNames
        {
            get
            {
                List<IWorker> list = _workers.Values.ToList();
                list.Sort((l, r) => l.StepNumber.CompareTo(r.StepNumber));
                return list.Select(w => w.Name).ToArray();
            }
        }

        public T GetWorker<T>(string workerName) where T : class, IWorker
        {
            return this[workerName] as T;
        }

        /// <summary>
        /// Tries to cast CurrentWorkState to WorkState&lt;T&gt;
        /// and returns WorkState.Data if it exists, otherwise 
        /// returns null;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetWorkStateData<T>()
        {
            T result = default(T);
            if (CurrentWorkState != null)
            {
                WorkState<T> workState;
                CurrentWorkState.TryCast<WorkState<T>>(out workState);
                result = workState.Data;
            }

            return result;
        }

        [Verbosity(LogEventType.Information, MessageFormat="JobName={Name}")]
        public event EventHandler JobStarted;

        protected void OnJobStarted()
        {
            if (JobStarted != null)
            {
                JobStarted(this, new WorkStateEventArgs(CurrentWorkState));
            }
        }

        [Verbosity(LogEventType.Information, MessageFormat="JobName={Name}, Worker={CurrentWorkerName}")]
        public event EventHandler WorkerQueued;

        protected void OnWorkerQueued()
        {
            if (WorkerQueued != null)
            {
                WorkerQueued(this, new WorkStateEventArgs(CurrentWorkState));
            }
        }

        [Verbosity(LogEventType.Information, MessageFormat = "JobName={Name}")]
        public event EventHandler JobFinished;

        protected void OnJobFinished()
        {
            if (JobFinished != null)
            {
                JobFinished(this, new WorkStateEventArgs(CurrentWorkState));
            }
        }

        [Verbosity(LogEventType.Information, MessageFormat = "JobName={Name},Worker={CurrentWorkerName}")]
        public event EventHandler WorkerStarting;        

        protected void OnWorkerStarting()
        {
            if (WorkerStarting != null)
            {
                WorkerStarting(this, new WorkStateEventArgs(CurrentWorkState));
            }
        }

        [Verbosity(LogEventType.Information, MessageFormat = "JobName={Name},Worker={CurrentWorkerName}")]
        public event EventHandler WorkerFinished;

        protected void OnWorkerFinished()
        {
            if (WorkerFinished != null)
            {
                WorkerFinished(this, new WorkStateEventArgs(CurrentWorkState));
            }
        }

        [Verbosity(LogEventType.Error, MessageFormat = "JobName={Name},Worker={CurrentWorkerName},Message={WorkStateMessage}")]
        public event EventHandler WorkerException;

        protected void OnWorkerException()
        {
            if (WorkerException != null)
            {
                WorkerException(this, new WorkStateEventArgs(CurrentWorkState));
            }
        }

        /// <summary>
        /// Represents the name of the currently running worker
        /// </summary>
        public string CurrentWorkerName
        {
            get
            {
                if(CurrentWorkState != null && !string.IsNullOrEmpty(CurrentWorkState.WorkName))
                {
                    return CurrentWorkState.WorkName;
                }
                
                return "UNKNOWN";
            }
        }

        /// <summary>
        /// The Message property of the CurrentWorkState
        /// </summary>
        public string WorkStateMessage
        {
            get
            {
                if (CurrentWorkState != null)
                {
                    return CurrentWorkState.Message ?? string.Empty;
                }

                return string.Empty;
            }
        }

        protected internal int StepNumber
        {
            get;
            set;
        }

        public void Run()
        {
            OnJobStarted();
            bool addToQueue;
            WorkerNames.Each((workerName, i) =>
            {
                addToQueue = i >= StepNumber;
                if (addToQueue)
                {
                    IWorker work = this[workerName];
                    CurrentWorkState = new WorkState(work, "queueing worker");
                    WorkQueue.Enqueue(work);
                    OnWorkerQueued();
                }
            });

            while (WorkQueue.Count > 0)
            {
                IWorker work = WorkQueue.Dequeue();

                CurrentWorkState = new WorkState(work, "starting work {0}"._Format(work.Name));

                OnWorkerStarting();

                CurrentWorkState = work.Do(this);

                OnWorkerFinished();

                if (CurrentWorkState.Status == Status.Failed)
                {
                    OnWorkerException();
                    break;
                }
            }

            OnJobFinished();
        }
    }
}
