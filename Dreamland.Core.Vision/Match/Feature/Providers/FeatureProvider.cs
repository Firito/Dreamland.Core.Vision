using OpenCvSharp;

namespace Dreamland.Core.Vision.Match
{
    [FeatureProviderType(FeatureMatchType.Unknown, Description = "特征点匹配的抽象类。")]
    internal abstract class FeatureProvider : IFeatureProvider
    {
        /// <summary>
        ///     当前 <see cref="IFeatureProvider"/> 所使用的算法类型
        /// </summary>
        public FeatureMatchType MatchFeatureType => GetMatchFeatureType();

        /// <summary>
        ///     进行特征点匹配
        /// </summary>
        /// <param name="sourceMat">对应的查询（原始）图像</param>
        /// <param name="searchMat">对应的训练（模板）图像</param>
        /// <returns></returns>
        public FeatureMatchResult Match(Mat sourceMat, Mat searchMat)
        {
            return Match(sourceMat, searchMat, new FeatureMatchArgument());
        }
        
        /// <summary>
        ///     进行特征点匹配
        /// </summary>
        /// <param name="sourceMat">对应的查询（原始）图像</param>
        /// <param name="searchMat">对应的训练（模板）图像</param>
        /// <param name="argument">匹配参数</param>
        /// <returns></returns>
        public abstract FeatureMatchResult Match(Mat sourceMat, Mat searchMat, FeatureMatchArgument argument);

        /// <summary>
        ///     获取当前类所使用的<see cref="FeatureMatchType"/>
        /// </summary>
        /// <returns></returns>
        protected virtual FeatureMatchType GetMatchFeatureType()
        {
            var type = GetType();
            return FeatureMatchFactory.GetMatchFeatureTypeFromAttribute(type);
        }

        /// <summary>
        ///     <see cref="Point2f"/>转<see cref="Point2d"/>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected Point2d Point2FToPoint2D(Point2f input)
        {
            return new Point2d(input.X, input.Y);
        }
    }
}