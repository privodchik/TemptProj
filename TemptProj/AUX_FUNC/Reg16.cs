using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptProj.AUX_FUNC
{
    public class Reg16
    {
        public static byte hi(ushort _reg) { return (byte)(_reg >> 8); }
        public static byte lo(ushort _reg) { return (byte)(_reg); }
    }
}
