using System;
using System.Linq;
using OpenCvSharp;

namespace Dreamland.Core.Vision.Match
{
    /// <summary>
    ///     模版匹配
    /// </summary>
    public static class TemplateMatch
    {
        /// <summary>
        ///     进行模版匹配
        /// </summary>
        /// <param name="sourceMat">原始图片</param>
        /// <param name="searchMat">需要查找的图片</param>
        /// <param name="threshold"> 相似度匹配的阈值
        ///     <para>
        ///         在<see cref="TemplateMatchModes.SqDiff"/>和<see cref="TemplateMatchModes.SqDiffNormed"/>模式下，当相识度大于该阈值的时候，就忽略掉；
        ///         在其他<see cref="TemplateMatchModes"/>模式下，当相识度小于该阈值的时候，就忽略掉；
        ///     </para>
        /// </param>
        /// <param name="maxCount">最大的匹配数</param>
        /// <param name="matchModes">匹配算法</param>
        /// <returns></returns>
        internal static TemplateMatchResult Match(Mat sourceMat, Mat searchMat, double threshold, uint maxCount,
            TemplateMatchModes matchModes)
        {
            using var resultMat = new Mat();
            resultMat.Create(sourceMat.Rows - searchMat.Rows + 1, sourceMat.Cols - searchMat.Cols + 1,
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
        /// <param name="threshold"> 相似度匹配的阈值
        ///     <para>
        ///         在<see cref="TemplateMatchModes.SqDiff"/>和<see cref="TemplateMatchModes.SqDiffNormed"/>模式下，当相识度大于该阈值的时候，就忽略掉；
        ///         在其他<see cref="TemplateMatchModes"/>模式下，当相识度小于该阈值的时候，就忽略掉；
        ///     </para>
        /// </param>
        /// <param name="maxCount">最大的匹配数</param>
        /// <param name="matchModes">匹配算法</param>
        /// <returns></returns>
        private static TemplateMatchResult GetMatchResult(Mat searchMat, Mat resultMat, double threshold, uint maxCount,
            TemplateMatchModes matchModes)
        {
            var matchResult = new TemplateMatchResult();
            while (matchResult.MatchItems.Count < maxCount)
            {
                double value;
                Point topLeft;
                Cv2.MinMaxLoc(resultMat, out var minValue, out var maxValue, out var minLocation, out var maxLocation);
                if (matchModes == TemplateMatchModes.SqDiff || matchModes == TemplateMatchModes.SqDiffNormed)
                {
                    value = minValue;
                    topLeft = minLocation;
                }
                else
                {
                    value = maxValue;
                    topLeft = maxLocation;
                }

                Console.WriteLine($"TemplateMatch Value({threshold:F}) = {value:F}");
                if (maxValue < threshold)
                {
                    break;
                }

                var matchItem = new TemplateMatchResultItem()
                {
                    Value = value
                };
                var centerX = topLeft.X + (double) searchMat.Width / 2;
                var centerY = topLeft.Y + (double) searchMat.Height / 2;
                matchItem.Point = new System.Drawing.Point((int) centerX, (int) centerY);
                matchItem.Rectangle =
                    new System.Drawing.Rectangle(topLeft.X, topLeft.Y, searchMat.Width, searchMat.Height);
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

        internal static TemplateMatchModes ConvertToMatchModes(TemplateMatchType type)
        {
            var i = (int) type;
            if (Enum.IsDefined(typeof(TemplateMatchModes), i))
            {
                return (TemplateMatchModes) Enum.ToObject(typeof(TemplateMatchModes), i);
            }

            return TemplateMatchModes.CCoeffNormed;
        }
    }
}