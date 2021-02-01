using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;


namespace TemptProj
{
    public partial class MainWindow : Window
    {
        async void send_frame()
        {
            while (m_serialPort.BytesToWrite > 0)
            {
                await Task.Delay(1, m_cts.Token);
            }
        }

        async void receive_frame()
        {
            await Task.Delay(5, m_cts.Token);

            int _oldDataInInputBuffer = m_serialPort.BytesToRead;

            if (_oldDataInInputBuffer > 0)
            {
                while (true)
                {
                    await Task.Delay(3, m_cts.Token);
                    if (m_serialPort.BytesToRead == _oldDataInInputBuffer)
                    {
                        break;
                    }
                    else
                    {
                        _oldDataInInputBuffer = m_serialPort.BytesToRead;
                    }
                }
                m_modbus.input_buffer_resize((byte)_oldDataInInputBuffer);
                m_serialPort.Read(m_modbus.input_buffer_get(), 0, _oldDataInInputBuffer);
            }
            else
            {
                ++m_modbus.ErrorRDCounter;
            }
        }

    }
}
