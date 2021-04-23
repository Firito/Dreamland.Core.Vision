using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;
using Range = OpenCvSharp.Range;

namespace Dreamland.Core.Vision.Match
{
    /// <summary>
    ///     匹配帮助类
    /// </summary>
    internal static class MatchHelper
    {
        /// <summary>
        ///     预览模版匹配结果
        /// </summary>
        /// <param name="matchResult"></param>
        /// <param name="sourceImage"></param>
        internal static void PreviewTemplateMatchResult(TemplateMatchResult matchResult, Mat sourceImage)
        {
            using var image = new Mat(sourceImage, Range.All);
            if (matchResult.Success)
            {
                foreach (var matchItem in matchResult.MatchItems)
                {
                    var rectangle = matchItem.Rectangle;
                    Cv2.Rectangle(image, new Point(rectangle.X, rectangle.Y), new Point(rectangle.Right, rectangle.Bottom), Scalar.RandomColor(), 3);
                }
            }
            PreviewMatchResultImage(image);
        }

        /// <summary>
        ///     预览匹配结果
        /// </summary>
        internal static void PreviewFeatureMatchResult(FeatureMatchResult matchResult, Mat sourceMat, Mat searchMat, 
            IEnumerable<KeyPoint> keySourcePoints, IEnumerable<KeyPoint> keySearchPoints,
            IEnumerable<DMatch> goodMatches)
        {
            using var image = new Mat(sourceMat, Range.All);
            if (matchResult.Success)
            {
                foreach (var matchItem in matchResult.MatchItems)
                {
                    var rectangle = matchItem.Rectangle;
                    Cv2.Rectangle(image, new Point(rectangle.X, rectangle.Y), new Point(rectangle.Right, rectangle.Bottom), Scalar.RandomColor(), 3);
                }
            }

            using var imgMatch = new Mat();
            Cv2.DrawMatches(image, keySourcePoints, searchMat, keySearchPoints, goodMatches, imgMatch, flags: DrawMatchesFlags.NotDrawSinglePoints);
            PreviewMatchResultImage(imgMatch);
        }

        /// <summary>
        ///     预览图像
        /// </summary>
        /// <param name="imageMat"></param>
        private static void PreviewMatchResultImage(Mat imageMat)
        {
            var windowName = $"预览窗口{Guid.NewGuid()}";
            const int maxHeight = 500;
            if (imageMat.Height < maxHeight)
            {
                Cv2.ImShow(windowName, imageMat);
            }
            else
            {
                var radio = (double) imageMat.Width / imageMat.Height;
                var resizeWidth = maxHeight * radio;
                var size = new Size(resizeWidth, maxHeight);
                using var resizeMat = new Mat(size, imageMat.Type());
                Cv2.Resize(imageMat, resizeMat, size);
                Cv2.ImShow(windowName, resizeMat);
            }

            Cv2.WaitKey(5000);
            Cv2.DestroyWindow(windowName);
        }
    }
}
