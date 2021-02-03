using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptProj.StateMachine
{

    enum eState
    {
        INI = 0,
        READY,
        RUN,
        FLT
    }
    interface IState
    {
        void operate();
    }
}
