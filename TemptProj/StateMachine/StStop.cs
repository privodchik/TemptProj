using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptProj.StateMachine
{
    public class StStop : State
    {
        public StStop(object _parent = null) : base(_parent)
        {
            EName = eState.STOP;
        }
    }
}
