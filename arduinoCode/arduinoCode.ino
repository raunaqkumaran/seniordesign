#include "support.h"  //Standard C++ code for support functions (easier to develop in VS than in Arduino's atrocious IDE)

//Library includes and initializations for most of the sensors we're using
#include <HX711.h>
#include <Wire.h>
#include <SPI.h>
#include <String.h>
#include <Adafruit_LIS3DH.h>
#include <Adafruit_Sensor.h>
#include <SoftwareSerial.h>

//Define pin numbers for the LIS3DH accelerometer
#define LIS3DH_CLK 22
#define LIS3DH_MISO 24
#define LIS3DH_MOSI 26
#define LIS3DH_CS 28
Adafruit_LIS3DH lis = Adafruit_LIS3DH(LIS3DH_CS, LIS3DH_MOSI, LIS3DH_MISO, LIS3DH_CLK);

double totalForce; //Sum of all forces
double counterWeight = 5; //Weight of counterweight (Units don't matter here, just be consistent)

//Parameters for load cell 1
int DOUT = 14;
int CLK = 15;
double location1[] = {-0.27305, -0.1651, 0};
coordinates scale1;

//Parameters for load cell 2
int DOUT2 = 16;
int CLK2 = 17;
double location2[] = {-0.27305, 0.1651, 0};
coordinates scale2;

//Parameters for load cell 3
int DOUT3 = 18;
int CLK3 = 19;
double location3[] = {0.27305, -0.1651, 0};
coordinates scale3;

//Parameters for load cell 4
int DOUT4 = 20;
int CLK4 = 21;
double location4[] = {0.27305, 0.1651, 0};
coordinates scale4;

coordinates correction;
double forces[4];
coordinates locations[] = {coordFromArray(location1), coordFromArray(location2), coordFromArray(location3),
                           coordFromArray(location4)};
double omega;

HX711 loadCell_1, loadCell_2, loadCell_3, loadCell_4;

//Default offsets and calibration factors. These are overriden by whatever is passed by the GUI
double calibration_1 = 145000;
double calibration_2 = 145000;
double calibration_3 = 145000;
double calibration_4 = 145000;
double offset1 = 7840;
double offset2 = 7840;
double offset3 = 7840;
double offset4 = 7840;
double sampleTime = 0.5;
coordinates com;
SoftwareSerial xbee(10, 11);

void setup() {
    Serial.begin(9600);
    xbee.begin(9600);
    initializeAccelerometer(lis);
    setupScales(loadCell_1, DOUT, CLK, calibration_1, offset1);
    scale1 = coordFromArray(location1);
    setupScales(loadCell_2, DOUT2, CLK2, calibration_2, offset2);
    scale2 = coordFromArray(location2);
    setupScales(loadCell_3, DOUT3, CLK3, calibration_3, offset3);
    scale3 = coordFromArray(location3);
    setupScales(loadCell_4, DOUT4, CLK4, calibration_4, offset4);
    scale4 = coordFromArray(location4);
}

void initializeAccelerometer(Adafruit_LIS3DH &lis) {
    while (!Serial) delay(10);     // will pause Zero, Leonardo, etc until serial console opens

    //Serial.println("LIS3DH test!");

    if (!lis.begin(0x18)) {   // change this to 0x19 for alternative i2c address
        Serial.println("Couldn't start");
        xbee.println("Couldn't start");
        while (1) yield();
    }
    Serial.println("LIS3DH found!");

    //Serial.print("Range = ");
    //Serial.print(2 << lis.getRange());
    //Serial.println("G");
    lis.getDataRate();
}

//Initializes each load cell.
void setupScales(HX711 &loadCell, int dout, int clk, double calibrationFactor, double cellOffset) {
    loadCell.begin(dout, clk);
    loadCell.set_scale(calibrationFactor);
    loadCell.set_offset(cellOffset);        //Can replace this with tare() if you want to tare and start from a zero-weight every time, or add in a tare button
    loadCell.read_average();
    Serial.println("Scales calibrated!");
}

//Arduino doesn't have a very logical way to read entire strings from a Serial port. This does that.
//All communication between the user interface and the arduino is by sending strings back and forth. The format of the string, and the order of the strings defines what the quantity is.
String readString()
{
    String readString = "";
    if (xbee.available()) {
        while (xbee.available() > 0) {
            char temp = xbee.read();
            if (temp == '\n') {
                break;
            }
            readString += temp;
            delay(10);
        }
    }
    Serial.print(readString);
    return readString;
}

//Runs static balancing. Pretty straightforward sum of moments to find a center of mass location, and a correction required to move CoM back to (0, 0)
void staticBalancing(double counterWeight) {
    forces[0] = getLoading(loadCell_1, 15);
    forces[1] = getLoading(loadCell_2, 15);
    forces[2] = getLoading(loadCell_3, 15);
    forces[3] = getLoading(loadCell_4, 15);
    totalForce = sumArr(forces, 4);
    com = comLocation(forces, locations, 4);
    printCoord(com, false, 0);
    coordinates correction = correctionMoment(totalForce, com);   //Calculates the required correction to maintain a CoM at 0,0,0
    if (counterWeight == 0)
    {
      counterWeight = 0.0001;
    }
    xbee.print(39.37 * correction.magnitude / counterWeight); xbee.print("\n"); //Magic constant to convert meters to inches
    Serial.print(correction.magnitude / counterWeight); Serial.print("\n");
}

void loop() {
    sensors_event_t event;
    lis.getEvent(&event);

    String readstring = readString();
    //Every time the GUI wants the arduino to do something, it will send a string. This if ladder checks for that string, and reacts accordingly
    if (readstring.length() > 0)
      //Serial.println(readstring);
        if (readstring == "START_STATIC") {
            counterWeight = readString().toDouble();
            staticBalancing(counterWeight);
            xbee.flush();
            Serial.flush();
        }
        if (readstring == "START_DYNAMIC") {
            omega = readString().toDouble();
            counterWeight = readString().toDouble();
            do {
                dynamicMoment(omega, counterWeight);
            }while(readString() != "END_DYNAMIC");
            xbee.flush();
            Serial.flush();
        }
        if (readstring == "RECALIBRATE")
        {
          calibration_1 = readString().toDouble();
          offset1 = readString().toDouble();
          calibration_2 = readString().toDouble();
          offset2 = readString().toDouble();
          calibration_3 = readString().toDouble();
          offset3 = readString().toDouble();
          calibration_4 = readString().toDouble();
          offset4 = readString().toDouble();
          
          setupScales(loadCell_1, DOUT, CLK, calibration_1, offset1);
          setupScales(loadCell_2, DOUT2, CLK2, calibration_2, offset2);
          setupScales(loadCell_3, DOUT3, CLK3, calibration_3, offset3);
          setupScales(loadCell_4, DOUT4, CLK4, calibration_4, offset4);
          xbee.flush();
          Serial.flush();
        }
    }

void printAccelerometer(sensors_event_t event) {
    Serial.print("\n\n----NEW TEST----\n\n");
    /* Display the results (acceleration is measured in m/s^2) */
    Serial.print("\nX: ");
    Serial.print(event.acceleration.x);
    Serial.print(" \tY: ");
    Serial.print(event.acceleration.y);
    Serial.print(" \tZ: ");
    Serial.print(event.acceleration.z);
    Serial.println(" m/s^2 ");
    Serial.println();

}

//Gets a load cell reading
double getLoading(HX711 scale, int samples) {
    double reading = scale.get_units(samples);
    return reading;
}

//Uses another sum of moments to calculate if there is any residual moment. This is basically the same exact calculation process as the static balancing.
//"Radius of rotation" should be a constant value if there are no vibrations or precessions, as the accelerometer should be a constant distance away from the rotational axis of the system
double dynamicMoment(double omega, double counterWeight) {
    double forces[3];
    delay(10);
    if (counterWeight == 0)
    {
      counterWeight = 0.001;
    }
    forces[0] = getLoading(loadCell_1, 1);
    forces[1] = getLoading(loadCell_2, 1);
    forces[2] = getLoading(loadCell_3, 1);
    forces[3] = getLoading(loadCell_4, 1);
    double totalForce = sumArr(forces, 4);
    coordinates com = comLocation(forces, locations, 4);
    coordinates correction = correctionMoment(totalForce, com);
    double momentMagnitude = correction.magnitude;
    //Serial.print("\nDynamic moment: ");
    if (correction.x > 0){
        xbee.println("C"+String(39.37 * -momentMagnitude / counterWeight));
        Serial.println("C"+String(39.37 * -momentMagnitude / counterWeight));
    }
    else{
        xbee.println("C"+String(39.37 * momentMagnitude / counterWeight));
        Serial.println("C"+String(39.37 * momentMagnitude / counterWeight));
    }
    sensors_event_t event;
    lis.getEvent(&event);
    //Serial.print("\nRadius of rotation: ");
    dynamicBalancing(event, omega);
}

void dynamicBalancing(sensors_event_t event, double omega) {
    double radius = 39.37 * radiusOfRotation(omega, event.acceleration.x);
    xbee.println("R"+String(radius));
    Serial.println("R"+String(radius));
}

void printCoord(coordinates coord, boolean mag, double counterWeight) {
    String str;
    //Serial.print("\nx: "); Serial.print(coord.x); Serial.print("\ty: "); Serial.print(coord.y); Serial.print("\tz: "); Serial.print(coord.z);
    if (mag) {
        //Serial.print("\tdistance: "); Serial.print(coord.magnitude / counterWeight);
    }
    //Serial.print("\n");
    //Serial.print("COM_LOCATION");
    str = "x: " + String(39.37 * coord.x) + " y:" + String(39.37 * coord.y);
    xbee.println(str);
    Serial.println(str);
}
