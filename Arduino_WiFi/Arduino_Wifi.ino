#include "WiFiS3.h"
#include "wifi_secrets.h"

#define BUFFER_SIZE 128

#define TRIG_PIN 13
#define ECHO_PIN 12


int status = WL_IDLE_STATUS;
char ssid[] = SECRET_SSID;
char pass[] = SECRET_PASS;
int keyIndex = 0;
unsigned int localPort = 2390;
WiFiUDP Udp;

unsigned char packetBuffer[BUFFER_SIZE];


void setup() {
  Serial.begin(115200);

  Serial.println("Start");

  while (status != WL_CONNECTED) {
    Serial.print("Attempting to connect to SSID: ");
    Serial.println(ssid);
    status = WiFi.begin(ssid, pass);
    delay(1000);
  }
  Serial.println("Connected to wifi");
  IPAddress ip = WiFi.localIP();
  Serial.print("IP Address: ");
  Serial.println(ip);
  Udp.begin(localPort);

  pinMode(TRIG_PIN, OUTPUT);
  pinMode(ECHO_PIN, INPUT);
}

void loop() {
  float sensorValue = getSensorValue();

  int packetSize = Udp.parsePacket();
  Serial.println(packetSize);
  if (packetSize) {
    Serial.println("Received");
    Serial.println(Udp.remoteIP());

    Udp.read(packetBuffer, BUFFER_SIZE);

    char message[10];
    snprintf(message, sizeof(message), "%f", sensorValue);
    Serial.println(sensorValue);

    Udp.beginPacket(Udp.remoteIP(), Udp.remotePort());
    Udp.write(message);
    Udp.endPacket();

    Serial.println("Sended");
  }

  delay(2);
}


float getSensorValue() {
  digitalWrite(TRIG_PIN, LOW);
  delayMicroseconds(2);
  digitalWrite(TRIG_PIN, HIGH);
  delayMicroseconds(10);
  digitalWrite(TRIG_PIN, LOW);

  unsigned long duration = pulseIn(ECHO_PIN, HIGH);
  float distance = (float)duration * 343 / 1000000 / 2;

  return distance;
}
