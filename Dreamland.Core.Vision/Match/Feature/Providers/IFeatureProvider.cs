using OpenCvSharp;

namespace Dreamland.Core.Vision.Match
{
    internal interface IFeatureProvider
    {
        /// <summary>
        ///     算法类型
        /// </summary>
        MatchFeatureType MathFeatureType { get; }

        /// <summary>
        ///     进行特征点匹配
        /// </summary>
        /// <param name="sourceMat"></param>
        /// <param name="searchMat"></param>
        /// <returns></returns>
        MatchResult Match(Mat sourceMat, Mat searchMat);
    }
}
