#include "WiFiS3.h"
#include "wifi_secrets.h"

#define BUFFER_SIZE 128

#define TRIG_PIN 13
#define ECHO_PIN 12

#define LED_PIN 8


int status = WL_IDLE_STATUS;
char ssid[] = SECRET_SSID;
char pass[] = SECRET_PASS;
int keyIndex = 0;
unsigned int localPort = 2390;
WiFiUDP Udp;

unsigned char packetBuffer[BUFFER_SIZE];


void setup() {
  Serial.begin(115200);

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
  pinMode(LED_PIN, OUTPUT);
}

void loop() {
  float sensorValue = getSensorValue();

  int packetSize = Udp.parsePacket();
  if (packetSize) {
    Udp.read(packetBuffer, BUFFER_SIZE);
    bool isMousePressed = packetBuffer[0] == 1;
    digitalWrite(LED_PIN, isMousePressed ? 1 : 0);

    char message[10];
    snprintf(message, sizeof(message), "%f", sensorValue);

    Udp.beginPacket(Udp.remoteIP(), Udp.remotePort());
    Udp.write(message);
    Udp.endPacket();
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
