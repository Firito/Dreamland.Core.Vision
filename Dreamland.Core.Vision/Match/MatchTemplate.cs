using System;
using System.Drawing;
using System.Linq;
using OpenCvSharp;

namespace Dreamland.Core.Vision.Match
{
    /// <summary>
    ///     模版匹配
    /// </summary>
    public static class MatchTemplate
    {
        /// <summary>
        ///     进行模版匹配
        /// </summary>
        /// <param name="sourceMat">原始图片</param>
        /// <param name="searchMat">需要查找的图片</param>
        /// <param name="threshold"> 阈值，当相识度小于该阈值的时候，就忽略掉</param>
        /// <param name="maxCount">最大的匹配数</param>
        /// <param name="matchModes">匹配算法</param>
        /// <returns></returns>
        internal static MatchResult Match(Mat sourceMat, Mat searchMat, double threshold, uint maxCount, TemplateMatchModes matchModes)
        {
            using var resultMat = new Mat();
            resultMat.Create(sourceMat.Cols - searchMat.Cols + 1, sourceMat.Rows - searchMat.Cols + 1,
                MatType.CV_32FC1);

            //进行模版匹配
            Cv2.MatchTemplate(sourceMat, searchMat, resultMat, matchModes);

            //对结果进行归一化
            Cv2.Normalize(resultMat, resultMat, 1, 0, NormTypes.MinMax, -1);

            return GetMatchResult(searchMat, resultMat, threshold, maxCount, matchModes);

        }

        /// <summary>
        ///     获取匹配结果
        /// </summary>
        /// <param name="searchMat">需要查找的图片</param>
        /// <param name="resultMat">匹配结果</param>
        /// <param name="threshold"> 阈值，当相识度小于该阈值的时候，就忽略掉</param>
        /// <param name="maxCount">最大的匹配数</param>
        /// <param name="matchModes">匹配算法</param>
        /// <returns></returns>
        private static MatchResult GetMatchResult(Mat searchMat, Mat resultMat, double threshold, uint maxCount, TemplateMatchModes matchModes)
        {
            var matchResult = new MatchResult();
            while (matchResult.MatchItems.Count < maxCount)
            {
                OpenCvSharp.Point topLeft;
                Cv2.MinMaxLoc(resultMat, out _, out var maxValue, out var minLocation, out var maxLocation);
                if (matchModes == TemplateMatchModes.SqDiff || matchModes == TemplateMatchModes.SqDiffNormed)
                {
                    topLeft = minLocation;
                }
                else
                {
                    topLeft = maxLocation;
                }

                Console.WriteLine($"TemplateMatch Value({threshold:F}) = {maxValue:F}");
                if (maxValue < threshold)
                {
                    break;
                }

                var matchItem = new MatchItem()
                {
                    Value = maxValue
                };
                matchItem.Point.Offset(topLeft.X + searchMat.Width / 2, topLeft.Y + searchMat.Height / 2);
                matchItem.Rectangle = new Rectangle(topLeft.X, topLeft.Y, searchMat.Width, searchMat.Height);
                matchResult.MatchItems.Add(matchItem);

                //屏蔽已筛选区域
                if (matchModes == TemplateMatchModes.SqDiff || matchModes == TemplateMatchModes.SqDiffNormed)
                {
                    Cv2.FloodFill(resultMat, topLeft, double.MaxValue);
                }
                else
                {
                    Cv2.FloodFill(resultMat, topLeft, double.MinValue);
                }
            }

            matchResult.Success = matchResult.MatchItems.Any();
            return matchResult;
        }

        internal static TemplateMatchModes ConvertToMatchModes(MatchTemplateType type)
        {
            var i = (int)type;
            if (Enum.IsDefined(typeof(TemplateMatchModes), i))
            {
                return (TemplateMatchModes)Enum.ToObject(typeof(TemplateMatchModes), i);
            }

            return TemplateMatchModes.CCoeffNormed;
        }
    }
}
