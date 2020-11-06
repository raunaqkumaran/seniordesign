using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private const int MillisecondsTimeout = 1200;
        private const int SmallTimeout = 100;
        private Boolean loopStop = false;
        public SerialPort arduinoPort;

        public Form1()
        {
            InitializeComponent();
            arduinoPort = new SerialPort();
            arduinoPort.BaudRate = 9600;
            arduinoPort.PortName = "COM4";
            arduinoPort.Open();
            arduinoPort.Close();
        }

        private void startStaticButton(object sender, EventArgs e)
        {
            arduinoPort.Open();
            arduinoPort.WriteLine("START_STATIC");
            double value = Decimal.ToDouble(weightSelectionBox.Value);
            arduinoPort.WriteLine(value.ToString());
            System.Threading.Thread.Sleep(MillisecondsTimeout);
            String result = arduinoPort.ReadLine();
            comLocation.Text = "Center of mass location: " + result;
            System.Threading.Thread.Sleep(SmallTimeout);
            result = arduinoPort.ReadLine();
            offsetLabel.Text = "Required counter balance offset: " + result;
            arduinoPort.Close();
        }

        private void endStaticButton(object sender, EventArgs e)
        {
            arduinoPort.Open();
            arduinoPort.WriteLine("END_STATIC");
            arduinoPort.Close();
        }

        private async void startDynamicButton(object sender, EventArgs e)
        {
            arduinoPort.Open();
            arduinoPort.WriteLine("START_DYNAMIC");
            arduinoPort.WriteLine(omegaBox.Value.ToString());
            Boolean end = true;
            await printDynamic();
            
        }

        private Task printDynamic()
        {
            return Task.Run(() =>
            {
                while (!loopStop)
                {
                    String val = "";
                    try
                    {
                        val = arduinoPort.ReadLine();
                    }
                    catch
                    { }
                    if (val.StartsWith("R"))
                    {
                        val.Remove(0, 1);
                        rotationRadius.Text = val;
                    }
                    if (val.StartsWith("C"))
                    {
                        val.Remove(0, 1);
                        correctionMoment.Text = val;
                    }
                }
            });
        }

        private void endDynamicButton(object sender, EventArgs e)
        {
            loopStop = true;
            arduinoPort.WriteLine("END_DYNAMIC");
            arduinoPort.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void weightSelectionBox_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_3(object sender, EventArgs e)
        {

        }
    }
}
