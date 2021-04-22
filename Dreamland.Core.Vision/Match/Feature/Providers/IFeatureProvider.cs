using OpenCvSharp;

namespace Dreamland.Core.Vision.Match
{
    internal interface IFeatureProvider
    {
        /// <summary>
        ///     当前 <see cref="IFeatureProvider"/> 所使用的算法类型
        /// </summary>
        FeatureMatchType MathFeatureType { get; }
        
        /// <summary>
        ///     进行特征点匹配
        /// </summary>
        /// <param name="sourceMat">对应的查询（原始）图像</param>
        /// <param name="searchMat">对应的训练（模板）图像</param>
        /// <returns></returns>
        FeatureMatchResult Match(Mat sourceMat, Mat searchMat);
        
        /// <summary>
        ///     进行特征点匹配
        /// </summary>
        /// <param name="sourceMat">对应的查询（原始）图像</param>
        /// <param name="searchMat">对应的训练（模板）图像</param>
        /// <param name="argument">匹配参数</param>
        /// <returns></returns>
        FeatureMatchResult Match(Mat sourceMat, Mat searchMat, FeatureMatchArgument argument);
    }
}
