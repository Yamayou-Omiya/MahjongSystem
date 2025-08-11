#include <Wire.h>
#include <LiquidCrystal_I2C.h>

// LCDインスタンスを4つ生成（プレイヤーA〜D用）
LiquidCrystal_I2C lcds[4] = {
  LiquidCrystal_I2C(0x24, 16, 2),
  LiquidCrystal_I2C(0x25, 16, 2),
  LiquidCrystal_I2C(0x26, 16, 2),
  LiquidCrystal_I2C(0x27, 16, 2)
};

String inputString = "";
int scores[4];

void setup() {
  Serial.begin(9600);
  for (int i = 0; i < 4; i++) {
    lcds[i].init();
    lcds[i].backlight();
    lcds[i].clear();

    // 最初に適当な表示
    lcds[i].setCursor(0, 0);
    lcds[i].print("Mahjong Score");
    lcds[i].setCursor(0, 1);
    lcds[i].print("System Ready!");
  }
  delay(2000); // 2秒表示してから待機表示に
  for (int i = 0; i < 4; i++) {
    lcds[i].clear();
    lcds[i].setCursor(0, 0);
    lcds[i].print("Waiting scores");
  }
}

void loop() {
  if (Serial.available()) {
    inputString = Serial.readStringUntil('\n');

    // カンマ区切り → scores[]に格納
    int idx = 0, start = 0;
    for (int i = 0; i <= inputString.length(); i++) {
      if (inputString[i] == ',' || i == inputString.length()) {
        scores[idx++] = inputString.substring(start, i).toInt();
        start = i + 1;
      }
    }

    // 各LCDに自分視点で表示
    for (int i = 0; i < 4; i++) {
      lcds[i].clear();
      
      // 1行目：他家の点数を左・中央・右に配置
      int otherIdx[3];
      int j = 0;
      for (int k = 0; k < 4; k++) {
        if (k != i) otherIdx[j++] = k;
      }

      lcds[i].setCursor(0, 0);
      lcds[i].print(scores[otherIdx[0]]);
      lcds[i].setCursor(6, 0);
      lcds[i].print(scores[otherIdx[1]]);
      lcds[i].setCursor(12, 0);
      lcds[i].print(scores[otherIdx[2]]);

      // 2行目：自分の点数を中央に表示
      String myScore = String(scores[i]);
      int pos = (16 - myScore.length()) / 2;
      lcds[i].setCursor(pos, 1);
      lcds[i].print(myScore);
    }
  }
}
