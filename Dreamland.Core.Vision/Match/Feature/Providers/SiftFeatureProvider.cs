using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenCvSharp;
using OpenCvSharp.Features2D;

namespace Dreamland.Core.Vision.Match
{
    /// <summary>
    ///     提供基于 <see cref="Dreamland.Core.Vision.Match.MatchFeatureType.Sift"/> 算法的特征点匹配
    /// </summary>
    [FeatureProviderType(MatchFeatureType.Sift, Description = "使用Sift算法进行特征点匹配。")]
    internal class SiftFeatureProvider : FeatureProvider
    {
        public override MatchResult Match(Mat sourceMat, Mat searchMat, double ratio, uint matchPoints)
        {
            //创建SIFT
            using var sift = SIFT.Create();

            //创建特征点描述对象，为下边的特征点匹配做准备
            using var sourceDescriptors = new Mat();
            using var searchDescriptors = new Mat();

            //提取特征点，并进行特征点描述
            sift.DetectAndCompute(sourceMat, null, out var keySourcePoints, sourceDescriptors);
            sift.DetectAndCompute(searchMat, null, out var keySearchPoints, searchDescriptors);

            //创建Brute-force descriptor matcher
            using var bfMatcher = new BFMatcher();
            //对原图特征点描述加入训练
            bfMatcher.Add(new List<Mat>() {sourceDescriptors});
            bfMatcher.Train();
            //获得匹配特征点，并提取最优配对
            var matches = bfMatcher.KnnMatch(sourceDescriptors, searchDescriptors, (int)matchPoints);

            //即使使用SIFT算法，但此时没有经过点筛选的匹配效果同样糟糕，所进一步获取优秀匹配点
            var goodMatches = SelectGoodMatches(matches, ratio, matchPoints, keySourcePoints, keySearchPoints);
            Console.WriteLine($"SIFT FeatureMatch points count : {goodMatches.Count}");
            
            if (Debugger.IsAttached)
            {
                //调试模式下，查看一下当前阶段的匹配结果
                PreviewMathResult(sourceMat, searchMat, keySourcePoints, keySearchPoints, goodMatches);
            }
            
            //获取匹配结果
            return GetMatchResult(goodMatches, keySourcePoints);
        }

        /// <summary>
        ///     获取优秀的匹配点
        /// <para>
        ///     使用 "比较最近邻距离与次近邻距离的SIFT匹配方式" 和 "随机抽样一致(RANSAC)算法" 来进一步获取优秀匹配点
        /// </para>
        /// </summary>
        /// <param name="matches"></param>
        /// <param name="ratio"></param>
        /// <param name="matchPoints"></param>
        /// <param name="keySourcePoints"></param>
        /// <param name="keySearchPoints"></param>
        /// <returns></returns>
        private IList<DMatch[]> SelectGoodMatches(IEnumerable<DMatch[]> matches, double ratio, uint matchPoints, IList<KeyPoint> keySourcePoints, IList<KeyPoint> keySearchPoints)
        {
            var sourcePoints = new List<Point2d>();
            var searchPoints = new List<Point2d>();
            var goodMatches = new List<DMatch[]>();

            //比较最近邻距离与次近邻距离的SIFT匹配方式
            foreach (var items in matches)
            {
                if (matchPoints > 1 && (items.Length < 2 || items[0].Distance > ratio * items[1].Distance))
                {
                    continue;
                }

                goodMatches.Add(items);
                sourcePoints.Add(Point2FToPoint2D(keySourcePoints[items[0].QueryIdx].Pt));
                searchPoints.Add(Point2FToPoint2D(keySearchPoints[items[0].TrainIdx].Pt));
            }

            //随机抽样一致(RANSAC)算法 (如果原始的匹配结果为空, 则跳过过滤步骤）
            if (sourcePoints.Count > 0 && searchPoints.Count > 0)
            {
                var inliersMask = new Mat();
                Cv2.FindHomography(sourcePoints, searchPoints, HomographyMethods.Ransac, mask: inliersMask);
                // 如果通过RANSAC处理后的匹配点大于10个,才应用过滤. 否则使用原始的匹配点结果(匹配点过少的时候通过RANSAC处理后,可能会得到0个匹配点的结果).
                if (inliersMask.Rows > 10)
                {
                    inliersMask.GetArray(out byte[] maskBytes);
                    var inliers = new List<DMatch[]>();

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

        private MatchResult GetMatchResult(IList<DMatch[]> matches, IList<KeyPoint> keySourcePoints)
        {
            //至少识别3个点才能得到一个几何图形
            var success = matches.Count > 3;
            var matchResult = new MatchResult()
            {
                Success = success
            };

            if (!success)
            {
                return matchResult;
            }


            foreach (var match in matches)
            {
                var point = keySourcePoints[match[0].QueryIdx].Pt;
            }
            return null;
        }
    }
}