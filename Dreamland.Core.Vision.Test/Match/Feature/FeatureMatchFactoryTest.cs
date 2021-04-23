using Dreamland.Core.Vision.Match;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dreamland.Core.Vision.Test
{
    /// <summary>
    ///     针对<see cref="FeatureMatchFactory"/>的测试
    /// </summary>
    [TestClass]
    public class FeatureMatchFactoryTest
    {
        [TestMethod(displayName:"反射创建 IFeatureProvider 测试")]
        [DataTestMethod]
        [DataRow(FeatureMatchType.Sift)]
        public void CreateFeatureProviderTest(FeatureMatchType featureType)
        {
            var provider = FeatureMatchFactory.CreateFeatureProvider(featureType);
            Assert.IsNotNull(provider);
            Assert.AreEqual(featureType, provider.MatchFeatureType);
        }
    }
}
