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
