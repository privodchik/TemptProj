using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;



namespace TemptProj
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort m_serialPort;
        private Modbus.ModBus m_modbus = new Modbus.ModBus();
        private System.Threading.CancellationTokenSource m_cts;
        
        public MainWindow()
        {
            InitializeComponent();

            btnConnect.Tag = false;

            string[] _availableSerialPots = SerialPort.GetPortNames();

            if (_availableSerialPots.Length != 0)
            {
                Array.Sort(_availableSerialPots);

                foreach (string x in _availableSerialPots)
                    cmbPort.Items.Add(x);

                cmbPort.SelectedIndex = 0;
            }
        }

        void background_func()
        {
            m_cts = new System.Threading.CancellationTokenSource();
            modbus_task(m_cts.Token);

        }
        async void modbus_task(System.Threading.CancellationToken _ct)
        {
            txtBlckView.Inlines.Add("modbus task has been started\n");
            

            while (!_ct.IsCancellationRequested)
            {
                System.Diagnostics.Stopwatch _time = new System.Diagnostics.Stopwatch();
                _time.Start(); 

                Task _cycleTime  = Task.Delay(100, _ct);
                                  
                m_serialPort.DiscardOutBuffer();
                byte[] _msg = m_modbus.make_frame(1);
                m_serialPort.Write(_msg, 0, _msg.Length);

                Task _taskSendMsg = Task.Run((Action)send_frame, _ct);
                try
                {
                    await waitWithTimout(_taskSendMsg, 5, _ct);
                    
                }
                catch
                {
                    m_modbus.ErrorWRCounter++;
                    txtBlckEWr.Text = m_modbus.ErrorWRCounter.ToString();
                }

                m_serialPort.DiscardInBuffer();
                await Task.Run((Action)receive_frame, _ct);

                try
                {
                    txtBlckERd.Text = (m_modbus.ErrorRDCounter).ToString();
                    await _cycleTime;
                }
                catch
                {

                }
                _time.Stop();
                lblCycleTime.Content = _time.ElapsedMilliseconds;
            }
            txtBlckView.Inlines.Add(new Run("modbus_task has been cancelled\n"));
            
        }

        async Task waitWithTimout(Task _task, int _timout, System.Threading.CancellationToken _ct)
        {
            Task _delayTask = Task.Delay(_timout, _ct);
            Task _firstToFinish = await Task.WhenAny(_task, _delayTask);
            if (_firstToFinish == _delayTask)
            {
                throw new TimeoutException();
            }
            await _task;

        }
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToBoolean(btnConnect.Tag) == false)
            {
                try{

                    string _portName = cmbPort.Text;
                    int _portBaud = Convert.ToInt32(cmbBaud.Text);

                    m_serialPort = new SerialPort(_portName, _portBaud, Parity.None, 8, StopBits.One);
                    txtBlckView.Inlines.Add(new Run(_portName + '\n') { Foreground = Brushes.Blue});
                    txtBlckView.Inlines.Add(new Run(_portBaud.ToString() + '\n') { Foreground = Brushes.Blue });
                    txtBlckView.Inlines.Add(new Run("Parity None\n") { Foreground = Brushes.Blue });
                    txtBlckView.Inlines.Add(new Run("8 bits\n") { Foreground = Brushes.Blue });
                    txtBlckView.Inlines.Add(new Run("StopBits One\n") { Foreground = Brushes.Blue });

                    m_serialPort.Open();
                    txtBlckView.Inlines.Add(new Run("Port has been opened\n") { Foreground = Brushes.Blue });
                    btnConnect.Tag = true;

                    background_func();
                }
                catch
                {
                    Brush _oldBrush = txtBlckView.Foreground;
                    txtBlckView.Inlines.Add(new Run("Failed to open port\n\n") { Foreground = Brushes.Red });
                }
                
            }
            else
            {
                btnConnect.Tag = false;
                m_cts.Cancel();
                m_serialPort.Close();
                txtBlckView.Inlines.Add(new Run("Port has been closed\n") { Foreground = Brushes.Red });
            }

            btnConnect.Content = Convert.ToBoolean(btnConnect.Tag) ? "Disconnect" : "Connect";
        }

        private void btnClr_click(object sender, RoutedEventArgs e)
        {
            txtBlckView.Text = String.Empty;
        }

        private void btnErrReset_click(object sender, RoutedEventArgs e)
        {
            txtBlckEWr.Text = (m_modbus.ErrorWRCounter = 0).ToString();
            txtBlckERd.Text = (m_modbus.ErrorRDCounter = 0).ToString();
        }
    }
}
