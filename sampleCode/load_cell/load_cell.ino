#include <HX711.h>

/*
 Example using the SparkFun HX711 breakout board with a scale
 By: Nathan Seidle
 SparkFun Electronics
 Date: November 19th, 2014
 License: This code is public domain but you buy me a beer if you use this and we meet someday (Beerware license).

 This example demonstrates basic scale output. See the calibration sketch to get the calibration_factor for your
 specific load cell setup.

 This example code uses bogde's excellent library: https://github.com/bogde/HX711
 bogde's library is released under a GNU GENERAL PUBLIC LICENSE

 The HX711 does one thing well: read load cells. The breakout board is compatible with any wheat-stone bridge
 based load cell which should allow a user to measure everything from a few grams to tens of tons.
 Arduino pin 2 -> HX711 CLK
 3 -> DAT
 5V -> VCC
 GND -> GND

 The HX711 board can be powered from 2.7V to 5V so the Arduino 5V power should be fine.

*/


#define calibration_factor -4000.0 //This value is obtained using the SparkFun_HX711_Calibration sketch

#define DOUT  2
#define CLK  3

double sampleTime = 0.5;     //How many seconds do you want to sample before returning a value?

HX711 scale;

void setup() {
  Serial.begin(9600);
  Serial.println("HX711 scale demo");

  scale.begin(DOUT, CLK);
  scale.set_scale(calibration_factor); //This value is obtained by using the SparkFun_HX711_Calibration sketch
  scale.tare(); //Assuming there is no weight on the scale at start up, reset the scale to 0

  Serial.println("Readings:");
  delay(100);
}

void loop() {
  Serial.print("Reading: ");
  Serial.print(getValue(), 1); //scale.get_units() returns a float
  Serial.print(" lbs"); //You can change this to kg but you'll need to refactor the calibration_factor
  Serial.println();
}

double getValue()
{
  double startTime = millis();
  double currentTime = startTime;
  double totalValue = 0;
  int samples = 0;
  double deltaTime = 0;
  while (deltaTime < sampleTime)
  {
    deltaTime = 0.001 * (millis() - startTime);
    totalValue += scale.get_units();
    samples++;
  }

  return totalValue / samples;
}
