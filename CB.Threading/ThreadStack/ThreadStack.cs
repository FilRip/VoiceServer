using System.ComponentModel;
using System.Collections.Generic;
using System;
using System.Collections;

namespace CB.Threading.ThreadStack
{
    public class aTask
    {
        private DoWorkEventHandler _doWorkHandler;
        private RunWorkerCompletedEventHandler _runCompletedHandler;
        private Hashtable _arguments;

        public DoWorkEventHandler doWorkHandler
        {
            get { return _doWorkHandler; }
            set { _doWorkHandler = value; }
        }

        public RunWorkerCompletedEventHandler runCompletedHandler
        {
            get { return _runCompletedHandler; }
            set { _runCompletedHandler = value; }
        }

        public Hashtable arguments
        {
            get { return _arguments; }
            set { _arguments = value; }
        }
    }

    public class threadStack : System.ComponentModel.BackgroundWorker
    {

        private bool _multiHandlers = false;
        protected List<DoWorkEventHandler> _listDoWork = new List<DoWorkEventHandler>();
        protected List<RunWorkerCompletedEventHandler> _listRunCompleted = new List<RunWorkerCompletedEventHandler>();
        protected List<ProgressChangedEventHandler> _listProgressHandler = new List<ProgressChangedEventHandler>();
        private bool _pileAvailable = true;
        private List<aTask> _liste = new List<aTask>();
        private bool _pause;

        public threadStack()
            : base()
        {
            WorkerSupportsCancellation = true;
        }

        public int nbTasks
        {
            get { return _liste.Count; }
        }

        public bool isStillHaveTaskToDo
        {
            get { return ((_liste.Count == 0) ? false : true); }
        }

        public void addTask(aTask tache)
        {
            addTask(tache, false);
        }

        public void addTask(aTask tache, bool prioritaire)
        {
            if (!_pileAvailable && (_liste.Count > 0))
            {
                throw new Exception("Already busy");
            }

            if (prioritaire && (_liste.Count > 0))
                _liste.Insert(0, tache);
            else
                _liste.Add(tache);
            
            if (!IsBusy)
                launchNextTask();
        }

        public void addTask(System.ComponentModel.DoWorkEventHandler doWork, System.ComponentModel.RunWorkerCompletedEventHandler runCompleted)
        {
            addTask(doWork, runCompleted, null, false);
        }
        public void addTask(System.ComponentModel.DoWorkEventHandler doWork, System.ComponentModel.RunWorkerCompletedEventHandler runCompleted, Hashtable arguments)
        {
            addTask(doWork, runCompleted, arguments, false);
        }

        public void addTask(System.ComponentModel.DoWorkEventHandler doWork, System.ComponentModel.RunWorkerCompletedEventHandler runCompleted, Hashtable arguments, bool prioritaire)
        {
            aTask tache = new aTask();
            tache.doWorkHandler = doWork;
            tache.runCompletedHandler = runCompleted;
            tache.arguments = arguments;
            addTask(tache, prioritaire);
        }

        public void launchNextTask()
        {
            launchNextTask(false);
        }

        public void launchNextTask(bool force)
        {
            if ((!_pause  || force)  && (_liste.Count > 0))
            {
                RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.executeTacheSuivante);
                setHandlers();
                RunWorkerAsync(_liste[0].arguments);
            }
        }

        public void cancelAllTasks()
        {
            if (IsBusy)
                CancelAsync();
            _liste.Clear();
        }

        private void executeTacheSuivante(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if ((_liste.Count > 0))
                _liste.Remove(_liste[0]);
            remMyHandler();
            if (!_pause)
                launchNextTask();
        }

        void remMyHandler()
        {
            RunWorkerCompleted -= new RunWorkerCompletedEventHandler(this.executeTacheSuivante);
        }

        public bool isPaused
        {
            get { return _pause; }
            set
            {
                _pause = value;
                if (!_pause)
                    launchNextTask();
            }
        }

        // '' <summary>
        // '' Cette classe de gestion multithread supporte-t-elle le multiCallBack
        // '' </summary>
        // '' <value></value>
        // '' <returns></returns>
        // '' <remarks></remarks>
        public bool multiHandlers
        {
            get { return _multiHandlers; }
            set { _multiHandlers = value; }
        }

        public List<DoWorkEventHandler> listDoWorkHandler
        {
            get { return _listDoWork; }
        }

        public List<RunWorkerCompletedEventHandler> listRunWorkerHandler
        {
            get { return _listRunCompleted; }
        }

        public List<ProgressChangedEventHandler> listProgressHandlers
        {
            get { return _listProgressHandler; }
        }

        public void setHandlers()
        {
            if (!_multiHandlers)
                clearHandlers();

            this.DoWork += _liste[0].doWorkHandler;
            _listDoWork.Add(_liste[0].doWorkHandler);
            RunWorkerCompleted += _liste[0].runCompletedHandler;
            _listRunCompleted.Add(_liste[0].runCompletedHandler);
        }
        public void setHandlers(ref DoWorkEventHandler doWork, ref RunWorkerCompletedEventHandler runCompleted)
        {
            if (!_multiHandlers)
                clearHandlers();

            this.DoWork += doWork;
            _listDoWork.Add(doWork);
            RunWorkerCompleted += runCompleted;
            _listRunCompleted.Add(runCompleted);
        }
        public void setHandlers(ref DoWorkEventHandler doWork, ref RunWorkerCompletedEventHandler runCompleted, ref ProgressChangedEventHandler progressHandler)
        {
            if (!_multiHandlers)
                clearHandlers();

            this.DoWork += doWork;
            _listDoWork.Add(doWork);
            RunWorkerCompleted += runCompleted;
            _listRunCompleted.Add(runCompleted);
            if (progressHandler != null)
                ProgressChanged += progressHandler;
        }

        public void clearHandlers()
        {
            if (_listDoWork!=null)
            {
                foreach (DoWorkEventHandler doWorkHandler in _listDoWork)
                    if (doWorkHandler != null) this.DoWork -= doWorkHandler;
                _listDoWork.Clear();
            }
            if (_listRunCompleted!=null)
            {
                foreach (RunWorkerCompletedEventHandler runWorkerHandler in _listRunCompleted)
                    RunWorkerCompleted -= runWorkerHandler;
                _listRunCompleted.Clear();
            }
            if (_listProgressHandler!=null)
            {
                foreach (ProgressChangedEventHandler progressHandler in _listProgressHandler)
                    this.ProgressChanged -= progressHandler;
                _listProgressHandler.Clear();
            }
        }
    }
}
