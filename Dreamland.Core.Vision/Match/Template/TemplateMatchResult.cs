using System.Collections.Generic;

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
        public IList<TemplateMatchResultItem> MatchItems { get; } = new List<TemplateMatchResultItem>();
    }

    /// <summary>
    ///     匹配项
    /// </summary>
    public class TemplateMatchResultItem : MatchResultItem
    {
    }
}
