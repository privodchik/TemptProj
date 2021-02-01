using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptProj.ModBus.Frame
{
    class BaseFrame : IFrame
    {
        protected byte m_address;
        public virtual void make() { }
    }
}
