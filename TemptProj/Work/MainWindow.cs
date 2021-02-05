﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;


namespace TemptProj
{
    public partial class MainWindow : Window
    {
        async Task send_frame()
        {
            while (m_serialPort.BytesToWrite > 0)
            {
                await Task.Delay(1, m_cts.Token);
            }
        }

        async void receive_frame()
        {
            try
            {
                await Task.Delay(5, m_cts.Token);
            }
            catch { }

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

        async Task<byte[]> rx_frame()
        {
            byte[] _rxMsg = null;

            try
            {
                await Task.Delay(20, m_cts.Token);
            }
            catch { }

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

                _rxMsg = new byte[_oldDataInInputBuffer];
                m_serialPort.Read(_rxMsg, 0, _oldDataInInputBuffer);
            }
            else
            {
                ++m_modbus.ErrorRDCounter;
            }
            return _rxMsg;
        }

        void operate_task()
        {

        }

        async void state_machine_task_async(int _periodMS, CancellationToken  _ct)
        {
            while (!_ct.IsCancellationRequested)
            {
                Task _tsk = Task.Delay(_periodMS, _ct);

                try
                {
                    await m_stateMachine.operate_async(_ct);
                    await _tsk;
                }
                catch { }
            }
        }

        void blink_task(int _periodMS)
        {
            TimerCallback _timerCB = new TimerCallback( _obj =>
                    {
                        Dispatcher.BeginInvoke(new Action(()=>
                            {
                                rectBlink.Fill = rectBlink.Fill != Brushes.Green ?
                                    Brushes.Green : Brushes.White;
                            }
                        ));

                    }
                );

            m_timer = new Timer(_timerCB, null, _periodMS, _periodMS);

        }

        void blink_task_stop()
        {
            rectBlink.Fill = Brushes.White;
            m_timer.Dispose();
        }

    }

    internal static class AsyncExtensions
    {
        public static void disable_async_warning()
        {
            ;
        }
    }


}
