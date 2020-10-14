#include "support.h"

#include <HX711.h>
#include <Wire.h>
#include <SPI.h>
#include <Adafruit_LIS3DH.h>
#include <Adafruit_Sensor.h>

#define LIS3DH_CLK 13
#define LIS3DH_MISO 12
#define LIS3DH_MOSI 11
#define LIS3DH_CS 10
Adafruit_LIS3DH lis = Adafruit_LIS3DH(LIS3DH_CS, LIS3DH_MOSI, LIS3DH_MISO, LIS3DH_CLK);

int DOUT = 2;
int CLK = 3;
double location1[] = {5, 0, 0};
double totalForce;
coordinates scale1;
int DOUT2 = 6;
int CLK2 = 7;
double location2[] = {-5, 0, 0};
coordinates scale2;
coordinates correction;
double forces[2];
coordinates locations[2];
double omega = 0.104719755;

HX711 loadCell_1;
double calibration_1 = 1100;
double calibration_2 = 1089;
HX711 loadCell_2;
double sampleTime = 0.5;

void setup() {
  Serial.begin(9600);
  initializeAccelerometer(lis);
  setupScales(loadCell_1, DOUT, CLK, calibration_1);
  scale1 = coordFromArray(location1);
  setupScales(loadCell_2, DOUT2, CLK2, calibration_2);
  scale2 = coordFromArray(location2);
  locations[0] = scale1; locations[1] = scale2;
}

void initializeAccelerometer(Adafruit_LIS3DH &lis)
{
  while (!Serial) delay(10);     // will pause Zero, Leonardo, etc until serial console opens

  Serial.println("LIS3DH test!");

  if (! lis.begin(0x18)) {   // change this to 0x19 for alternative i2c address
    Serial.println("Couldnt start");
    while (1) yield();
  }
  Serial.println("LIS3DH found!");

  Serial.print("Range = "); Serial.print(2 << lis.getRange());
  Serial.println("G");
  lis.getDataRate();
}

void setupScales(HX711 &loadCell, int dout, int clk, double calibrationFactor)
{
  loadCell.begin(dout, clk);
  loadCell.set_scale(calibrationFactor);
  loadCell.tare();
  loadCell.read_average();
}

void loop() {
  // put your main code here, to run repeatedly:
  sensors_event_t event;
  lis.getEvent(&event);
  coordinates com;

  if (Serial.available())
  {
    while (Serial.available())
    {
    Serial.read();
    }
    printAccelerometer(event); 
    double radius = radiusOfRotation(omega, event.acceleration.x);
    Serial.print("Radius of rotation: "); Serial.print(radius);
    forces[0] = getLoading(loadCell_1);
    Serial.print("\nLoading on scale 1: "); Serial.print(forces[0]);
    forces[1] = getLoading(loadCell_2);
    Serial.print("\nLoading on scale 2: "); Serial.print(forces[1]);
    Serial.print("\n\nLocation of cell 1: ");
    printCoord(locations[0], false);
    Serial.print("\nLocation of cell 2: ");
    printCoord(locations[1], false);
    com = comLocation(forces, locations, 2);
    Serial.print("\nLocation of COM: ");
    printCoord(com, false);
    totalForce = sumArr(forces, 2);
    correction = correctionMoment(totalForce, com);
    Serial.print("\nRequired correction: ");
    printCoord(correction, true);
    Serial.print("\n\n\n");
  }
  delay(100);
}

void printAccelerometer(sensors_event_t event)
{
    Serial.print("\n\n----NEW TEST----\n\n");
  /* Display the results (acceleration is measured in m/s^2) */
    Serial.print("\nX: "); Serial.print(event.acceleration.x);
    Serial.print(" \tY: "); Serial.print(event.acceleration.y);
    Serial.print(" \tZ: "); Serial.print(event.acceleration.z);
    Serial.println(" m/s^2 ");
    Serial.println();

}

double getLoading(HX711 scale)
{
  double reading = scale.get_units();
  return reading;
}

void printCoord(coordinates coord, boolean mag)
{
  Serial.print("\nx: "); Serial.print(coord.x); Serial.print("\ty: "); Serial.print(coord.y); Serial.print("\tz: "); Serial.print(coord.z);
  if (mag)
  {
    Serial.print("\tmag: "); Serial.print(coord.magnitude);
  }
  Serial.print("\n");
}
