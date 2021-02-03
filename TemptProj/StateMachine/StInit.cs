using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptProj.StateMachine
{
    class StInit : State
    {
        public StInit()
        {
            EName = eState.INI;
        }
        public override void operate() {
            base.operate();
        }
    }
}
