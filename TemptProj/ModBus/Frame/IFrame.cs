using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptProj.ModBus.Frame
{

    enum eFrame
    {
        RD_HOLDINGS = 3,
        RD_INPUTS = 4,
        WR_HOLDINGS = 16
    }
    public interface IFrame
    {
        void make();
    }
}
