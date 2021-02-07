using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemptProj.AUX_FUNC;

namespace TemptProj.ModBus.Frame
{
    public class PDUReadHoldings : PDUBaseFrame
    {
        public ushort RegsQuant { get; set; }

        public PDUReadHoldings() { FuncNo = (byte)eFrame.RD_HOLDINGS; }
        public override byte[] make()
        {
            PduBuffer = new byte[] {FuncNo,
                    Reg16.hi(StartAddress), Reg16.lo(StartAddress),
                    Reg16.hi(RegsQuant), Reg16.lo(RegsQuant)
            };

            return PduBuffer;
        }

    }
}
