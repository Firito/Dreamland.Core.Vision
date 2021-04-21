using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using Dreamland.Core.Vision.Match;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace Dreamland.Core.Vision.Test
{
    /// <summary>
    ///     ÕºœÒ∆•≈‰≤‚ ‘
    /// </summary>
    [TestClass]
    public class CvMatchTest
    {
        [TestMethod(displayName:"ƒ£∞Ê∆•≈‰≤‚ ‘")]
        [DataTestMethod]
        [DataRow((double)0.3, (uint)10, MatchTemplateType.SQDIFF_NORMED)]
        [DataRow((double)0.8, (uint)10, MatchTemplateType.CCORR_NORMED)]
        [DataRow((double)0.8, (uint)10, MatchTemplateType.CCOEFF_NORMED)]
        public void TemplateMatchTest(double threshold, uint maxCount, MatchTemplateType type)
        {
            var imageFolder = Path.GetFullPath(@".\_TestResources\CvMatchTest");
            var sourceImage = Path.Combine(imageFolder, "source.jpg");
            var testImage1 = Path.Combine(imageFolder, "test1.png");

            //Œƒº˛≤‚ ‘
            var matchResult = CvMatch.TemplateMatch(sourceImage, testImage1, threshold, maxCount, type);
            Assert.IsTrue(matchResult.Success && matchResult.MatchItems.Any());
            
#if DEBUG
            using var sourceMat = new Mat(sourceImage);
            if (Debugger.IsAttached)
            {
                foreach (var matchItem in matchResult.MatchItems)
                {
                    var rectangle = matchItem.Rectangle;
                    Cv2.Rectangle(sourceMat, new OpenCvSharp.Point(rectangle.X, rectangle.Y), new OpenCvSharp.Point(rectangle.Right, rectangle.Bottom), Scalar.RandomColor());
                }
            }
#endif

            //Bitmap≤‚ ‘
            using var sourceBitmap = new Bitmap(sourceImage);
            using var testBitmap1 = new Bitmap(testImage1);
            matchResult = CvMatch.TemplateMatch(sourceBitmap, testBitmap1, threshold, maxCount, type);
            Assert.IsTrue(matchResult.Success && matchResult.MatchItems.Any());

#if DEBUG
            if (Debugger.IsAttached)
            {
                foreach (var matchItem in matchResult.MatchItems)
                {
                    var rectangle = matchItem.Rectangle;
                    Cv2.Rectangle(sourceMat, new OpenCvSharp.Point(rectangle.X, rectangle.Y), new OpenCvSharp.Point(rectangle.Right, rectangle.Bottom), Scalar.RandomColor());
                }
                Cv2.ImShow("ƒ£∞Ê∆•≈‰≤‚ ‘", sourceMat);
                Cv2.WaitKey(5000);
                Cv2.DestroyAllWindows();
            }
#endif
        }
    }
}
