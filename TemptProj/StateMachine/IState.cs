using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptProj.StateMachine
{


    interface IState
    {
        Task operate();
        void reset();
    }
}
