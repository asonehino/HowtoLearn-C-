void setup() {
  //컴퓨터와 115200의 속도로 통신하겠다!
  Serial.begin(115200);
}

void loop() {
  //1초간격으로 0.00~100.00까지 랜덤한 값을 뽑아서 csv형식으로 전송한다!
  float temp = random(0, 10000)/100.0;
  float humi = random(0, 10000)/100.0;

  Serial.print(temp);
  Serial.print(",");
  Serial.print(humi);
  Serial.println();
  delay(1000);
}
