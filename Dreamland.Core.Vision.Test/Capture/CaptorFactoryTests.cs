using Dreamland.Core.Vision.Capture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace Dreamland.Core.Vision.Test
{
    [TestClass()]
    public class CaptorFactoryTests
    {
        [TestMethod("捕获工厂创建捕获者测试")]
        public void GetCaptorTest()
        {
            var captor = CaptorFactory.GetCaptor();
            using var data = captor.CaptureScreen();
            Assert.IsNotNull(data);
            Cv2.ImShow("捕获内容", data);
            Cv2.WaitKey(2000);
            Cv2.DestroyAllWindows();
        }
    }
}