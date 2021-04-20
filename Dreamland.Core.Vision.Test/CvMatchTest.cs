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
        [TestMethod(displayName:"∆•≈‰≤‚ ‘")]
        public void TemplateMatchTest()
        {
            var imageFolder = Path.GetFullPath(@".\TestResources\CvMatchTest");
            var sourceImage = Path.Combine(imageFolder, "source.jpg");
            var testImage1 = Path.Combine(imageFolder, "test1.png");

            //Œƒº˛≤‚ ‘
            var matchResult = CvMatch.TemplateMatch(sourceImage, testImage1);
            Assert.IsTrue(matchResult.Success && matchResult.MatchItems.Any());

            //Bitmap≤‚ ‘
            using var sourceBitmap = new Bitmap(sourceImage);
            using var testBitmap1 = new Bitmap(testImage1);
            matchResult = CvMatch.TemplateMatch(sourceBitmap, testBitmap1);
            Assert.IsTrue(matchResult.Success && matchResult.MatchItems.Any());
        }
    }
}
