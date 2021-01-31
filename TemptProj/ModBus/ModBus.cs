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

        private byte[] m_inputBuffer; 

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

        public void copy_to_buffer(byte[] _array)
        {
            m_inputBuffer = new byte[_array.Length];
            m_inputBuffer = _array;
        }

        public void input_buffer_resize(byte _length)
        {
            m_inputBuffer = new byte[_length];
        }
        public byte[] input_buffer_get() { return m_inputBuffer; }

    }
}
