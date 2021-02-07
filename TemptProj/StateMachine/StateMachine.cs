using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TemptProj.StateMachine
{

 
    public class StateMachine
    {
        

        private object m_parent;
        private State[] m_states;
        private State m_currentState;


        public StateMachine(State[] _states, object _parent = null)
        {
            m_states = _states;
            m_currentState = m_states[0];

            m_parent = _parent;
        }

        public void state_set(State.eState _state)
        {
            if (_state != m_currentState.EName)
            {
                m_currentState.reset();
                m_currentState = m_states[(int)_state];
            }
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

        public async Task operate_async(CancellationToken _ct)
        {
            await m_currentState.operate_async();
        }
    }
}
