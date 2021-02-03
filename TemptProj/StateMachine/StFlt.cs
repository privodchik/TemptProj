using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptProj.StateMachine
{
    class StFlt : State
    {
        public StFlt()
        {
            EName = eState.FLT;
        }
        public override void operate()
        {
            base.operate();
        }
    }
}
