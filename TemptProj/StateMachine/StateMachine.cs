using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptProj.StateMachine
{

 
    public class StateMachine
    {
        

        private object m_parent;
        private State[] m_states;
        private State m_currentState;


        public StateMachine(object _parent = null)
        {
            m_states = new State[4] {
                new StInit(),
                new StRdy(),
                new StRun(),
                new StFlt()
            };
            m_currentState = m_states[0];
        }

        public void state_set(State.eState _state)
        {
            m_currentState = m_states[(int)_state];
        }

        public State state_set_next()
        {
            int _idx = (int)m_currentState.EName;
            _idx++;
            if (_idx > State.NUM_OF_STATES - 1) _idx = 0;
            state_set((State.eState)_idx);
            return m_currentState;
        }

        public State state_get()
        {
            return m_currentState;
        }

        public void operate()
        {
            m_currentState.operate();
        }
    }
}
