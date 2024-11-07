#define CDS 36
#define ROTARY 39

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
}

void loop() {
  float cdsv = analogRead(CDS);
  float rotaryv = analogRead(ROTARY);

  Serial.print(cdsv);
  Serial.print(",");
  Serial.print(rotaryv);
  Serial.print(",");
  delay(1000);
}
