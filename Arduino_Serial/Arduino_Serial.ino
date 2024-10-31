#define BUFFER_SIZE 128

#define TRIG_PIN 13
#define ECHO_PIN 12

#define LED_PIN 8


unsigned char packetBuffer[BUFFER_SIZE];


void setup() {
  Serial.begin(115200);

  pinMode(TRIG_PIN, OUTPUT);
  pinMode(ECHO_PIN, INPUT);
  pinMode(LED_PIN, OUTPUT);
}

void loop() {
  float sensorValue = getSensorValue();

  int packetSize = Serial.available();
  if (packetSize > 0) {
    Serial.readBytes(packetBuffer, min(BUFFER_SIZE, packetSize));
    bool isMousePressed = packetBuffer[0] == 1;
    digitalWrite(LED_PIN, isMousePressed ? HIGH : LOW);

    char message[10];
    snprintf(message, sizeof(message), "%f\n", sensorValue);

    Serial.write(message, sizeof(message));
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
