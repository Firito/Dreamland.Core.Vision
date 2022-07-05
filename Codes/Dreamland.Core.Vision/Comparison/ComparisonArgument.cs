using Dreamland.Core.Vision.Match;

namespace Dreamland.Core.Vision.Comparison
{
    /// <summary>
    ///     比较参数
    /// </summary>
    public class ComparisonArgument : MatchArgument
    {
        /// <summary>
        ///     相似度匹配的阈值, 时取值为（0-1），值越大误差越大
        /// </summary>
        public double Threshold { get; set; } = 1;

        /// <summary>
        ///     相似度匹配的算法
        /// </summary>
        public ComparisonSimilarityType Type { get; set; } = ComparisonSimilarityType.PIXEL_CONTRAST;
    }
}
