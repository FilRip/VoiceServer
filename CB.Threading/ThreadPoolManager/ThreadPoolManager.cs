using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;

namespace CB.Threading
{
    public class ThreadPoolManager
    {
        private class aJob
        {
            private int _beforeExecute;
            private int _beforeRepeat;
            private int _numThread;
            private Timer _core;

            public aJob(TimerCallback method, object param, int timerBeforeStart, int timerBeforeRepeat)
            {
                _beforeExecute = timerBeforeStart;
                _beforeRepeat = timerBeforeRepeat;
                _core = new Timer(method, param, timerBeforeStart, timerBeforeRepeat);
            }

            public aJob(TimerCallback method, object param, int timerBeforeStart)
            {
                _beforeExecute = timerBeforeStart;
                _beforeRepeat = 0;
                _core = new Timer(new TimerCallback(x => { method.Invoke(param); delete(); }), param, timerBeforeStart, 0);
            }

            public int beforeExecute
            {
                get { return _beforeExecute; }
                set { _beforeExecute = value; }
            }

            public int beforeRepeat
            {
                get { return _beforeRepeat; }
                set { _beforeRepeat = value; }
            }

            public int numThread
            {
                get { return _numThread; }
                set { _numThread = value; }
            }

            public void delete()
            {
                _core.Dispose();
                deleted(_numThread);
            }

            public void executeNow()
            {
                _core.Change(0, _beforeRepeat);
                _beforeExecute = 0;
            }

            public void changeDelay(int beforeStart, int beforeRep)
            {
                _core.Change(beforeStart, beforeRep);
                _beforeExecute = beforeStart;
                _beforeRepeat = beforeRep;
            }

            public delegate void delegateDeleted(int numThread);
            public event delegateDeleted deleted;
        }

        private List<aJob> _listThread;

        public ThreadPoolManager()
        {
            _listThread=new List<aJob>();
        }

        private void oneDeleted(int numThread)
        {
            aJob jobTrouve = null;
            foreach (aJob job in _listThread)
                if (job.numThread == numThread)
                {
                    _listThread.Remove(job);
                    jobTrouve = job;
                    break;
                }
            if (jobTrouve != null) jobTrouve = null;
        }

        public int addJob(TimerCallback method, object param, int timerBeforeStart)
        {
            aJob t;
            timerBeforeStart = Math.Max(0, timerBeforeStart);
            t = new aJob(method, param, timerBeforeStart);
            t.deleted += new aJob.delegateDeleted(oneDeleted);
            _listThread.Add(t);
            return setNumThread(t);
        }

        private int setNumThread(aJob t)
        {
            int max;
            max = maxNum() + 1;
            t.numThread = max;
            return max;
        }

        public int addJob(TimerCallback method, object param, int timerBeforeStart, int timerBeforeRepeat)
        {
            if (timerBeforeRepeat <= 0) return addJob(method, param, timerBeforeStart);
            timerBeforeStart = Math.Max(0, timerBeforeStart);
            aJob t;
            t = new aJob(method, param, timerBeforeStart, timerBeforeRepeat);
            t.deleted += new aJob.delegateDeleted(oneDeleted);
            _listThread.Add(t);
            return setNumThread(t);
        }

        public void executeNow(int numThread)
        {
            foreach (aJob job in _listThread)
                if (job.numThread == numThread)
                {
                    job.executeNow();
                    break;
                }
        }

        public void changeDelay(int numThread, int beforeStart, int beforeRepeat)
        {
            foreach (aJob job in _listThread)
                if (job.numThread == numThread)
                {
                    job.changeDelay(beforeStart, beforeRepeat);
                    break;
                }
        }

        public void stopJob(int numThread)
        {
            try
            {
                foreach (aJob job in _listThread)
                    if (job.numThread == numThread)
                    {
                        job.delete();
                        break;
                    }
            }
            catch { }
        }

        public void stopAllJobs()
        {
            for (int i = _listThread.Count-1; i >= 0; i--)
                _listThread[i].delete();
        }

        private int maxNum()
        {
            int max = 0;
            foreach (aJob job in _listThread)
                if (job.numThread > max) max = job.numThread;
            return max;
        }

        public int nbThreads
        {
            get { return _listThread.Count; }
        }
    }
}
