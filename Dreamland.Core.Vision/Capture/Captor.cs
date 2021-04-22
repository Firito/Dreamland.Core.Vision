using OpenCvSharp;
using System;
using System.Diagnostics;
using PInvoke;

namespace Dreamland.Core.Vision.Capture
{
    /// <summary>
    ///     提供捕获功能的基本方法
    ///     使用责任链模式进行实际捕获方法的执行
    /// </summary>
    internal abstract class Captor : ICaptor
    {
        private double _currentFrameRate;

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="lineCaptor"></param>
        protected Captor(ICaptor lineCaptor)
        {
            LineCaptor = lineCaptor;
        }

        /// <summary>
        ///     下一阶捕获者
        /// <para>当前捕获者无法捕获时，将调用下一阶捕获者进行尝试</para>
        /// </summary>
        public ICaptor LineCaptor { get; }

        /// <summary>
        ///     是否可执行
        /// </summary>
        /// <returns></returns>
        public abstract bool CanExecute();

        /// <summary>
        ///     捕获屏幕
        /// </summary>
        public abstract Mat ExecuteCaptureScreen();

        /// <summary>
        ///     捕获窗口
        /// </summary>
        public abstract Mat ExecuteCaptureWindow(IntPtr hWnd);

        /// <summary>
        ///     获取当前捕获帧率
        /// </summary>
        public double CurrentFrameRate => _currentFrameRate;

        /// <summary>
        ///     捕获屏幕
        /// </summary>
        public Mat CaptureScreen()
        {
            return AttachFrameRate(() => CanExecute() ? ExecuteCaptureScreen() : LineCaptor?.CaptureScreen());
        }

        /// <summary>
        ///     捕获窗口
        /// </summary>
        public Mat CaptureWindow(IntPtr hWnd)
        {
            return AttachFrameRate(() => CanExecute() ? ExecuteCaptureWindow(hWnd) : LineCaptor?.CaptureWindow(hWnd));
        }

        /// <summary>
        ///     附加帧率
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        protected Mat AttachFrameRate(Func<Mat> func)
        {
            var stopwatch = Stopwatch.StartNew();
            var time = stopwatch.ElapsedMilliseconds;

            var captureData = func?.Invoke();

            stopwatch.Stop();
            var elapsedTime = stopwatch.ElapsedMilliseconds - time;


            if (captureData == null || elapsedTime <= 0)
            {
                return captureData;
            }

            _currentFrameRate = Math.Round(1000f / elapsedTime, 2);
            return captureData;
        }

        /// <summary>
        /// 获取窗口尺寸
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        protected Size GetWindowSize(IntPtr hWnd)
        {
            User32.GetWindowRect(hWnd, out var rect);
            return new Size(Math.Abs(rect.left - rect.right), Math.Abs(rect.bottom - rect.top));
        }
    }
}
