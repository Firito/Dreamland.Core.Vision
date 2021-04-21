using OpenCvSharp;

namespace Dreamland.Core.Vision.Match
{
    [FeatureProviderType(MatchFeatureType.Unknown, Description = "特征点匹配的抽象类。")]
    internal abstract class FeatureProvider : IFeatureProvider
    {
        public MatchFeatureType MathFeatureType => GetMathFeatureType();

        public abstract MatchResult Match(Mat sourceMat, Mat searchMat);

        /// <summary>
        ///     获取当前类所使用的<see cref="MatchFeatureType"/>
        /// </summary>
        /// <returns></returns>
        protected virtual MatchFeatureType GetMathFeatureType()
        {
            var type = GetType();
            return MatchFeatureFactory.GetMathFeatureTypeFromAttribute(type);
        }
    }
}
