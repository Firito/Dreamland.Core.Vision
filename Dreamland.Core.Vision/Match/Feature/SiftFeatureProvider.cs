using System;
using OpenCvSharp;

namespace Dreamland.Core.Vision.Match
{
    /// <summary>
    ///     提供基于 <see cref="Dreamland.Core.Vision.Match.MatchFeatureType.Sift"/> 算法的特征点匹配
    /// </summary>
    [FeatureProviderType(MatchFeatureType.Sift, Description = "使用Sift算法进行特征点匹配。")]
    internal class SiftFeatureProvider : FeatureProvider
    {
        public override MatchResult Match(Mat sourceMat, Mat searchMat)
        {
            throw new NotImplementedException();
        }
    }
}
