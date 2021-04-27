using OpenCvSharp;
using System;
using System.Collections.Generic;

namespace Dreamland.Core.Vision.Match
{
    /// <summary>
    ///     提供对匹配进行调试的相关拓展方法
    /// </summary>
    internal static class MatchDebugExtension
    {
        /// <summary>
        ///     预览模版匹配结果（仅在开启了<see cref="MatchArgument.PreviewMatchResult"/>配置时）
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="matchResult"></param>
        /// <param name="sourceImage"></param>
        internal static void PreviewDebugTemplateMatchResult(this MatchArgument argument, TemplateMatchResult matchResult, Mat sourceImage)
        {
            if (!argument.IsExtensionConfigEnabled(MatchArgument.PreviewMatchResult))
            {
                return;
            }

            using var image = new Mat(sourceImage, OpenCvSharp.Range.All);
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
        ///     预览匹配结果（仅在开启了<see cref="MatchArgument.PreviewMatchResult"/>配置时）
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="matchResult"></param>
        /// <param name="sourceMat"></param>
        /// <param name="searchMat"></param>
        /// <param name="keySourcePoints"></param>
        /// <param name="keySearchPoints"></param>
        /// <param name="goodMatches"></param>
        internal static void PreviewDebugFeatureMatchResult(this MatchArgument argument, FeatureMatchResult matchResult, Mat sourceMat, Mat searchMat,
            IEnumerable<KeyPoint> keySourcePoints, IEnumerable<KeyPoint> keySearchPoints,
            IEnumerable<DMatch> goodMatches)
        {
            if (!argument.IsExtensionConfigEnabled(MatchArgument.PreviewMatchResult))
            {
                return;
            }

            using var image = new Mat(sourceMat, OpenCvSharp.Range.All);
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
        ///     输出调试信息（仅在开启了<see cref="MatchArgument.ConsoleOutput"/>配置时）
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="message"></param>
        internal static void OutputDebugMessage(this MatchArgument argument, string message)
        {
            if (argument.IsExtensionConfigEnabled(MatchArgument.ConsoleOutput))
            {
                Console.WriteLine("[ConsoleOutput] " + message);
            }
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
                var radio = (double)imageMat.Width / imageMat.Height;
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
