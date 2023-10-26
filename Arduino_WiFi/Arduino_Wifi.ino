#include "WiFiS3.h"
#include "wifi_secrets.h"

#define BUFFER_SIZE 128


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
}

void loop() {
  float value = (float)random(0, 100) / 200;

  int packetSize = Udp.parsePacket();
  Serial.println(packetSize);
  if (packetSize) {
    Serial.println("Received");
    Serial.println(Udp.remoteIP());

    Udp.read(packetBuffer, BUFFER_SIZE);

    char message[10];
    snprintf(message, sizeof(message), "%f", value);

    Udp.beginPacket(Udp.remoteIP(), Udp.remotePort());
    Udp.write(message);
    Udp.endPacket();

    Serial.println("Sended");
  }

  delay(2);
}
