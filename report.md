# 虚拟现实技术 大作业 实验报告

##### 分组：

王安冬 2016010912

吴一凡 2016011269

贾明麟 2016011258

## 实验内容

* VR坦克大战
* 第一人称操控坦克在VR场景中战斗
  * 操控方式：LeapMotion（连接电脑）
    * 左手张开，调整炮弹发射角度，握拳发射炮弹
    * 右手手心法向操控坦克前后移动与左右转向
  * 显示方式：CardBoard（结合手机）
  * 通信方式：基于USB模拟的TCP通信
* 游戏规则：
  * 敌方坦克会在场景内数个出生点出现
  * 敌方坦克被炮弹击中可被消灭
  * 敌方坦克被消灭5秒后会在原出生点重生
  * 敌方坦克会不断追逐玩家
  * 敌方坦克会不断向前开炮
  * 玩家被炮弹击中会损失生命值
  * 玩家生命值降为0游戏结束
  * 游戏目标是存活尽量长的时间

## 实现流程

1. 实现LeapMotion数据获取与转json
2. 实现Csharp的json解析
3. 基于ADB工具搭建PC和Android的TCP通信环境
4. 游戏主逻辑的编写
5. 将json数据传入到Unity工程内部
   1. 在场景中显示frame id
   2. 在场景中显示手（数个圆球构成）
   3. 识别手势并代替键盘用于操控
6. 设计起伏地形（最终与AI不兼容而舍弃）
7. 设计AI
8. 增加敌方坦克重生
9. 增加生存时间显示

## 技术细节

### LeapMotion

LeapMotion通过LeapSDK for Python，与电脑上的Python程序进行连接

Python程序中继承Leap.Listener类，重写其on_frame函数，具体如下：

每当获取Frame，将其中如下数据，转成json格式进行传输

> fps
>
> timestamp
>
> is_valid
>
> id
>
> hand_count
>
> 若hand_count不为0，则还将有检测到的手的如下数据（如检测到两只手则有两份）
>
> is_left
>
> is_right
>
> is_valid
>
> pinch_strength
>
> grab_strength
>
> direction
>
> palm_normal
>
> palm_position
>
> palm_velocity
>
> wrist_position
>
> sphere_center
>
> sphere_radius
>
> 各手指的各骨头的两个端点的位置

json示例见/pc_client/json_example_*.txt

### Unity

#### GameObject 说明

##### Google VR部分

以下4个Prefab导入自Google VR SDK for Unity

- GvrControllerMain：GVR主控制器
- GvrEventSystem：GVR事件系统，可替换Unity原生的EventSystem
- GvrEditorEmulator：在Unity编辑器中模拟VR设备输入
- GvrInstantPreviewMain：用于在Unity编辑器预览模式
- GvrControllerPointer：用于接收VR设备输入（*挂载于VR Camera下*）

##### LeapMotion 部分

- ~~Leap Rig：导入自Leap Motion SDK Core~~ *未使用*
- LeapMotionMonitorCanvas：显示接收到的LeapMotion数据相关信息，用于调试

##### Game 部分

- LevelArt：场景地图
- Tank：玩家操控单位
    - TankRenderer：坦克模型
    - Left/RightDustTrail：粒子特效
    - FireTransform：弹药发射位置
    - VR Camera
        - Camera：主摄像机
        - GvrControllerPointer：用于接收VR设备输入
        - Hands：双手模型，渲染LeapMotion识别到的手部
- PostFX：图像后处理效果

#### Script 说明

- Lib/SimpleJSON.cs：用于JSON数据的解析
- Managers/LeapMotionManager.cs：监听、接收、解析LeapMotion发来的数据
- Shell/ShellExplosion.cs：控制炮弹的爆炸、爆炸后的伤害判定
- Shell/BombExplosion.cs：控制炸弹的爆炸、爆炸后的伤害判定（另一种攻击方式）
- Tank/TankMovement.cs：接收键盘或LeapMotion输入，控制坦克移动
- Tank/TankHealth.cs：维护坦克的生命值、接收伤害、控制坦克的死亡
- Tank/TankShooting.cs：接收键盘或LeapMotion输入，控制坦克进行攻击
- UI/LabelController.cs：控制LeapMotionMonitorCanvas显示，用于调试
- HandControl.cs：解析LeapMotion数据、渲染双手模型
- EnemyNavigation.cs：控制敌方AI的移动与攻击

### 信号传输

由于 Leap Motion 不能直接连接在 Android 上，因此只能使用 PC 代为转发，也就是：

1. Leap Motion 接在 PC 上，PC 可以读到每一帧的识别结果；
2. 通过某种方式将识别结果传输给 Android

应该采用何种方式呢？原本考虑的是连接 WiFi 使用互联网或局域网建立 TCP 连接进行通信。但是，这样不仅延迟较高、吞吐量较低，考虑到一帧识别结果大概就需要 $6\text{KiB}$ ，识别帧率大概在 $100+\text{FPS}$ ，还有可能大量浪费流量。

出于巧合，我在网上看到一种利用 ``adb forward`` 进行端口映射，从而用 USB 进行 Android 与 PC 之间的 TCP 通信的方法。显然，这种传输方法更加快速而稳定。而且使用端口映射只需使用一条命令即可。

整体流程如下：

1. Android 连接到 PC ，开启调试模式；

2. 在命令行中输入命令 

   ``adb forward tcp:<client_port> tcp:<server_port>``

   其中 ``<client_port>,<server_port>`` 分别指未被占用的 PC 、Android 端口号

3. Android 开启 TCP 服务器，监听端口 ``localhost:<server_port>``

4. PC 作为 TCP 客户端连接到服务器 ``localhost:<client_port>`` 并向其传输每一帧的识别结果

客户端由贾明麟基于 Legacy Leap Motion Python SDK 实现，它将选取的部分数据打包成一个 JSON 字符串传给服务器；

而服务器的解析部分由吴一凡在 ``Assets/Scripts/Managers/LeapMotionListener.cs`` 中实现：

基于典型的异步 Socket 服务器，在函数 ``ReceiveCallBack`` 中收到字符串 ``msgReceive`` ， 并通过开源的 JSON 库解析为 JSON 对象类型的 ``data`` 。由于服务器运行在一个新线程上，``data`` 对象可能会被 Unity 主线程读取，因此在解析前后加了一个互斥锁。

这样的话，在 Unity 主线程中只要通过 ``data`` 就能获得最新的一帧识别结果了，事实上在项目中也是通过它来完成手势操作移动、攻击。

## 小组分工

### 王安冬

- 游戏主逻辑编写：包括玩家单位移动、玩家单位生命值控制、AI生成/重生控制
- LeapMotion接收端的数据解析
- 炮弹发射、控制移动的手势识别

### 吴一凡

* Android-PC通信框架设计
* Android Unity 服务器收包与 JSON 解析的实现
* 生存时间计分板实现
* 在 Cardboard 上进行 VR 测试

### 贾明麟

* LeapMotion硬件数据获取并转为json格式发出
* AI坦克的移动（利用王安冬同学提供的思路）、开炮逻辑

## 遇到的困难

### 王安冬

关于手部模型的渲染，最初打算使用`LeapMotion SDK for Unity`进行，该SDK原本直接从PC的LeapMotion驱动服务获取数据，数据按帧给出，封装为`Frame`结构。由于我们需要在手机上运行游戏，故需了解LeapMotion渲染CapsuleHand的过程。通过阅读LeapMotion的C#部分代码，我将关键的部分代码抽取出来（在LeapMotionCore文件夹下），并修改了`HandModelBase.cs`的代码，将其改为从我们编写的`LeapMotionManager`获取数据。

然而，队友使用的Python语言的SDK版本较低，两边的`Frame`定义不同，虽然尝试了妥协方案，但效果不佳，延迟较大，无法满足功能需求，遂放弃。最后我们自己构造了一个简易的手部模型（由6个球组成），并编写了手部的渲染代码（见`HandControl.cs`）

### 吴一凡

由于每秒传输的识别帧数过多，超过了 Unity 的渲染能力，因此可能会出现 Android 服务器同时收到多个 JSON 包的情况，正常的解析会出错。遇到这种情况我们直接忽略这些包。由于包很多并不会影响正常操控。

### 贾明麟

* LeapSDK提供的Python接口为2进制的pyd文件，使用python3程序引用之会出现“python27.dll”与当前python版本不兼容的错误
  * 解决方法：拾起已经被遗弃的Python2.7
* LeapSDK自带的Frame的序列化与反序列化方法被封装在.dll文件中，无法移植到安卓平台
  * 解决方法：将Frame的关键数据提取出来，手动转格式进行传输。

## Reference

本项目的模型、音频、粒子特效等素材取自[Unity官方Tutorial](https://learn.unity.com/project/tanks-tutorial)。