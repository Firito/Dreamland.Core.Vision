using System.Collections.Generic;
using System.Drawing;

namespace Dreamland.Core.Vision.Match
{
    /// <summary>
    ///     特征点匹配结果
    /// </summary>
    public class FeatureMatchResult : MatchResult
    {
        /// <summary>
        ///     获取到的匹配信息
        /// </summary>
        public IList<FeatureMatchResultItem> MatchItems { get; } = new List<FeatureMatchResultItem>();
    }

    /// <summary>
    ///     匹配项
    /// </summary>
    public class FeatureMatchResultItem : MatchResultItem
    {
        /// <summary>
        ///     匹配的特征点
        /// </summary>
        public List<Point> FeaturePoints { get; internal set; }
    }
}
