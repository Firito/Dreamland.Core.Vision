namespace Dreamland.Core.Vision.Match
{
    /// <summary>
    ///     模版匹配参数
    /// </summary>
    public class TemplateMatchArgument : MatchArgument
    {
        /// <summary>
        ///     相似度匹配的阈值
        /// <para>
        ///         在<see cref="TemplateMatchType.SQDIFF"/>和<see cref="TemplateMatchType.SQDIFF_NORMED"/>模式下，当相识度大于该阈值的时候，就忽略掉；
        ///         在其他<see cref="TemplateMatchType"/>模式下，当相识度小于该阈值的时候，就忽略掉；
        /// </para>
        /// </summary>
        public double Threshold { get; set; } = 0.5;

        /// <summary>
        ///     最大匹配数，返回最匹配的结果的最大个数
        /// </summary>
        public uint MaxCount { get; set; } = 1;
    }
}
