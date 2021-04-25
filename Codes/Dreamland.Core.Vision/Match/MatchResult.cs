using System.Drawing;

namespace Dreamland.Core.Vision.Match
{
    /// <summary>
    ///     匹配结果
    /// </summary>
    public abstract class MatchResult
    {
        /// <summary>
        ///     获取一个值，该值指示匹配是否成功。
        /// </summary>
        public bool Success { get; internal set; }
    }

    /// <summary>
    ///     匹配项
    /// </summary>
    public abstract class MatchResultItem
    {
        /// <summary>
        ///     匹配中心点坐标
        /// </summary>
        public Point Point { get; internal set; }

        /// <summary>
        ///     匹配区域
        /// </summary>
        public Rectangle Rectangle { get; internal set; }

        /// <summary>
        ///     匹配值
        /// <para>
        ///     值的大小描述了匹配相似度，根据使用的具体算法决定值的大小和相似度的关系
        /// </para>
        /// </summary>
        public double Value { get; internal set; }
    }
}
