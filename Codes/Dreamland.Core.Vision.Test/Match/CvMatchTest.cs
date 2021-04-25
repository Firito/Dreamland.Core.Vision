using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Dreamland.Core.Vision.Match;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        [DataRow((double)0.3, (uint)10, TemplateMatchType.SQDIFF_NORMED)]
        [DataRow((double)0.8, (uint)10, TemplateMatchType.CCORR_NORMED)]
        [DataRow((double)0.8, (uint)10, TemplateMatchType.CCOEFF_NORMED)]
        public void TemplateMatchTest(double threshold, uint maxCount, TemplateMatchType type)
        {
            var imageFolder = Path.GetFullPath(@".\_TestResources\CvMatchTest");
            var sourceImage = Path.Combine(imageFolder, "source.png");
            var testImage1 = Path.Combine(imageFolder, "test1.png");

            //Œƒº˛≤‚ ‘
            var matchResult = CvMatch.TemplateMatch(sourceImage, testImage1, type, new TemplateMatchArgument()
            {
                MaxCount = maxCount,
                Threshold = threshold,
                ExtensionConfig = new Dictionary<string, object>()
                {
                    {"PreviewMatchResult", true }
                }
            });
            Assert.IsTrue(matchResult.Success && matchResult.MatchItems.Any());

            //Bitmap≤‚ ‘
            using var sourceBitmap = new Bitmap(sourceImage);
            using var testBitmap1 = new Bitmap(testImage1);
            matchResult = CvMatch.TemplateMatch(sourceBitmap, testBitmap1, type, new TemplateMatchArgument()
            {
                MaxCount = maxCount,
                Threshold = threshold,
                ExtensionConfig = new Dictionary<string, object>()
                {
                    {"PreviewMatchResult", true }
                }
            });
            Assert.IsTrue(matchResult.Success && matchResult.MatchItems.Any());
        }

        
        [TestMethod(displayName:"Ãÿ’˜µ„∆•≈‰≤‚ ‘")]
        [DataTestMethod]
        [DataRow(0.5, 3, FeatureMatchType.Sift)]
        [DataRow(0.2, 10, FeatureMatchType.Sift)]
        public void FeatureMatchTest(double ratio, int threshold, FeatureMatchType type)
        {
            var imageFolder = Path.GetFullPath(@".\_TestResources\CvMatchTest");
            var sourceImage = Path.Combine(imageFolder, "source.png");
            var testImage1 = Path.Combine(imageFolder, "test1.png");

            //Œƒº˛≤‚ ‘
            var matchResult = CvMatch.FeatureMatch(sourceImage, testImage1, type, new FeatureMatchArgument()
            {
                Ratio = ratio,
                RansacThreshold = (uint)threshold,
                ExtensionConfig = new Dictionary<string, object>()
                {
                    {"PreviewMatchResult", true }
                }
            });
            Assert.IsTrue(matchResult.Success && matchResult.MatchItems.Any());

            //Bitmap≤‚ ‘
            using var sourceBitmap = new Bitmap(sourceImage);
            using var testBitmap1 = new Bitmap(testImage1);
            matchResult = CvMatch.FeatureMatch(sourceImage, testImage1, type, new FeatureMatchArgument()
            {
                Ratio = ratio,
                RansacThreshold = (uint)threshold,
                ExtensionConfig = new Dictionary<string, object>()
                {
                    {"PreviewMatchResult", true }
                }
            });
            Assert.IsTrue(matchResult.Success && matchResult.MatchItems.Any());
        }
    }
}
