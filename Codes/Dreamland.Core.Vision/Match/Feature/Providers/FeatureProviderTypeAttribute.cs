using System;

namespace Dreamland.Core.Vision.Match
{
    /// <summary>
    ///     用于描述<see cref="IFeatureProvider"/>所使用的算法<see cref="FeatureMatchType"/>
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
        public FeatureMatchType FeatureMatchType { get; }

        public FeatureProviderTypeAttribute(FeatureMatchType type)
        {
            FeatureMatchType = type;
        }
    }
}
