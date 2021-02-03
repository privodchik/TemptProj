using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptProj.StateMachine
{
    class StRdy : State
    {
        public StRdy()
        {
            EName = eState.READY;
        }
        public override void operate()
        {
            base.operate();
        }
    }
}
