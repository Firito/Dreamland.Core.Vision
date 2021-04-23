using System;
using System.Collections.Generic;
using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Features2D;

namespace Dreamland.Core.Vision.Match
{
    /// <summary>
    ///     提供基于 <see cref="Dreamland.Core.Vision.Match.FeatureMatchType.Sift"/> 算法的特征点匹配
    /// </summary>
    [FeatureProviderType(FeatureMatchType.Sift, Description = "使用Sift算法进行特征点匹配。")]
    internal class SiftFeatureProvider : FeatureProvider
    {
        /// <summary>
        ///     Homography算法最低要求的Point点数
        /// </summary>
        /// <value>The input arrays should have at least 4 corresponding point sets to calculate Homography</value>
        private const int MinCorrespondingPointCount = 4; 

        public override FeatureMatchResult Match(Mat sourceMat, Mat searchMat, FeatureMatchArgument argument)
        {
            //创建SIFT
            using var sift = SIFT.Create();

            //创建特征点描述对象，为下边的特征点匹配做准备
            using var sourceDescriptors = new Mat();
            using var searchDescriptors = new Mat();

            //提取特征点，并进行特征点描述
            sift.DetectAndCompute(sourceMat, null, out var sourceKeyPoints, sourceDescriptors);
            sift.DetectAndCompute(searchMat, null, out var searchKeyPoints, searchDescriptors);

            //创建Brute-force descriptor matcher
            using var bfMatcher = new BFMatcher();
            //对原图特征点描述加入训练
            bfMatcher.Add(new List<Mat>() {sourceDescriptors});
            bfMatcher.Train();
            //获得匹配特征点，并提取最优配对
            var matches = bfMatcher.KnnMatch(sourceDescriptors, searchDescriptors, (int) argument.MatchPoints);

            //即使使用SIFT算法，但此时没有经过点筛选的匹配效果同样糟糕，所进一步获取优秀匹配点
            var goodMatches = SelectGoodMatches(matches, argument, sourceKeyPoints, searchKeyPoints);
            Console.WriteLine($"SIFT FeatureMatch points count : {goodMatches.Count}");

            //获取匹配结果
            var matchResult = GetMatchResult(goodMatches, sourceKeyPoints, searchKeyPoints);

            //如果开启了匹配结果预览，则显示匹配结果
            if (argument.ExtensionConfig != null &&
                argument.ExtensionConfig.TryGetValue("PreviewMatchResult", out var isEnabled) && isEnabled is true)
            {
                //调试模式下，查看一下当前阶段的匹配结果
                MatchHelper.PreviewFeatureMatchResult(matchResult, sourceMat, searchMat, sourceKeyPoints, searchKeyPoints, goodMatches);
            }

            return matchResult;
        }

        /// <summary>
        ///     获取优秀的匹配点
        /// <para>
        ///     使用 "比较最近邻距离与次近邻距离的SIFT匹配方式" 和 "随机抽样一致(RANSAC)算法" 来进一步获取优秀匹配点
        /// </para>
        /// </summary>
        /// <param name="matches"></param>
        /// <param name="argument"></param>
        /// <param name="sourceKeyPoints"></param>
        /// <param name="searchKeyPoints"></param>
        /// <returns></returns>
        private IList<DMatch> SelectGoodMatches(IEnumerable<DMatch[]> matches, FeatureMatchArgument argument,
            IList<KeyPoint> sourceKeyPoints, IList<KeyPoint> searchKeyPoints)
        {
            var sourcePoints = new List<Point2d>();
            var searchPoints = new List<Point2d>();
            var goodMatches = new List<DMatch>();

            //比较最近邻距离与次近邻距离的SIFT匹配方式
            foreach (var items in matches)
            {
                if (items.Length == 0)
                {
                    continue;
                }

                if (argument.MatchPoints > 1 &&
                    (items.Length < 2 || items[0].Distance > argument.Ratio * items[1].Distance))
                {
                    continue;
                }

                //此处直接选择欧氏距离小的匹配关键点
                var goodMatch = items[0];
                goodMatches.Add(goodMatch);
                sourcePoints.Add(Point2FToPoint2D(sourceKeyPoints[goodMatch.QueryIdx].Pt));
                searchPoints.Add(Point2FToPoint2D(searchKeyPoints[goodMatch.TrainIdx].Pt));
            }

            //随机抽样一致(RANSAC)算法 (如果原始的匹配结果为空, 则跳过过滤步骤）
            if (sourcePoints.Count >= MinCorrespondingPointCount && searchPoints.Count >= MinCorrespondingPointCount)
            {
                var inliersMask = new Mat();
                Cv2.FindHomography(sourcePoints, searchPoints, HomographyMethods.Ransac, argument.RansacThreshold,
                    mask: inliersMask);
                // 如果通过RANSAC处理后的匹配点大于10个,才应用过滤. 否则使用原始的匹配点结果(匹配点过少的时候通过RANSAC处理后,可能会得到0个匹配点的结果).
                if (inliersMask.Rows > 10)
                {
                    inliersMask.GetArray(out byte[] maskBytes);
                    var inliers = new List<DMatch>();

                    for (var i = 0; i < maskBytes.Length; i++)
                    {
                        if (maskBytes[i] != 0)
                        {
                            inliers.Add(goodMatches[i]);
                        }
                    }

                    return inliers;
                }
            }

            return goodMatches;
        }

        private FeatureMatchResult GetMatchResult(IList<DMatch> matches, IList<KeyPoint> sourceKeyPoints,
            IList<KeyPoint> searchKeyPoints)
        {
            //至少识别3个点才能得到一个几何图形
            var success = matches.Count > 3;
            var matchResult = new FeatureMatchResult()
            {
                Success = success
            };

            if (!success)
            {
                return matchResult;
            }

            var goodSourceKeyPoints = new List<KeyPoint>();
            var goodSearchKeyPoints = new List<KeyPoint>();

            var points = new List<System.Drawing.Point>();

            double x = double.MaxValue, x1 = 0, y = double.MaxValue, y1 = 0;
            foreach (var match in matches)
            {
                goodSourceKeyPoints.Add(sourceKeyPoints[match.QueryIdx]);
                goodSearchKeyPoints.Add(searchKeyPoints[match.TrainIdx]);

                var point = sourceKeyPoints[match.QueryIdx].Pt;
                points.Add(new System.Drawing.Point((int) point.X, (int) point.Y));

                x = Math.Min(point.X, x);
                x1 = Math.Max(point.X, x1);
                y = Math.Min(point.Y, y);
                y1 = Math.Max(point.Y, y1);
            }

            var leftTop = new System.Drawing.Point((int) Math.Min(x, x1), (int) Math.Min(y, y1));
            var size = new System.Drawing.Size((int) Math.Abs(x - x1), (int) Math.Abs(y - y1));
            matchResult.MatchItems.Add(new FeatureMatchResultItem()
            {
                Point = new System.Drawing.Point((int) (x + (double) size.Width / 2),
                    (int) (y + (double) size.Height / 2)),
                FeaturePoints = points,
                Rectangle = new Rectangle(leftTop, size),
            });
            return matchResult;
        }
    }
}