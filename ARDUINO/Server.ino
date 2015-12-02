#include <RF24Network.h>
#include <RF24.h>
#include <SPI.h>

RF24 radio(9, 10);
RF24Network network(radio);
const uint16_t this_node = 0;
const uint16_t temp_node = 1;
const uint16_t dimmer_node = 3;
const uint16_t switch_node = 5;

struct dataStruct {
  unsigned int nodeID;
  unsigned int state;
};

dataStruct input;
byte *start = (byte*)&input;

// -------------------- SETUP --------------------
void setup() {
  Serial.begin(57600);
  SPI.begin();
  radio.begin();
  network.begin(90, this_node);
}

// -------------------- LOOP --------------------

void loop() {
  network.update();

  while ( network.available() ) {
    getTemperature();
  }

  while( Serial.available() ) {
    *(start++) = Serial.read();

    if(start >= (byte*)&input + sizeof(input) ){
      start = (byte*)&input;
      
      if(input.nodeID == dimmer_node){
        sendToDimmer(input.state);
      } 
      if(input.nodeID == switch_node){
        sendToSwitch(input.state);
      }
    }
  }
}

// --------------- GET TEMPERATURE --------------------
void getTemperature(){
  RF24NetworkHeader header;
  dataStruct data;
  network.read(header, &data, sizeof(data));
  Serial.write((char*)&data, sizeof(data));
}

// -------------- SEND DIMMER--------------------
void sendToDimmer(unsigned int state){
  RF24NetworkHeader header(dimmer_node);
  dataStruct dimmerData = { this_node, state     };
  bool ok = network.write(header, &dimmerData, sizeof(dimmerData));
  if (ok) {
  } 
  else {
  }
}

// ---------------- SEND SWITCH --------------------
void sendToSwitch(unsigned int state){
  RF24NetworkHeader header(switch_node);
  dataStruct switchData = { this_node, state     };
  bool ok = network.write(header, &switchData, sizeof(switchData));
  if (ok) {
  } 
  else {
  }
}








