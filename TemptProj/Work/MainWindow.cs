using System;
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

        public async Task<byte[]> serial_port_poll_task(byte[] _txMsg, CancellationToken _ct)
        {
            Task _cycleTime = Task.Delay(100, _ct);

            m_serialPort.DiscardOutBuffer();
            m_serialPort.DiscardInBuffer();
            m_serialPort.Write(_txMsg, 0, _txMsg.Length);
            Task _tskSendMsg = Task.Run(send_frame, _ct);
            try
            {
                await wait_with_timout_async(_tskSendMsg, 5, _ct);
            }
            catch
            {
                txtBlckEWr.Text = (++m_modbus.ErrorRDCounter).ToString();
            }

            byte[] _rxMsg = await Task.Run(rx_frame, _ct);

            await _cycleTime;

            return _rxMsg;
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

        void blink_task(int _periodMS, CancellationToken _ct)
        {
            TimerCallback _timerCB = new TimerCallback( _obj =>
                    {
                        Dispatcher.BeginInvoke(new Action(()=>
                            {
                                rectBlink.Fill = rectBlink.Fill != Brushes.Green ?
                                    Brushes.Green : Brushes.White;
                            }
                        ));

                        if (((CancellationToken)_obj).IsCancellationRequested)
                        {
                            Dispatcher.BeginInvoke(new Action(()=>
                            {
                                rectBlink.Fill = Brushes.White;
                            }));
                            m_timerForBlink.Dispose();
                        }

                    }
                );

            m_timerForBlink = new Timer(_timerCB, _ct, _periodMS, _periodMS);

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
