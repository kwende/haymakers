/*
 * IRremote: IRsendDemo - demonstrates sending IR codes with IRsend
 * An IR LED must be connected to Arduino PWM pin 3.
 * Version 0.1 July, 2009
 * Copyright 2009 Ken Shirriff
 * http://arcfn.com
 */

#include <IRremote.h>

IRsend irsend;

void setup()
{
  Serial.begin(9600);
}

void loop() {

  char buffer[2]; 
  if(Serial.readBytes(buffer, 2)==2) 
  {
    int value = (byte)buffer[0]; 
    value <<= 8; 
    value |= (byte)buffer[1]; 
    
    for (int i = 0; i < 3; i++) {
      irsend.sendSony(value, 12); // Sony TV power code
      delay(100);
    } 
    
    Serial.println(value, HEX);
  }
}
