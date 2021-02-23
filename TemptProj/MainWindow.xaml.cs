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
using System.Threading;
using TemptProj.StateMachine;



namespace TemptProj
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private State[] m_states;
        public  StateMachine.StateMachine m_stateMachine;

        private SerialPort m_serialPort;
        private Modbus.ModBus m_modbus = new Modbus.ModBus();
        private System.Threading.CancellationTokenSource m_cts;

        private Timer m_timerForBlink;

        public MainWindow()
        {
            InitializeComponent();

            m_states = new State[] 
            {
                    new StStop(this),
                    new StInit(this),
                    new StRdy(this),
                    new StRun(this),
                    new StFlt(this)
            };

            m_stateMachine = new StateMachine.StateMachine(m_states, this);

            lblState.Content = m_stateMachine.state_get().Name;

            btnConnect.Tag = false;

            string[] _availableSerialPots = SerialPort.GetPortNames();

            if (_availableSerialPots.Length != 0)
            {
                Array.Sort(_availableSerialPots);

                foreach (string x in _availableSerialPots)
                    cmbPort.Items.Add(x);

                cmbPort.SelectedIndex = 0;
            }

            read_configuration();
        }



        void background_func()
        {
            m_cts = new CancellationTokenSource();
            blink_task(1000, m_cts.Token);
            state_machine_task_async(100, m_cts.Token);
        }

        async Task wait_with_timout_async(Task _task, int _timout, CancellationToken _ct)
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
            if (Convert.ToBoolean(btnConnect.Tag) == false) // Disconnect state 
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
            else // Connect state
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

        private void btnSwitch_Click(object sender, RoutedEventArgs e)
        {
            m_serialPort.BaudRate = Convert.ToInt32(cmbBaud.Text);
            txtBlckView.Inlines.Add(new Run("Port " + m_serialPort.PortName + " baud is " + m_serialPort.BaudRate + "\n") { Foreground = Brushes.Blue });
        }
    }
}
