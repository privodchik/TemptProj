using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptProj.StateMachine
{
    public class StRdy : State
    {
        public StRdy(object _parent = null) : base(_parent)
        {
            EName = eState.READY;
        }
        public async override Task operate()
        {
            await base.operate();

        }
    }
}
