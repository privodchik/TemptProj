using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace TemptProj
{
    public partial class MainWindow : Window
    {
        private void read_configuration()
        {
            //XmlDocument _iniFile = new XmlDocument();
            string _fileName = "ini.xml";
            try
            {
                XmlReaderSettings _settings = new XmlReaderSettings();
                _settings.IgnoreWhitespace = true;
                XmlReader _reader = XmlReader.Create(inputUri: _fileName, _settings);

                List<XmlNodeType> _nodes = new List<XmlNodeType>();
                List<string> _nodeNames = new List<string>();

                while (_reader.Read())
                {
                    _nodes.Add(_reader.NodeType);
                    _nodeNames.Add(_reader.Name);

                    if (_reader.IsStartElement("COM") && !_reader.IsEmptyElement)
                    {
                        try
                        {
                            string _com = _reader.ReadElementContentAsString();
                            cmbPort.SelectedValue = _com;
                        }
                        catch
                        {
                        }
                    }
                    if (_reader.IsStartElement("Baud") && !_reader.IsEmptyElement)
                    {
                        try
                        {
                            string _baud = _reader.ReadElementContentAsString();
                            cmbBaud.SelectedValue = _baud;
                        }
                        catch
                        {
                        }
                    }
                }
                _reader.Close();
            }
            catch (Exception e)
            {
                //XmlWriter _writer = XmlWriter.Create(_fileName);
                //_writer.

                XmlTextWriter _writer = new XmlTextWriter(_fileName, Encoding.GetEncoding(1251));
                _writer.Formatting = Formatting.Indented;
                _writer.WriteStartDocument();

                _writer.WriteStartElement("Data");
                
                _writer.WriteStartElement("COM");
                _writer.WriteString(cmbPort.SelectedItem.ToString());
                _writer.WriteEndElement();

                _writer.WriteStartElement("Baud");
                _writer.WriteString(cmbBaud.Text);
                _writer.WriteEndElement();

                _writer.WriteEndElement();

                _writer.WriteEndDocument();
                _writer.Flush();
                _writer.Close();

            }

        }
    }
}