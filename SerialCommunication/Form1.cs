using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();
                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;

                comboBoxBaudrate.SelectedIndex = comboBoxBaudrate.Items.IndexOf("115200");
            }
            catch (Exception)
            { }
        }

        private void cboPoort_DropDown(object sender, EventArgs e)
        {
            try
            {
                string selected = (string)comboBoxPoort.SelectedItem;
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();

                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);

                comboBoxPoort.SelectedIndex = comboBoxPoort.Items.IndexOf(selected);
            }
            catch (Exception)
            {
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen) //ik heb een verbinding --> gebruiker wil verbreken
                {
                    serialPortArduino.Close();
                    radioButtonVerbonden.Checked = false;
                    buttonConnect.Text = "Connect";
                    labelStatus.Text = "Status: Disconnected";
                }

                else //ik heb GEEN verbinding --> gebruiker wil verbinding maken
                {
                    serialPortArduino.PortName = (string)comboBoxPoort.SelectedItem;
                    serialPortArduino.BaudRate = Int32.Parse((string)comboBoxBaudrate.SelectedItem);
                    serialPortArduino.DataBits = (int)numericUpDownDatabits.Value;

                    //radioButtons Parity: welke is aangevinkt?
                    if (radioButtonParityEven.Checked) serialPortArduino.Parity = Parity.Even;
                    else if (radioButtonParityOdd.Checked) serialPortArduino.Parity = Parity.Odd;
                    else if (radioButtonParityNone.Checked) serialPortArduino.Parity = Parity.None;
                    else if (radioButtonParityMark.Checked) serialPortArduino.Parity = Parity.Mark;
                    else if (radioButtonParitySpace.Checked) serialPortArduino.Parity = Parity.Space;

                    //radioButtons StopBits: welke is aangevinkt?
                    if (radioButtonStopbitsNone.Checked) serialPortArduino.StopBits = StopBits.None;
                    else if (radioButtonStopbitsOne.Checked) serialPortArduino.StopBits = StopBits.One;
                    else if (radioButtonStopbitsOnePointFive.Checked) serialPortArduino.StopBits = StopBits.OnePointFive;
                    else if (radioButtonStopbitsTwo.Checked) serialPortArduino.StopBits = StopBits.Two;


                    //RadioButton Handshake: welke is aangevinkt?
                    if (radioButtonHandshakeNone.Checked) serialPortArduino.Handshake = Handshake.None;
                    else if (radioButtonHandshakeRTS.Checked) serialPortArduino.Handshake = Handshake.RequestToSend;
                    else if (radioButtonHandshakeRTSXonXoff.Checked) serialPortArduino.Handshake = Handshake.RequestToSendXOnXOff;
                    else if (radioButtonHandshakeXonXoff.Checked) serialPortArduino.Handshake = Handshake.XOnXOff;

                    //checkbox RTS
                    serialPortArduino.RtsEnable = checkBoxRtsEnable.Checked;

                    //checkbox DTR
                    serialPortArduino.DtrEnable = checkBoxDtrEnable.Checked;

                    // wat als seriele poort  geopend wordt?
                    serialPortArduino.Open();
                    string commando = "ping";
                    serialPortArduino.WriteLine(commando);
                    string antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    if (antwoord == "pong") //als antwoord pong is; is de verbinding gelegd
                    { radioButtonVerbonden.Checked = true;
                        buttonConnect.Text = "Disconnect";
                        labelStatus.Text = "Status: connected";
                    }
                    else { serialPortArduino.Close(); labelStatus.Text = "Error: verkeerd antwoord"; } //als de arduino niet antwoord


                }  


            }
            catch (Exception uitzondering) 
            { labelStatus.Text = "Error: " + uitzondering.Message; 
                serialPortArduino.Close () ;
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }

        }

        private void checkBoxDigital2_CheckedChanged(object sender, EventArgs e)
        {
            //DIGITALE UITGANG 2 AANGEVINKT
            try  
            {
                if (serialPortArduino.IsOpen)
                {   string commando; //set d2 high/low
                    if (checkBoxDigital2.Checked) commando = "set d2 high";
                    else commando = "set d2 low";
                    serialPortArduino.WriteLine (commando);
                }
            }

            catch (Exception uitzondering)
            {
                labelStatus.Text = "Error: " + uitzondering.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void checkBoxDigital3_CheckedChanged(object sender, EventArgs e)
        {//DIGITALE UITGANG 3 AANGEVINKT
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando; //set d3 high/low
                    if (checkBoxDigital3.Checked) commando = "set d3 high";
                    else commando = "set d3 low";
                    serialPortArduino.WriteLine(commando);
                }
            }

            catch (Exception uitzondering)
            {
                labelStatus.Text = "Error: " + uitzondering.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }

        }

        private void checkBoxDigital4_CheckedChanged(object sender, EventArgs e)
        {// DIGITALE UITGANG 4 AANGEVINKT
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando; //set d4 high/low
                    if (checkBoxDigital4.Checked) commando = "set d4 high";
                    else commando = "set d4 low";
                    serialPortArduino.WriteLine(commando);
                }
            }

            catch (Exception uitzondering)
            {
                labelStatus.Text = "Error: " + uitzondering.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void trackBarPWM9_Scroll(object sender, EventArgs e)
        {
            try
            { 
                if (serialPortArduino.IsOpen) 
                {
                    string commando = String.Format("set pwm9 {0}", trackBarPWM9.Value); //set pwm 9 0...255
                    serialPortArduino.WriteLine (commando);
                }
            }
            catch (Exception uitzondering)
            {
                labelStatus.Text = "Error: " + uitzondering.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void trackBarPWM10_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando = String.Format("set pwm10 {0}", trackBarPWM10.Value); //set pwm 10 0...255
                    serialPortArduino.WriteLine(commando);
                }
            }
            catch (Exception uitzondering)
            {
                labelStatus.Text = "Error: " + uitzondering.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void trackBarPWM11_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando = String.Format("set pwm11 {0}", trackBarPWM11.Value); //set pwm 11 0...255
                    serialPortArduino.WriteLine(commando);
                }
            }
            catch (Exception uitzondering)
            {
                labelStatus.Text = "Error: " + uitzondering.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        //OEFENING 3
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            timerOefening3.Enabled = tabControl.SelectedIndex == 3;
            timerOefening4.Enabled = tabControl.SelectedIndex == 4; 
        }

        private void timerOefening3_Tick(object sender, EventArgs e)
        {
            try 
            { if (serialPortArduino.IsOpen)
                {
                    serialPortArduino.ReadExisting();
                    string commando = "get d5"; //probeer digitale ingang 5
                    serialPortArduino.WriteLine(commando);
                    string antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.Trim();
                    antwoord = antwoord.Substring(4);

                    radioButtonDigital5.Checked = (antwoord == "1");


                    //digitale poort 6
                    serialPortArduino.ReadExisting();
                    string commandod6 = "get d6"; //probeer digitale ingang 6
                    serialPortArduino.WriteLine(commandod6);
                    string antwoordd6 = serialPortArduino.ReadLine();
                    antwoordd6 = antwoordd6.Trim();
                    antwoordd6 = antwoordd6.Substring(4);

                    radioButtonDigital6.Checked = (antwoordd6 == "1");

                    //digitale poort 7
                    serialPortArduino.ReadExisting();
                    string commandod7 = "get d7"; //probeer digitale ingang 7
                    serialPortArduino.WriteLine(commandod7);
                    string antwoordd7 = serialPortArduino.ReadLine();
                    antwoordd7 = antwoordd7.Trim();
                    antwoordd7 = antwoordd7.Substring(4);

                    radioButtonDigital7.Checked = (antwoordd7 == "1");
                }
            }

            catch (Exception uitzondering)
            {
                labelStatus.Text = "Error: " + uitzondering.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }
        
        //OEFENING 4

        private void timerOefening4_Tick(object sender, EventArgs e)
        {

            try
            { if (serialPortArduino.IsOpen)
                  serialPortArduino.ReadExisting();
                  string commando = "get a0"; //probeer analogoe ingang 0
                  serialPortArduino.WriteLine(commando);
                  string antwoord = serialPortArduino.ReadLine();
                  antwoord = antwoord.Trim();
                  antwoord = antwoord.Substring(4);
                labelAnalog0.Text = antwoord;
                      

            }


            catch (Exception uitzondering)
            {
                labelStatus.Text = "Error: " + uitzondering.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }



        }
    }
}
