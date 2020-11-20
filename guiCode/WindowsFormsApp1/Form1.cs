﻿using System;
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

            //Define port characteristisc to communicate with the xbee. Defaults to COM5 but can be changed in the GUI to another port. 
            //GUI does not check which ports are available, allows the user to enter in any port number. 
            //The user needs to figure out which port the xbee is connected too. This can be determined when connecting to the xbee using the XCTU utility.
            arduinoPort = new SerialPort();
            arduinoPort.BaudRate = 9600;
            arduinoPort.PortName = "COM5";

            //Default calibration values. These too, can be changed through the GUI. These are deterined by finding a zero offset for each balance, and then finding a calibration factor against a 2.5 lbs weight
            Calibration1.Value = 143550;
            Offset1.Value = -24496;
            Calibration2.Value = 143550;
            Offset2.Value = 159207;
            Calibration3.Value = 147870;
            Offset3.Value = 163607;
            Calibration4.Value = 149378;
            Offset4.Value = -20886;

            //Set calibration factors and write something to the Console. This won't be visible unless there actually is a console window open. 
            applyCalibrationFactors();
            Console.WriteLine("STARTING");
        }

        //This reuns whenever the "Start static" button is pressed in the GUI. 
        private void startStaticButton(object sender, EventArgs e)
        {
            arduinoPort.Open();                 //This will throw an error if the port is already open in another utility, or if the port number was incorrectly set. 
            arduinoPort.DiscardInBuffer();      //Discarding buffers is a quick and easy (but probably not "best practice") to clear any characters still in the port buffer as a result of unprocessed messages from the C# to the xbee, or from the xbee to the C#. Makes sure things happen in a predictable and sequential manner. 
            arduinoPort.DiscardOutBuffer();
            arduinoPort.WriteLine("START_STATIC");      //All communication between the GUI and the xbee (thus the arduino) is conducted by writing strings to the port. "START_STATIC" tells the arduino to start sampling load cells for a static balancing calculation
            double value = Decimal.ToDouble(weightSelectionBox.Value);      //Parse the value of the counterbalance, so a correction can be determined. Only looking at correction in the x direction.       
            arduinoPort.WriteLine(value.ToString());                        //Again, all communication is through writing strings to the port buffer. This writes the counterbalance weight to the arduino. 
            String result = arduinoPort.ReadLine();                         //Bring in data back from the arduino through the serial port (the arduino writes to the port via the xbee)
            comLocation.Text = "Center of mass location (in): " + result;
            System.Threading.Thread.Sleep(SmallTimeout);                    //Wait a little bit to let things settle before writing to the GUI. Not entirely sure if this is necessary, or helpful. 
            result = arduinoPort.ReadLine();
            offsetLabel.Text = "Required counter balance offset (in): " + result;
            arduinoPort.Close();
        }

        //Same logic as the start static, except now for dynamic. This button has to run asynchronously, to make sure the endDynamic button is still enabled. I don't reaallllly know how this async/await construct works. This should help https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/ 
        private async void startDynamicButton(object sender, EventArgs e)
        {
            arduinoPort.Open();
            arduinoPort.DiscardInBuffer();
            arduinoPort.DiscardOutBuffer();
            staticBalanceButton.Enabled = false;
            dynamicBalanceButton.Enabled = false;
            omegaBox.Enabled = false;
            weightSelectionBox.Enabled = false;
            applyCalibration.Enabled = false;
            arduinoPort.WriteLine("START_DYNAMIC");
            arduinoPort.WriteLine(omegaBox.Value.ToString());
            loopStop = false;
            double counterWeight = Decimal.ToDouble(weightSelectionBox.Value);
            arduinoPort.WriteLine(counterWeight.ToString());
            await printDynamic();               //Need this to make sure print dynamic keeps running, but without disabling the rest of the UI (which is what would happen otherwise).
            
        }

        private Task printDynamic()
        {
            //Use these lists to run a moving average with a 100 sample window. 
            List<double> rotationRadiusList = new List<double>();
            List<double> correctionMomentList = new List<double>();

            return Task.Run(() =>
            {
                while (!loopStop)
                {
                    String val = "";
                    try
                    {
                        val = arduinoPort.ReadLine();               //Val should be in the format C[NUMBER] or R[NUMBER]. One indicates a correction, the other indicates a radius of rotation. 
                        val = val.Replace("\n", "").Replace("\r", "");      //Strip out carriage returns that might've been sent with the val string. 
                    }
                    catch
                    {
                        Console.WriteLine("Found something");           //Print this to the console if the val string is not what was expected. 
                    }
                    if (val.StartsWith("R"))                            //If the string starts with R, it's a "radius of rotation". 
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
                        rotationRadius.Invoke((MethodInvoker)delegate       //.NET doesn't allow the UI to be modified in any thread except the main UI thread. The invokation pushes up this specific method call up to the calling thread (the UI thread)
                        {
                            rotationRadius.Text = "Radius of rotation (m): " + average.ToString();
                        });
                    }
                    if (val.StartsWith("C"))                            //If the string starts with a C, it's a correction. 
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
                            correctionMoment.Text = "Counterbalance correction (in): " + average.ToString();
                        });
                    }
                }
                arduinoPort.Close();
            });
        }

        //Ends the dynamic balancing step. Sends a "END_DYNAMIC" string to the arduino, telling the arduino to stop calculating dynamic balancing values. 
        private void endDynamicButton(object sender, EventArgs e)
        {
            loopStop = true;
            staticBalanceButton.Enabled = true;
            dynamicBalanceButton.Enabled = true;
            weightSelectionBox.Enabled = true;
            omegaBox.Enabled = true;
            applyCalibration.Enabled = true;

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

        //Resets the arduino port name if the port selection box in the GUI is modified. 
        private void portBox(object sender, EventArgs e)
        {
            arduinoPort.PortName = "COM" + portSelector.Value.ToString();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        //Apply new calibration factors if the UI button is pressed. 
        private void applyCalibration_Click(object sender, EventArgs e)
        {
            applyCalibrationFactors();
        }

        //Send new calibration factors to the arduino. This is run both at the beginning of the programme, as well as when the recalibrate button is pressed. 
        private void applyCalibrationFactors()
        {
            arduinoPort.Open();
            arduinoPort.DiscardInBuffer(); arduinoPort.DiscardOutBuffer();
            arduinoPort.WriteLine("RECALIBRATE");
            arduinoPort.WriteLine(Calibration1.Value.ToString());
            arduinoPort.WriteLine(Offset1.Value.ToString());
            arduinoPort.WriteLine(Calibration2.Value.ToString());
            arduinoPort.WriteLine(Offset2.Value.ToString());
            arduinoPort.WriteLine(Calibration3.Value.ToString());
            arduinoPort.WriteLine(Offset3.Value.ToString());
            arduinoPort.WriteLine(Calibration4.Value.ToString());
            arduinoPort.WriteLine(Offset4.Value.ToString());
            arduinoPort.Close();
        }
    }
}
