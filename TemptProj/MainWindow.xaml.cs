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
                m_serialPort.Close();
                txtBlckView.Inlines.Add(new Run("Port has been closed\n") { Foreground = Brushes.Red });
            }

            btnConnect.Content = Convert.ToBoolean(btnConnect.Tag) ? "Disconnect" : "Connect";
        }

        private void btnClr_click(object sender, RoutedEventArgs e)
        {
            txtBlckView.Text = String.Empty;
        }
    }
}
