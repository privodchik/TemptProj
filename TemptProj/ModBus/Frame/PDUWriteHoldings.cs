using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemptProj.AUX_FUNC;

namespace TemptProj.ModBus.Frame
{
    public class PDUWriteHoldings : PDUBaseFrame
    {
        public ushort RegsQuant { get; set; }

        public ushort[] Data { get; set; }

        public PDUWriteHoldings()
        {
            FuncNo = (byte)eFrame.WR_HOLDINGS;
        }

        public override byte[] make()
        {
            PduBuffer = new byte[6 + Data.Length];

            PduBuffer[0] = FuncNo;

            unsafe {
                ushort* _pArray;
                fixed (byte* _parray = &PduBuffer[1])
                {
                    _pArray = (ushort*)_parray;
                    *_pArray++ = StartAddress;
                    *_pArray++ = RegsQuant;
                    byte* _p = (byte*)_pArray;
                    *_p++ = (byte)(RegsQuant * 2);
                    _pArray = (ushort*)_p;

                    for (int i = 0; i < Data.Length; i++)
                        *_pArray++ = Data[i];
                }
            }
            return PduBuffer;
        }

    }
}
