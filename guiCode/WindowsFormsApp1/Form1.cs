using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    //heres a comment
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
            Console.WriteLine("STARTING");
        }

        private void startStaticButton(object sender, EventArgs e)
        {
            arduinoPort.Open();
            arduinoPort.DiscardInBuffer();
            arduinoPort.DiscardOutBuffer();
            arduinoPort.WriteLine("START_STATIC");
            double value = Decimal.ToDouble(weightSelectionBox.Value) * 9.81;
            arduinoPort.WriteLine(value.ToString());
            String result = arduinoPort.ReadLine();
            comLocation.Text = "Center of mass location (m): " + result;
            System.Threading.Thread.Sleep(SmallTimeout);
            result = arduinoPort.ReadLine();
            offsetLabel.Text = "Required counter balance offset (m): " + result;
            arduinoPort.Close();
        }

        private async void startDynamicButton(object sender, EventArgs e)
        {
            arduinoPort.Open();
            arduinoPort.DiscardInBuffer();
            arduinoPort.DiscardOutBuffer();
            staticBalanceButton.Enabled = false;
            dynamicBalanceButton.Enabled = false;
            omegaBox.Enabled = false;
            weightSelectionBox.Enabled = false;
            arduinoPort.WriteLine("START_DYNAMIC");
            arduinoPort.WriteLine(omegaBox.Value.ToString());
            loopStop = false;
            double counterWeight = Decimal.ToDouble(weightSelectionBox.Value) * 9.81;
            arduinoPort.WriteLine(counterWeight.ToString());
            await printDynamic();
            
        }

        private Task printDynamic()
        {
            List<double> rotationRadiusList = new List<double>();
            List<double> correctionMomentList = new List<double>();

            return Task.Run(() =>
            {
                while (!loopStop)
                {
                    String val = "";
                    try
                    {
                        val = arduinoPort.ReadLine();
                        val = val.Replace("\n", "").Replace("\r", "");
                    }
                    catch
                    {
                        Console.WriteLine("Found something");
                    }
                    if (val.StartsWith("R"))
                    {
                        val =  val.Remove(0, 1);
                        double value = Convert.ToDouble(val);
                        if (rotationRadiusList.Count > 100)
                        {
                            rotationRadiusList.RemoveAt(0);
                        }
                        rotationRadiusList.Add(value);
                        double average = rotationRadiusList.Average();
                        average = Math.Round(average, 3);
                        rotationRadius.Invoke((MethodInvoker)delegate
                        {
                            rotationRadius.Text = "Radius of rotation (m): " + average.ToString();
                        });
                    }
                    if (val.StartsWith("C"))
                    {
                        val = val.Remove(0, 1);
                        double value = Convert.ToDouble(val);
                        if (correctionMomentList.Count > 100)
                        {
                            correctionMomentList.RemoveAt(0);
                        }
                        correctionMomentList.Add(value);
                        double average = correctionMomentList.Average();
                        average = Math.Round(average, 3);
                        correctionMoment.Invoke((MethodInvoker)delegate
                        {
                            correctionMoment.Text = "Counterbalance correction (m): " + average.ToString();
                        });
                    }
                }
                arduinoPort.Close();
            });
        }

        private void endDynamicButton(object sender, EventArgs e)
        {
            loopStop = true;
            staticBalanceButton.Enabled = true;
            dynamicBalanceButton.Enabled = true;
            weightSelectionBox.Enabled = true;
            omegaBox.Enabled = true;

            if (arduinoPort.IsOpen)
            {
                arduinoPort.WriteLine("END_DYNAMIC");
                arduinoPort.DiscardInBuffer();
                arduinoPort.DiscardOutBuffer();
            }
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

        private void label1_Click_4(object sender, EventArgs e)
        {

        }

        private void portBox(object sender, EventArgs e)
        {
            arduinoPort.PortName = "COM" + portSelector.Value.ToString();
        }
    }
}
