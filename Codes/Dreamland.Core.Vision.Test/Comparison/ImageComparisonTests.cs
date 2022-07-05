using System.IO;
using Dreamland.Core.Vision.Comparison;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dreamland.Core.Vision.Test
{
    [TestClass()]
    public class ImageComparisonTests
    {
        [TestMethod()]
        [DataRow("imxcg_11.png", "imxcg_22.png")]
        public void CompareSimilaritiesTest(string imageName1, string imageName2)
        {
            var imageFolder = Path.GetFullPath(@".\_TestResources\ComparisonTest");
            var image1 = Path.Combine(imageFolder, imageName1);
            var image2 = Path.Combine(imageFolder, imageName2);
            var similarity = ImageComparison.CompareSimilarity(image1, image2, new ComparisonArgument());
            Assert.IsTrue(similarity > 0.8);
        }

        [TestMethod()]
        [DataRow("imxcg_11.png", "imxcg_22.png")]
        public void CompareSimilaritiesTest2(string imageName1, string imageName2)
        {
            var imageFolder = Path.GetFullPath(@".\_TestResources\ComparisonTest");
            var image1 = Path.Combine(imageFolder, imageName1);
            var image2 = Path.Combine(imageFolder, imageName2);
            var similarity = ImageComparison.CompareSimilarity(image1, image2, new ComparisonArgument()
            {
                Type = ComparisonSimilarityType.EUCLIDEAN_DISTANCE
            });
            Assert.IsTrue(similarity > 0.7);
        }

        [TestMethod()]
        [DataRow("imxcg_11.png", "imxcg_33.png")]
        public void CompareSimilaritiesTest3(string imageName1, string imageName2)
        {
            var imageFolder = Path.GetFullPath(@".\_TestResources\ComparisonTest");
            var image1 = Path.Combine(imageFolder, imageName1);
            var image2 = Path.Combine(imageFolder, imageName2);
            var similarity = ImageComparison.CompareSimilarity(image1, image2, new ComparisonArgument()
            {
                Type = ComparisonSimilarityType.HASH_GRAY
            });
            Assert.IsTrue(similarity < 0.8);
        }
    }
}