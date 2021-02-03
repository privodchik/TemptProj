using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptProj.StateMachine
{
    class StRun : State
    {
        public StRun()
        {
            EName = eState.RUN;
        }
        public override void operate()
        {
            base.operate();
        }
    }
}
