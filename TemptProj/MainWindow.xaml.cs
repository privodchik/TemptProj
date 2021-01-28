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
                    m_serialPort = new SerialPort(Convert.ToString(cmbPort.SelectedValue), Convert.ToInt32(cmbBaud.SelectedValue), Parity.None, 8, StopBits.One);
                    m_serialPort.Open();
                }
                catch
                {

                }
                btnConnect.Tag = true;
                btnConnect.Content = "Disconnect";
            }
        }
    }
}
