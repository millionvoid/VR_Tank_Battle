# VR_Tank_Battle
A VR Game controlled by LeapMotion, shown via CardBoard

## 使用 LeapMotion 服务流程

1. 连接 Leap Motion，确认其可用

2. 连接 Android 手机，确认调试模式开启

3. 命令行 ``adb devices`` ，确认找到 Android 设备

4. 命令行 ``adb forward tcp:7777 tcp:8888`` ，建立连接

5. 手机上运行带有 ServerController.cs 的 Unity 程序

6. PC 端运行 ``pc_client/frame_to_json/main.py``

   环境 python2 ，无需任何拓展包