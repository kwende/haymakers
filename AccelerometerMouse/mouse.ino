const int xPin = 8; // X input from accelerometer
const int yPin = 9; // Y input accelerometer

int xPulse, yPulse; // read from accelerometer
int xVal, yVal; // converted value

void setup() {
Serial.begin(9600);
  pinMode(xPin, INPUT);
  pinMode(yPin, INPUT);
  pinMode(2, INPUT); 
}

void loop() {
  
  int buttonState =  digitalRead(2); 

  // read x &amp; y values
  xPulse = pulseIn(xPin,HIGH);
  yPulse = pulseIn(yPin,HIGH);
  
  xVal = ((xPulse / 10) - 500) * 8;
  yVal = ((yPulse / 10) - 500) * 8;
  
  Serial.print(buttonState);
  Serial.print('\t');
  Serial.print(xVal);
  Serial.print("\t");
  Serial.print(yVal);
  Serial.println(); 
   
  
  delay(10);
}
