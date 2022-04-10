# 简介

YouV.Log是基于NLog开发的轻量级，高性能的日志组件，主要特点是可分类打印日志，并能将所有类别的日志汇总到总日志。

# 准备

将`NLog`和`NLog.config`拷贝到应用根目录，添加引用: `YouV.Log.dll`，引入命名空间:`YouV.Log`。

# API

YouV.Log提供两个API。

(1) 打印日志

```csharp
public static void WriteLog(string message, string? category = null, LogLevel logLevel = LogLevel.INFO, bool alsoIntoAll = true)
```

| 参数        | 说明                 |
| ----------- | -------------------- |
| message     | 日志消息             |
| category    | 日志类别             |
| logLevel    | 日志等级             |
| alsoInfoAll | 日志是否汇总到总日志 |

（2）注册日志事件

```csharp
public static event LogWrittenEventHandler? LogWritten;
```
LogWrittenEventHandler类型
```csharp
public delegate void LogWrittenEventHandler(string message, string category, LogLevel logLevel);
```
每调用一次方法WriteLog()打印日志，便会异步执行一次此事件。所以，当您想分发日志到其他进程或实时显示日志到UI可注册此事件。

# 使用说明

如果您在开发一个小型的App，可能对日志功能要求并不高，只需要将所有的日志信息输入到同一个文本文件即可，您可以这样：

```csharp
Logger.WriteLog("我们是世界第一抗疫大国，赢麻了！");
```

日志等级默认是INFO，您也可以指定日志等级，YouV.Log提供5种日志等级：`DEBUG`,`INFO`,`WARN`,`ERROR`,`FATAL`.

```csharp
Logger.WriteLog("我们是世界第一抗疫大国，赢麻了！", logLevel: LogLevel.DEBUG);
Logger.WriteLog("我们是世界第一抗疫大国，赢麻了！", logLevel: LogLevel.INFO);
Logger.WriteLog("我们是世界第一抗疫大国，赢麻了！", logLevel: LogLevel.WARN);
Logger.WriteLog("我们是世界第一抗疫大国，赢麻了！", logLevel: LogLevel.ERROR);
Logger.WriteLog("我们是世界第一抗疫大国，赢麻了！", logLevel: LogLevel.FATAL);
```

或

```csharp
Logger.WriteLog("我们是世界第一抗疫大国，赢麻了！", null, LogLevel.DEBUG);
Logger.WriteLog("我们是世界第一抗疫大国，赢麻了！", null, LogLevel.INFO);
Logger.WriteLog("我们是世界第一抗疫大国，赢麻了！", null, LogLevel.WARN);
Logger.WriteLog("我们是世界第一抗疫大国，赢麻了！", null, LogLevel.ERROR);
Logger.WriteLog("我们是世界第一抗疫大国，赢麻了！", null, LogLevel.FATAL);
```

YouV.Log定义日志文件名是`app的名称.app.all.log`，另外，YouV.Log会将Warn，Error，Fatal等级的日志额外的输入到`app的名称.app.all.trouble.log`，当App发生故障时，这有利于快速查看到故障信息。

![动画](https://user-images.githubusercontent.com/67289897/162614486-39472c0d-036c-441c-ae7d-84c9835149b6.gif)


如果您在开发大型的App，可能会产生海量的日志信息。如果不对日志进行合理的分类，可能导致您去查看感兴趣的关键信息如大海捞针般困难。YouV.Log支持对日志进行分类打印，同时也会自动的汇总所有类别日志到app.all.log。这样，如果您的App发生故障，假设我们通过错误弹窗或其他现象得知是A模块发生了故障，那么我们可以直接只看A模块的相关日志，快速定位故障原因。我们仍旧可以通过app.all.log，查看整个App的所有模块按照执行先后打印的交织在一起的日志。

下面以一个自动化设备上位机App举例：

1. 登录模块 Authority

2. 扫码枪模块 Scanner

3. 视觉模块 Vision

4. 运动模块 Motion

5. 通信模块 Mqtt

```csharp
Logger.WriteLog("张三已登录", "Authority");
Logger.WriteLog("打开扫码枪失败!", "Scanner", LogLevel.ERROR);
Logger.WriteLog("视觉反馈坐标是{X:1.001,Y:2.652}", "Vision", LogLevel.WARN);
Logger.WriteLog("收到消息[ABS#X#51.075]", "Mqtt", LogLevel.FATAL);
```

![16点48分 2022年4月10日](https://user-images.githubusercontent.com/67289897/162614510-2d632f77-c7dc-47e5-b28c-ee0614893749.gif)

# alsoIntoAll

分模块打印日志，各个模块的日志同时也会自动的被汇总到总日志中。如果您希望某些日志仅仅存储在其模块日志中，不希望或不需要也同时输入到总日志，您可以设置参数alsoIntoAll=false.

```csharp
Logger.WriteLog("I am a msg of moduleA that will written to all.", "A", alsoIntoAll: false);
Logger.WriteLog("I am a msg of moduleA that won't written to all.", "A", alsoIntoAll: true);
```

`I am a msg of moduleA that will written to all.`和`I am a msg of moduleA that won't written to all.`都会被存储到A.log，但`I am a msg of moduleA that won't written to all.`不会存储在app.all.log.

![17点13分 2022年4月10日](https://user-images.githubusercontent.com/67289897/162614527-b7c74ab6-6177-4812-836f-d39ac723152e.gif)

适用于在某个线程中死循环中一直打印日志，监控线程是否死了，但又不想让监控日志输出到总日志，导致这种无太大价值的监控日志刷新总日志。

# 性能保证
1. 在NLog框架的基础上，仅添加if... else...类的代码封装，性能就是框架本身的性能
2. 使用双检锁机制封装，既避免每次写入日志时频繁的创建`写者`对象的开销，又避免了线程锁竞争开销
