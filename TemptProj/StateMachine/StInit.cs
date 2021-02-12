using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TemptProj.ModBus.Frame;


namespace TemptProj.StateMachine
{
    public class StInit : State
    {
        public StInit(object _parent = null) : base(_parent)
        {
            EName = eState.INI;
        }
        public async override Task operate_async() {
            await base.operate_async();

            PDUReadInputs _framePDU = new ModBus.Frame.PDUReadInputs();
            _framePDU.StartAddress = 0;
            _framePDU.RegsQuant = 1;
            _framePDU.make();

            byte _slAddr = 1;
            byte[] _adu =   Modbus.ModBus.make_frame(_slAddr, _framePDU.PduBuffer);

            byte[] _rxMsg = null;
            try
            {
                _rxMsg = await ((MainWindow)m_parent).serial_port_poll_task(_adu, m_cts.Token);
            }
            catch { }

            if (Modbus.ModBus.check_frame(_rxMsg))
            {
                if (Modbus.ModBus.check_slave_address(_rxMsg, _slAddr))
                    state_set(eState.READY);

            }
            else
                return;
                
        }

    }
}
