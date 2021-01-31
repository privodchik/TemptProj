using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptProj.Modbus
{
    class ModBus
    {
        public int ErrorWRCounter { get; set; }
        public int ErrorRDCounter { get; set; }

        public ModBus()
        {
            ErrorRDCounter = 0;
            ErrorWRCounter = 0;
        }

        public byte[] make_frame(int _address)
        {
            byte[] _array = new byte[1];
            _array[0] = Convert.ToByte(_address);
            return _array;
        }
    }
}
