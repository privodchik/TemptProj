using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptProj.ModBus.Frame
{
    public class PDUBaseFrame : IPDUFrame
    {

        private byte[] m_pduBuffer;

        public byte[] PduBuffer {
            get { return m_pduBuffer; }
            set { m_pduBuffer = value; }
        }
        public ushort StartAddress { get; set; }

        private byte m_funcNo;
        public virtual byte FuncNo {
            get { return m_funcNo; }
            protected set { m_funcNo = value; } 
        }

        public virtual byte[] make() { return null; }
    }
}
