#include "support.h"  //Standard C++ code for support functions (easier to develop in VS than in Arduino's atrocious IDE)

//Library includes and initializations for most of the sensors we're using
#include <HX711.h>
#include <Wire.h>
#include <SPI.h>
#include <String.h>
#include <Adafruit_LIS3DH.h>
#include <Adafruit_Sensor.h>

#define LIS3DH_CLK 13
#define LIS3DH_MISO 12
#define LIS3DH_MOSI 11
#define LIS3DH_CS 10
Adafruit_LIS3DH lis = Adafruit_LIS3DH(LIS3DH_CS, LIS3DH_MOSI, LIS3DH_MISO, LIS3DH_CLK);

double totalForce; //Sum of all forces
double counterWeight = 5; //Weight of counterweight (Units don't matter here, just be consistent)

//Parameters for load cell 1
int DOUT = 2;
int CLK = 3;
double location1[] = {5, 0, 0};
coordinates scale1;

//Parameters for load cell 2
int DOUT2 = 6;
int CLK2 = 7;
double location2[] = {-5, 0, 0};
coordinates scale2;

//Parameters for load cell 3
int DOUT3 = 13;
int CLK3 = 14;
double location3[] = {-5, 0, 0};
coordinates scale3;

//Parameters for load cell 4
int DOUT4 = 18;
int CLK4 = 19;
double location4[] = {-5, 0, 0};
coordinates scale4;

coordinates correction;
double forces[4];
coordinates locations[] = {coordFromArray(location1), coordFromArray(location2), coordFromArray(location3),
                           coordFromArray(location4)};
double omega;

HX711 loadCell_1, loadCell_2, loadCell_3, loadCell_4;
double calibration_1 = 1100;
double calibration_2 = 1089;
double calibration_3 = 1200;
double calibration_4 = 1500;
double offset1 = 100;
double offset2 = 100;
double offset3 = 100;
double offset4 = 100;
double sampleTime = 0.5;
coordinates com;

void setup() {
    Serial.begin(9600);
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

    Serial.println("LIS3DH test!");

    if (!lis.begin(0x18)) {   // change this to 0x19 for alternative i2c address
        Serial.println("Couldnt start");
        while (1) yield();
    }
    Serial.println("LIS3DH found!");

    Serial.print("Range = ");
    Serial.print(2 << lis.getRange());
    Serial.println("G");
    lis.getDataRate();
}

void setupScales(HX711 &loadCell, int dout, int clk, double calibrationFactor, double cellOffset) {
    loadCell.begin(dout, clk);
    loadCell.set_scale(calibrationFactor);
    loadCell.set_offset(cellOffset);
    loadCell.read_average();
}

String readString()
{
    String readString = "";
    if (Serial.available()) {
        while (Serial.available() > 0) {
            char temp = Serial.read();
            if (temp == '\n') {
                break;
            }
            readString += temp;
        }
    }
    return readString;
}

void staticBalancing() {
    counterWeight = readString().toDouble();
    forces[0] = getLoading(loadCell_1);
    forces[1] = getLoading(loadCell_2);
    forces[2] = getLoading(loadCell_3);
    forces[3] = getLoading(loadCell_4);
    totalForce = sumArr(forces, 4);
    com = comLocation(forces, locations, 4);
    printCoord(com, false, 0);
    coordinates correction = correctionMoment(totalForce, com);
    Serial.print(correction.magnitude / counterWeight);
}

void loop() {
    sensors_event_t event;
    lis.getEvent(&event);

    String readstring = readString();
        if (readstring == "START_STATIC") {
            staticBalancing();
        }
        if (readstring == "START_DYNAMIC") {
            omega = readString().toDouble();
            do {
                dynamicMoment(omega);
            }while(readString() != "END_DYNAMIC");
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

double dynamicMoment(double omega) {
    double forces[3];
    forces[0] = getLoading(loadCell_1);
    forces[1] = getLoading(loadCell_2);
    forces[2] = getLoading(loadCell_3);
    forces[3] = getLoading(loadCell_4);
    double totalForce = sumArr(forces, 4);
    coordinates com = comLocation(forces, locations, 4);
    coordinates correction = correctionMoment(totalForce, com);
    double momentMagnitude = correction.magnitude;
    //Serial.print("\nDynamic moment: ");
    if (correction.x > 0)
        Serial.println("C"+String(-momentMagnitude));
    else
        Serial.println("C"+String(momentMagnitude));
    sensors_event_t event;
    lis.getEvent(&event);
    //Serial.print("\nRadius of rotation: ");
    dynamicBalancing(event, omega);
}

void dynamicBalancing(sensors_event_t event, double omega) {
    double radius = radiusOfRotation(omega, event.acceleration.x);
    Serial.println("R"+String(radius));
}

double getLoading(HX711 scale) {
    double reading = scale.get_units(15);
    return reading;
}

void printCoord(coordinates coord, boolean mag, double counterWeight) {
    String str;
    //Serial.print("\nx: "); Serial.print(coord.x); Serial.print("\ty: "); Serial.print(coord.y); Serial.print("\tz: "); Serial.print(coord.z);
    if (mag) {
        //Serial.print("\tdistance: "); Serial.print(coord.magnitude / counterWeight);
    }
    //Serial.print("\n");
    //Serial.print("COM_LOCATION");
    str = "x: " + String(coord.x);
    Serial.println(str);
}

//Should be able to ignore this whole function. Here for posterity.

void loop2() {
    // put your main code here, to run repeatedly:
    sensors_event_t event;
    lis.getEvent(&event);
    coordinates com;

    if (Serial.available()) {
        while (Serial.available()) {
            Serial.read();
        }
        printAccelerometer(event);
        double radius = radiusOfRotation(omega, event.acceleration.x);
        Serial.print("Radius of rotation: ");
        Serial.print(radius);
        forces[0] = getLoading(loadCell_1);
        Serial.print("\nLoading on scale 1: ");
        Serial.print(forces[0]);
        forces[1] = getLoading(loadCell_2);
        Serial.print("\nLoading on scale 2: ");
        Serial.print(forces[1]);
        forces[2] = getLoading(loadCell_3);
        Serial.print("\nLoading on scale 3: ");
        Serial.print(forces[2]);
        forces[3] = getLoading(loadCell_4);
        Serial.print("\nLoading on scale 4: ");
        Serial.print(forces[3]);
        Serial.print("\nLocation of COM: ");
        com = comLocation(forces, locations, 4);
        printCoord(com, false, 0);
        totalForce = sumArr(forces, 4);
        correction = correctionMoment(totalForce, com);
        Serial.print("\nRequired correction: ");
        printCoord(correction, true, counterWeight);
        Serial.print("\n\n\n");
    }
    delay(100);
}
