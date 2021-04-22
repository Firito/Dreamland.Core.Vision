using OpenCvSharp;

namespace Dreamland.Core.Vision.Match
{
    /// <summary>
    ///     特征点匹配
    /// </summary>
    internal static class FeatureMatch
    {
        /// <summary>
        ///     进行特征点匹配
        /// </summary>
        /// <param name="sourceMat">对应的查询（原始）图像</param>
        /// <param name="searchMat">对应的训练（模板）图像</param>
        /// <param name="featureMatchType">特征点匹配算法</param>
        /// <returns></returns>
        internal static FeatureMatchResult Match(Mat sourceMat, Mat searchMat, FeatureMatchType featureMatchType)
        {
            var featureProvider = FeatureMatchFactory.CreateFeatureProvider(featureMatchType);
            if (featureProvider == null)
            {
                return new FeatureMatchResult()
                {
                    Success = false
                };
            }

            return featureProvider.Match(sourceMat, searchMat);
        }

        /// <summary>
        ///     进行特征点匹配
        /// </summary>
        /// <param name="sourceMat">对应的查询（原始）图像</param>
        /// <param name="searchMat">对应的训练（模板）图像</param>
        /// <param name="featureMatchType">特征点匹配算法</param>
        /// <param name="argument">匹配参数</param>
        /// <returns></returns>
        internal static FeatureMatchResult Match(Mat sourceMat, Mat searchMat, FeatureMatchType featureMatchType,
            FeatureMatchArgument argument)
        {
            var featureProvider = FeatureMatchFactory.CreateFeatureProvider(featureMatchType);
            if (featureProvider == null)
            {
                return new FeatureMatchResult()
                {
                    Success = false
                };
            }

            return featureProvider.Match(sourceMat, searchMat, argument);
        }
    }
}
