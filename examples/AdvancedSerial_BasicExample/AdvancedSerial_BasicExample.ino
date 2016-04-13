#include "Arduino.h"
#include "AdvancedSerial.h"

//Instantiate a new advanced serial object
AdvancedSerial AdvSerial(&Serial);

//signals
float sine_value = 0;
float cosine_value = 0;

void setup(){
    //Initialize Serial port
    Serial.begin(9600);

    //Add signals to Advanced Serial which will be transmitted
    AdvSerial.addSignal("sine",&sine_value);
    AdvSerial.addSignal("cosine",&cosine_value);
}
unsigned long lastupdate = 0;

void loop(){
  //Update the calculated cosine and sine values;
  sine_value = 2.0 * sin( 0.5*3.1415*((double)(millis()))/1000);
  cosine_value = cos( 0.25*3.1415*((double)(millis()))/1000);

  //Print an ascii string once per second.
  if((millis() - lastupdate) > 1000){
      lastupdate = millis();
      Serial.print("Test current ms:");
      Serial.println(lastupdate);
  }

  //Execute the advanced serial code
  AdvSerial.exec();
}

