#include <RF24Network.h>
#include <RF24.h>
#include <SPI.h>

RF24 radio(9, 10);
RF24Network network(radio);
const uint16_t this_node = 3;
const uint16_t other_node = 0;

struct dataStruct {
  unsigned int nodeID;
  unsigned int state;
};

const int ledPin = 3;

void setup() {
  Serial.begin(57600);
  pinMode(ledPin, OUTPUT);
  analogWrite(ledPin,255);
  SPI.begin();
  radio.begin();
  network.begin(90,this_node);
}

void loop() {
  network.update();

  while ( network.available() ) {
    RF24NetworkHeader header;
    dataStruct data;
    network.read(header, &data, sizeof(data));
    Serial.print("DOSTALEM: ");
    Serial.println(data.state);
    analogWrite(ledPin, data.state);
  }
}




