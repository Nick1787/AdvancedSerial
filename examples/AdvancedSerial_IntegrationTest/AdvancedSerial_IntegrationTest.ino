#include "Arduino.h"
#include "HardwareSerial.h"
#include "HardwareSerial_private.h"
#include "AdvancedSerial.h"

AdvancedSerial ASerial(&Serial);

int counter = 0;
unsigned long timems = 0;
unsigned long curtime = 0;
unsigned long writems = 0;

bool squarewv = 0;
float sinValue = 0;
double cosValue = 0;
float A0_data = 0;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  Serial.print("Testing ASI...");
  ASerial.addSymbol("Counter-int",&counter);
  ASerial.addSymbol("time_ms-long",&timems);
  ASerial.addSymbol("sine-float",&sinValue);
  ASerial.addSymbol("cos-double",&cosValue);
  ASerial.addSymbol("square-bool",&squarewv);
  ASerial.addSymbol("Analog0-float",&A0_data);
  ASerial.TransmitSymbolList(123);
}


void loop() {
  curtime = millis();

  //Update A0 Read Data
  int sensorValue = analogRead(A0);
  A0_data = sensorValue * (5.0 / 1023.0);
  
  //Update Test Data
  sinValue = 2.0 * sin( 0.5*3.1415*((double)(curtime))/1000);
  cosValue = (double)(1.0 * cos( 0.25*3.1415*((double)(curtime))/1000));

  //Call Advanced SErial Executive
  ASerial.exec();
  
  // put your main code here, to run repeatedly:
  if((curtime - timems) > 1000){
      timems = curtime;
      counter = counter + 1;
      Serial.print("Printing Comments Test...");
      Serial.println(counter);
      if(squarewv==1){
        squarewv=0;
      }else{
        squarewv=1;
      }
  }

  if((curtime-writems)>100){
      writems = curtime;
      ASerial.TransmitSymbolData(123);
  }
}
