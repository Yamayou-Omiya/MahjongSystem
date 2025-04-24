# 麻雀 自動点数計算アプリ

本アプリは，**ツモ，ロン，流局**の各状況に対応した自動点数計算ができる **Windowsフォームアプリケーション** です．プレイヤーの得点を自動的に更新し，ログも記録できます．3人麻雀・4人麻雀の切り替えも可能です．
![image](https://github.com/user-attachments/assets/229bcb65-ccf0-433e-af9b-7b6c6a053c4c)


## 🎯 主な機能

- ✅ ツモ，ロン，流局の点数自動計算
- ✅ プレイヤー名の変更機能
- ✅ リーチ棒・供託棒の管理
- ✅ 東1局〜北4局までの局進行
- ✅ 3人麻雀 / 4人麻雀の切り替え
- ✅ 点数ログの自動出力
- ✅ COMポートによるArduino連携（将来的な拡張）

---

## 🕹️ 操作手順

1. アプリ起動後，プレイヤー名を任意に入力
![image](https://github.com/user-attachments/assets/e03b1410-da18-4414-bb41-43164bff37fb)
決定ボタンを押すと，下部に名前が反映されます．player1が親でスタートし，player2,3,4の順に南，西，北で開始します
2. 「3人麻雀」「4人麻雀」モードを選択
![image](https://github.com/user-attachments/assets/af884f8f-04cd-4a63-9ae5-19ea7a72f212)
三麻と四麻で点数計算をそれぞれ行うことが可能です，現状局数を設定しても意味は特にありません
3. 各プレイヤーのリーチや和了状況をUIで入力
以下に各試合の入力例を示します．

![image](https://github.com/user-attachments/assets/7f12288c-4a00-4b1e-b452-8f08a819e26b)
ケース1 : 東一局，player1（親）のツモあがり

![image](https://github.com/user-attachments/assets/0d5d7b28-7c4e-4c04-b908-f744a8424cc5)
ケース2 : 東一局，player2（子）のツモあがり

![image](https://github.com/user-attachments/assets/ab07d4cc-114f-4645-af81-112edd93162e)
ケース3 : 東三局，player1（子）がplayer2からロン

![image](https://github.com/user-attachments/assets/ac1d0b8c-4316-44e1-a756-d784821c2efe)
ケース4 : 東三局，player3（親）がplayer1からロン

![image](https://github.com/user-attachments/assets/95ee3729-c493-4c0a-9962-968f99b4f753)
ケース5 : 東四局，流局，player1,2のテンパイ

4. 翻・符を選択 → 点数が自動計算されます
5. 「決定」ボタンで点数を反映，ログに記録されます

---

## 📐 点数計算ロジック

本アプリでは，以下の点数表に基づいて自動計算が行われます：

- 子ツモ（親・子からの支払い）
- 親ツモ（子3人からの支払い）
- ロン（親／子）
- 流局時テンパイ者への支払い処理

> 詳細は [`Form1.cs`](./Form1.cs) をご確認ください．

---


## 💻 動作環境

- Windows 10 以上
- .NET Framework 4.7.2 以上
- Visual Studio 2022

---

