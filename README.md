# TaotieToolkit
信息收集的工具，加了一丢到bypass的功能

## Usage:

```
TaotieToolkit AllUserDirectories        用户目录
TaotieToolkit ApplockerEnumerating      枚举AppLocker
TaotieToolkit AvProcessEDRproduct       杀软或edr查询
TaotieToolkit BlockMouseAndKeyboard     禁用键盘和鼠标，second为时间，单位为秒
TaotieToolkit ClipboardGet              获取剪贴板数据
TaotieToolkit CsharpVersion             c#版本
TaotieToolkit Defender                  Windows Defender排除项
TaotieToolkit DisableDefender           禁用Windows Defender
TaotieToolkit DisableTaskManager        禁用任务管理器
TaotieToolkit DisableUAC                禁用UAC
TaotieToolkit Drives                    磁盘情况
TaotieToolkit EnableTaskManager         启用任务管理器
TaotieToolkit EnableUAC                 启用UAC
TaotieToolkit EnvironmentalVariables    环境变量
TaotieToolkit GetProcessList            枚举所有进程信息
TaotieToolkit GetRecycle                回收站
TaotieToolkit GetSystemInfo             获取系统信息
TaotieToolkit GetWindowTitle            获取活动窗口
TaotieToolkit Hibernate                 休眠
TaotieToolkit IsDebugger                是否为被调试状态
TaotieToolkit IsInSandboxie             是否在Sandboxie沙箱中
TaotieToolkit IsInVirtualMachine        是否在虚拟机中
TaotieToolkit LAPS                      枚举LAPS
TaotieToolkit Logoff                    注销登录
TaotieToolkit NetworkConnentions        网络连接
TaotieToolkit PatchAmsiScanBuffer       Patch the AmsiScanBuffer function in amsi.dll.
TaotieToolkit PatchETWEventWrite        Patch ETW
TaotieToolkit PowershellInfo            powershell版本信息
TaotieToolkit Reboot                    重启
TaotieToolkit RecentFiles               最近的文件
TaotieToolkit Screenshot                获取屏幕截图
TaotieToolkit Shutdown                  关机
TaotieToolkit UserIsActive              用户当前是否处于活动状态
```

## 添加功能：

- 继承`ICommandMarker`接口，即可自动注册命令
- 继承`ICommand`接口，实现接口的成员变量和方法
  - 填写`Name`及`Description`，`Name`表示传递给工具的参数名，`Description`用做打印帮助的提示，注意：Name不要重复，默认与类名相同
  - 在`Execute`方法中编写代码，工具类请放入Utils文件夹中，可从`args`变量中读取到传递过来的参数，如有多参数，请自行解析，无需return，直接打印结果即可。

## 免责声明
免责声明
本项目仅用于研究、教育和授权测试。其目的是协助专业人员和研究人员识别漏洞并增强系统安全性。
用户在任何系统、网络或数字环境中使用此工具之前必须获得所有相关方的明确、相互同意，因为未经授权的活动可能会导致严重的法律后果。用户有责任遵守与网络安全和数字访问相关的所有适用法律和法规。
本项目的创建者不承担因该工具的任何误用或非法使用而造成的责任，也不对由此造成的任何损害或损失负责。