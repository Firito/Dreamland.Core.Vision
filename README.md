# 介绍

## 项目描述

为Dreamland项目提供计算机视觉相关功能的支持

## 功能

### 屏幕/窗口捕获

``` C#
    //创建一个捕获者
    var captor = CaptorFactory.GetCaptor();
    //返回一个Mat类型的屏幕截图
    using Mat screenImage = captor.CaptureScreen();
    
    //拿到一个窗口句柄
    IntPtr hWnd = ...;
    //返回一个Mat类型的窗口截图
    using Mat data = captor.CaptureWindow(hWnd);
```

#### 注意事项

在非100%DPI，因为DPI感知的问题，会导致截图大小错误；

方法一、[推荐] 使用application (.exe) manifest

添加 app.manifest 文件，并找到DPI感知配置，并取消注释

``` xml
  <!-- 指示该应用程序可以感知 DPI 且 Windows 在 DPI 较高时将不会对其进行
       自动缩放。Windows Presentation Foundation (WPF)应用程序自动感知 DPI，无需
       选择加入。选择加入此设置的 Windows 窗体应用程序(目标设定为 .NET Framework 4.6 )还应
       在其 app.config 中将 "EnableWindowsFormsHighDpiAutoResizing" 设置设置为 "true"。-->
  
  <application xmlns="urn:schemas-microsoft-com:asm.v3">
    <windowsSettings>
      <dpiAware xmlns="http://schemas.microsoft.com/SMI/2005/WindowsSettings">true</dpiAware>
    </windowsSettings>
  </application>
```

方法二、调用SetProcessDPIAware()

``` C#
    //使用静态方法，改变程序DPI感知
    User32.SetProcessDPIAware();
```

### 图像匹配(定位)

#### 模版匹配

注：模版匹配对旋转和缩放的图片无法准确识别

使用范例：

``` C#
    /// 传入两张图片，
    /// <param name="sourceImage">对应的查询（原始）图像</param>
    /// <param name="searchImage">对应的训练（模板）图像（宽高不得超过被查询图像）</param>
    var matchResult = CvMatch.TemplateMatch(@"sourceImage", @"searchImage");
    if(matchResult.Success)
    {
        //匹配的结果，可能存在多出匹配
        var matchItems = matchResult.MatchItems;
        //匹配中心点坐标
        var centerPoint = MatchItems[0].Point;
        //匹配区域
        var rectangle = MatchItems[0].Rectangle;
        //匹配值，值的大小描述了匹配相似度，根据使用的具体算法决定值的大小和相似度的关系
        var value = MatchItems[0].Value;
    }
```

#### 特征点匹配

注：根据选择的特征点匹配算法不同，对旋转和缩放的图片识别的精准性也会不同

目前提供的算法支持：

* SIFT

使用范例：

``` C#
    /// 传入两张图片，
    /// <param name="sourceImage">对应的查询（原始）图像</param>
    /// <param name="searchImage">对应的训练（模板）图像（宽高不得超过被查询图像）</param>
    var matchResult = CvMatch.FeatureMatch(@"sourceImage", @"searchImage");
    if(matchResult.Success)
    {
        //匹配的结果，可能存在多出匹配
        var matchItems = matchResult.MatchItems;
        //匹配中心点坐标
        var centerPoint = MatchItems[0].Point;
        //匹配区域
        var rectangle = MatchItems[0].Rectangle;
        //匹配值，值的大小描述了匹配相似度，根据使用的具体算法决定值的大小和相似度的关系
        var value = MatchItems[0].Value;
        //匹配的特征点
        var featurePoints = MatchItems[0].FeaturePoints;
    }
```
