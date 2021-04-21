using System.Collections.Generic;
using System.Drawing;

namespace Dreamland.Core.Vision.Match
{
    /// <summary>
    ///     模板匹配结果
    /// </summary>
    public class TemplateMatchResult : MatchResult
    {
        /// <summary>
        ///     获取到的匹配信息
        /// </summary>
        public IList<MatchItem> MatchItems { get; } = new List<MatchItem>();
    }

    /// <summary>
    ///     匹配项
    /// </summary>
    public class MatchItem
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
