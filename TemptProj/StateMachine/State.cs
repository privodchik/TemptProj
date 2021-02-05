using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TemptProj;

namespace TemptProj.StateMachine
{
    public abstract class State
    {
        
        public enum eState
        {
            INI = 0,
            READY,
            RUN,
            FLT
        }


        protected object m_parent;

        protected eState m_eName;
        protected string m_Name;

        public const int NUM_OF_STATES = (int)eState.FLT + 1;

        public eState EName
        {
            get{ return m_eName; }
            protected set {
                m_eName = value;
                m_Name = value.ToString();
            }
        }

        public string Name
        {
            get { return m_Name; }
            private set { m_Name = value; }
        }


        public State(object _parent) { m_parent = _parent; }



        protected CancellationTokenSource m_cts;
        public virtual Task operate()
        {
            ((MainWindow)m_parent).lblState.Content = m_Name;
            m_cts = new CancellationTokenSource();
            return Task.FromResult(default(object));
        }


        public virtual void reset() {
            m_cts.Cancel();
        }
    }
}
