#define CDS 36
#define ROTARY 39

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
}

void loop() {
  float cdsv = 3.3*analogRead(CDS)/4095.0;
  float rotaryv = 3.3*analogRead(ROTARY)/4095.0;

  Serial.print(cdsv);
  Serial.print(", ");
  Serial.print(rotaryv);
  Serial.print("\n");
  delay(1000);
}
