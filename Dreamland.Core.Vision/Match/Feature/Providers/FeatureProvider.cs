using System;
using System.Collections.Generic;
using OpenCvSharp;

namespace Dreamland.Core.Vision.Match
{
    [FeatureProviderType(FeatureMatchType.Unknown, Description = "特征点匹配的抽象类。")]
    internal abstract class FeatureProvider : IFeatureProvider
    {
        /// <summary>
        ///     当前 <see cref="IFeatureProvider"/> 所使用的算法类型
        /// </summary>
        public FeatureMatchType MathFeatureType => GetMathFeatureType();

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
        protected virtual FeatureMatchType GetMathFeatureType()
        {
            var type = GetType();
            return FeatureMatchFactory.GetMathFeatureTypeFromAttribute(type);
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

        /// <summary>
        ///     预览匹配结果
        /// </summary>
        protected virtual void PreviewMathResult(Mat sourceMat, Mat searchMat, 
            IEnumerable<KeyPoint> keySourcePoints, IEnumerable<KeyPoint> keySearchPoints,
            IEnumerable<DMatch> goodMatches)
        {
            using var imgMatch = new Mat();
            Cv2.DrawMatches(sourceMat, keySourcePoints, searchMat, keySearchPoints, goodMatches, imgMatch, flags: DrawMatchesFlags.NotDrawSinglePoints);

            var windowName = $"预览窗口{Guid.NewGuid()}";
            Cv2.ImShow(windowName, imgMatch);
            Cv2.WaitKey(5000);
            Cv2.DestroyWindow(windowName);
        }
    }
}