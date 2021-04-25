using OpenCvSharp;
using PInvoke;
using System;

namespace Dreamland.Core.Vision.Capture
{
    /// <summary>
    ///     基于Gdi的捕获器
    /// </summary>
    internal class GdiCaptor : Captor
    {
        public GdiCaptor(ICaptor lineCaptor) : base(lineCaptor)
        {
        }

        public override bool CanExecute()
        {
            return true;
        }

        public override Mat ExecuteCaptureScreen()
        {
            var hWnd = User32.GetDesktopWindow();
            return ExecuteCaptureWindow(hWnd);
        }

        public override Mat ExecuteCaptureWindow(IntPtr hWnd)
        {
            var windowDc = User32.GetWindowDC(hWnd);
            var windowSize = GetWindowSize(hWnd);
            var width = windowSize.Width;
            var height = windowSize.Height;
            var memDc = Gdi32.CreateCompatibleDC(windowDc);
            var memBmp = Gdi32.CreateCompatibleBitmap(windowDc, width, height);
            var oldMemBmp = Gdi32.SelectObject(memDc, memBmp);
            var result = Gdi32.BitBlt(memDc.DangerousGetHandle(), 0, 0, width, height, windowDc.DangerousGetHandle(), 0, 0,
                (int)TernaryRasterOperations.SRCCOPY);

            Mat mat = null;
            var bytes = new byte[width * height * 3];
            //获取位图像素RGB数据
            if (result)
            {
                var bitmapInfo = new BITMAPINFOHEADER()
                {
                    biWidth = width,
                    biHeight = -height,
                    biPlanes = 1,
                    biBitCount = 24,
                    biCompression = BitmapCompressionMode.BI_RGB
                };
                bitmapInfo.Init();
                Gdi32Extension.GetDIBits(memDc.DangerousGetHandle(), memBmp, 0, (uint)height, bytes, ref bitmapInfo, DIB_Color_Mode.DIB_RGB_COLORS);
                mat = new Mat(height, width, MatType.CV_8UC3, bytes);
            }

            Gdi32.SelectObject(memDc, oldMemBmp);
            Gdi32.DeleteObject(memBmp);
            Gdi32.DeleteDC(memDc);
            User32.ReleaseDC(hWnd, windowDc.HWnd);
            return mat;
        }
    }
}
