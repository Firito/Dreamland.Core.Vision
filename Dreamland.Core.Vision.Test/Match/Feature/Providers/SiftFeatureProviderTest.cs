using System.IO;
using Dreamland.Core.Vision.Match;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace Dreamland.Core.Vision.Test
{
    /// <summary>
    ///     针对 "SIFT" 算法的测试
    /// </summary>
    [TestClass]
    public class SiftFeatureProviderTest
    {
        [TestMethod(displayName:"Sift算法 特征点匹配测试")]
        [DataTestMethod]
        [DataRow(0.5, 2)]
        [DataRow(0.2, 2)]
        [DataRow(0.5, 5)]
        [DataRow(0.2, 5)]
        public void MatchTest(double ratio, int matchPoints)
        {
            var imageFolder = Path.GetFullPath(@".\_TestResources\CvMatchTest");
            var sourceImage = Path.Combine(imageFolder, "source.png");
            var searchImage = Path.Combine(imageFolder, "test1.png");

            using var sourceMat = new Mat(sourceImage, ImreadModes.AnyColor);
            using var searchMat = new Mat(searchImage, ImreadModes.AnyColor);

            var siftFeatureProvider = new SiftFeatureProvider();
            siftFeatureProvider.Match(sourceMat, searchMat, ratio, (uint) matchPoints);
        }
    }
}
