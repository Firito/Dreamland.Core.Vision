using System;

namespace Dreamland.Core.Vision.Match
{
    /// <summary>
    ///     用于描述<see cref="IMatchFeature"/>所使用的算法<see cref="MatchFeatureType"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class FeatureProviderTypeAttribute : Attribute
    {
        /// <summary>
        ///     描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     特征点匹配算法类型
        /// </summary>
        public MatchFeatureType MatchFeatureType { get; }

        public FeatureProviderTypeAttribute(MatchFeatureType type)
        {
            MatchFeatureType = type;
        }
    }
}
