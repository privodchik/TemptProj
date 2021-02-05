using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace TemptProj.StateMachine
{
    public class StInit : State
    {
        public StInit(object _parent = null) : base(_parent)
        {
            EName = eState.INI;
        }
        public async override Task operate() {
            await base.operate();

            byte[] _mas = { 0x01,
                            0x03,
                            0x00, 0x00,
                            0x00, 0x01,
                            0x84, 0x0A
                          };

            byte[] _rxMsg = null;
            try
            {
                _rxMsg = await ((MainWindow)m_parent).serial_port_poll_task(_mas, m_cts.Token);
            }
            catch { }

            if (_rxMsg != null)
            {
                ushort _val = (ushort)((_rxMsg[3] << 8) | _rxMsg[4]);
                ((MainWindow)m_parent).m_stateMachine.state_set(eState.READY);
            }

                
        }

    }
}
