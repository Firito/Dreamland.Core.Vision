using Dreamland.Core.Vision.Match;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dreamland.Core.Vision.Test
{
    /// <summary>
    ///     针对<see cref="MatchFeatureFactory"/>的测试
    /// </summary>
    [TestClass]
    public class MatchFeatureFactoryTest
    {
        [TestMethod(displayName:"反射创建 IMatchFeature 测试")]
        [DataTestMethod]
        [DataRow(MatchFeatureType.Sift)]
        public void CreateMatchFeature(MatchFeatureType featureType)
        {
            var matchFeature = MatchFeatureFactory.CreateMatchFeature(featureType);
            Assert.IsNotNull(matchFeature);
            Assert.AreEqual(featureType, matchFeature.MathFeatureType);
        }
    }
}
