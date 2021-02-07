using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptProj.StateMachine
{
    public class StRun : State
    {
        public StRun(object _parent = null) : base(_parent)
        {
            EName = eState.RUN;
        }
        public async override Task operate_async()
        {
            await base.operate_async();
        }
    }
}
