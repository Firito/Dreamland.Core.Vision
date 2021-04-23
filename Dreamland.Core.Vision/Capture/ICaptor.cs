using OpenCvSharp;
using System;

namespace Dreamland.Core.Vision.Capture
{
    /// <summary>
    ///     提供屏幕/窗口捕获相关方法的接口
    /// </summary>
    public interface ICaptor
    {
        /// <summary>
        ///     下一阶捕获者
        /// <para>当前捕获者无法捕获时，将调用下一阶捕获者进行尝试</para>
        /// </summary>
        ICaptor LineCaptor { get; }

        /// <summary>
        ///     捕获屏幕
        /// </summary>
        Mat CaptureScreen();

        /// <summary>
        ///     捕获窗口
        /// </summary>
        Mat CaptureWindow(IntPtr hWnd);

        /// <summary>
        ///     获取当前捕获帧率
        /// </summary>
        double CurrentFrameRate { get; }
    }
}