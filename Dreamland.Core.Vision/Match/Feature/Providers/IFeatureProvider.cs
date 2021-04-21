using OpenCvSharp;

namespace Dreamland.Core.Vision.Match
{
    internal interface IFeatureProvider
    {
        /// <summary>
        ///     当前 <see cref="IFeatureProvider"/> 所使用的算法类型
        /// </summary>
        MatchFeatureType MathFeatureType { get; }

        /// <summary>
        ///     进行特征点匹配
        /// </summary>
        /// <param name="sourceMat">原始图像</param>
        /// <param name="searchMat">要在原始图像中搜索的图像</param>
        /// <param name="ratio">比例阈值
        /// <para><value>该值用于配置匹配点筛选，降低这个比例阈值，匹配点数目会减少，但更加稳定；反之匹配点增加，但错误识别也会增加。默认值为 0.5。</value></para>
        /// </param>
        /// <param name="matchPoints">设置在 <see cref="sourceMat"/> 中每个特征点查询查找 <see cref="matchPoints"/> 个最佳匹配项
        /// <para><value>值越大，查找时间越长，越精准。默认值为 2。</value></para>
        /// </param>
        /// <returns></returns>
        MatchResult Match(Mat sourceMat, Mat searchMat, double ratio = 0.5, uint matchPoints = 2);
    }
}
