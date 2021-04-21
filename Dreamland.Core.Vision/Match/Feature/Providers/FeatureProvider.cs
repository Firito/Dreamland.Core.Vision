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
        /// <param name="sourceMat">原始图像</param>
        /// <param name="searchMat">要在原始图像中搜索的图像</param>
        /// <param name="ratio">比例阈值
        /// <para><value>该值用于配置匹配点筛选，降低这个比例阈值，匹配点数目会减少，但更加稳定；反之匹配点增加，但错误识别也会增加。默认值为 0.5。</value></para>
        /// </param>
        /// <param name="matchPoints">设置在 <see cref="sourceMat"/> 中每个特征点查询查找 <see cref="matchPoints"/> 个最佳匹配项
        /// <para><value>值越大，查找时间越长，越精准。默认值为 1。</value></para>
        /// </param>
        /// <returns></returns>
        public abstract FeatureMatchResult Match(Mat sourceMat, Mat searchMat, double ratio, uint matchPoints);

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
            IEnumerable<IEnumerable<DMatch>> goodMatches)
        {
            using var imgMatch = new Mat();
            Cv2.DrawMatchesKnn(sourceMat, keySourcePoints, searchMat, keySearchPoints, matches1To2:goodMatches, outImg:imgMatch, flags: DrawMatchesFlags.NotDrawSinglePoints);

            var windowName = $"预览窗口{Guid.NewGuid()}";
            Cv2.ImShow(windowName, imgMatch);
            Cv2.WaitKey(5000);
            Cv2.DestroyWindow(windowName);
        }
    }
}