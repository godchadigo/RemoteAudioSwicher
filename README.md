# RemoteAudioSwicher
## 製作動機
- 因為平常有躺在床上聽音樂的習慣，但是又懶得從床上爬起來去調整喇吧音量，又或者是有時候想睡覺，電腦音量忘記關訊息提示，此時我就想手機都在身上，能否用手機來控制電腦音效卡輸出以及音量，然後此專案就誕生了...
- 因為目前在當兵沒什麼時間，這個成品利用假日從0到有只花了1天的時間，所以代碼非常的亂之後有空再整理@@

## 原理
- 利用Windows api去獲取本機的音效卡，以及控制本機的音量。
- 然後利用JsonRPC的方式通訊，c#端為Server，python端為Client。
- python端實現WebAPI以及簡易的Flask Web，在板模中利用Jquery js來做Web api callback，python收到callback後再去呼叫jsonRpc下指令給c# Server端。

## 使用方法
1. 下載Release檔案
2. 解壓縮檔案後，雙擊start.bat
![image](https://github.com/godchadigo/RemoteAudioSwicher/assets/19208239/db797746-af22-4d5d-9a24-94c9fdba2426)
4. 訪問http://yourIP:5000/device  (yourIP再啟動start.bat時會顯示)
![image](https://github.com/godchadigo/RemoteAudioSwicher/assets/19208239/f8e4b9ae-c9cd-4b38-bd5a-5f47c99f4f41)
![image](https://github.com/godchadigo/RemoteAudioSwicher/assets/19208239/c3ea8227-2c7f-435c-89ac-7778ad7d9cca)

## 借助工具
[AudioSwitcher](https://github.com/xenolightning/AudioSwitcher)
[Touchsocket](https://github.com/RRQM/TouchSocket)
