#include <RF24Network.h>
#include <RF24.h>
#include <SPI.h>

RF24 radio(9, 10);
RF24Network network(radio);
const uint16_t this_node = 1;
const uint16_t other_node = 0;

struct dataStruct {
  unsigned int nodeID;
  unsigned int state;
};

const int ledPin = A3;

void setup() {
  pinMode(ledPin, OUTPUT);  
  digitalWrite(ledPin, HIGH);

  SPI.begin();
  radio.begin();
  network.begin(90,this_node);
  randomSeed(analogRead(0));
}

long time;
boolean set = false;

void loop() {
  network.update();
  digitalWrite(ledPin, HIGH);

  if(set){
    time = millis();
    set = false;
  }
  if(millis()-time >= 10000) {
    randTemperature();
    set = true;
  }
}

void randTemperature() {
  int randTemp;
  randTemp = random(18,22);
  dataStruct data = { this_node, randTemp};
  RF24NetworkHeader header(/*to node*/ other_node);
  bool ok = network.write(header, &data, sizeof(data));
  if (ok){
    digitalWrite(ledPin, LOW);
    delay(500);
  }
}


