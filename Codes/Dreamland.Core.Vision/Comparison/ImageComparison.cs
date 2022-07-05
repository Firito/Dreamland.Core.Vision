using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dreamland.Core.Vision.Comparison
{
    /// <summary>
    ///     图片比较相似度
    /// </summary>
    public static class ImageComparison
    {
        /// <summary>
        ///     比较图像对比度
        /// </summary>
        /// <param name="image1">对应的比较图像1</param>
        /// <param name="image2">对应的比较图像2</param>
        /// <returns>相似度（0-1），越接近1表示越相似</returns>
        public static double CompareSimilarity(string image1, string image2)
        {
            return CompareSimilarity(image1, image2, new ComparisonArgument());
        }

        /// <summary>
        ///     比较图像对比度
        /// </summary>
        /// <param name="image1">对应的比较图像1</param>
        /// <param name="image2">对应的比较图像2</param>
        /// <param name="argument">比较参数</param>
        /// <returns>相似度（0-1），越接近1表示越相似</returns>
        public static double CompareSimilarity(string image1, string image2, ComparisonArgument argument)
        {
            switch (argument.Type)
            {
                case ComparisonSimilarityType.HASH_BRIGHTNESS:
                    return CompareHashBrightness(image1, image2, argument.Threshold);
                case ComparisonSimilarityType.HASH_GRAY:
                    return CompareHashGray(image1, image2, argument.Threshold);
                case ComparisonSimilarityType.EUCLIDEAN_DISTANCE:
                    return CompareEuclideanDistance(image1, image2, argument.Threshold);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 比较图像哈希(亮度值)
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        private static double CompareHashBrightness(string image1, string image2, double threshold)
        {
            const int sizeValue = 16;
            using var bitmap1 = new Bitmap(image1);
            using var bitmap2 = new Bitmap(image2);
            var colors1 = GetColors(bitmap1, new Size(sizeValue, sizeValue));
            var colors2 = GetColors(bitmap2, new Size(sizeValue, sizeValue));
            var equalElements = colors1.Zip(colors2, (i, j) =>
            {
                threshold *= byte.MaxValue;
                var brightness1 = i.GetBrightness();
                var brightness2 = j.GetBrightness();
                return (brightness1 < threshold && brightness2 < threshold) || (brightness1 > threshold && brightness2 > threshold);
            }).Count(eq => eq);
            var percentage = equalElements / Math.Pow(sizeValue, 2);
            return Math.Round(percentage, 2);
        }

        /// <summary>
        /// 比较图像哈希(灰度值)
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        private static double CompareHashGray(string image1, string image2, double threshold)
        {
            const int sizeValue = 32;
            using var bitmap1 = new Bitmap(image1);
            using var bitmap2 = new Bitmap(image2);
            var colors1 = GetColors(bitmap1, new Size(sizeValue, sizeValue));
            var colors2 = GetColors(bitmap2, new Size(sizeValue, sizeValue));
            var equalElements = colors1.Zip(colors2, (i, j) => Math.Abs(GetGray(i) - GetGray(j)) < threshold * 64).Count(eq => eq);
            var percentage = equalElements / Math.Pow(sizeValue, 2);
            return Math.Round(percentage, 2);
        }

        /// <summary>
        /// 比较图像的像素欧氏距离
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        private static double CompareEuclideanDistance(string image1, string image2, double threshold)
        {
            using var bitmap1 = new Bitmap(image1);
            using var bitmap2 = new Bitmap(image2);

            var width = Math.Min(bitmap1.Width, bitmap2.Width);
            var height = Math.Min(bitmap1.Height, bitmap2.Height);
            var size = new Size(width, height);

            var colors1 = GetColors(bitmap1, size);
            var colors2 = GetColors(bitmap2, size);
            var equalElements = colors1.Zip(colors2, (i, j) => GetEuclideanDistance(i, j) < threshold * 16).Count(eq => eq);
            var percentage = equalElements / (double)(size.Width * size.Height);
            return Math.Round(percentage, 2);
        }

        /// <summary>
        ///     获取缩小图片后的颜色值
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private static List<Color> GetColors(Bitmap bitmap, Size size)
        {
            using var bmpMin = new Bitmap(bitmap, size);
            var listResult = new List<Color>(); 
            for (var i = 0; i < bmpMin.Width; i++)
            {
                for (var j = 0; j < bmpMin.Height; j++)
                {
                    var color = bmpMin.GetPixel(i, j);
                    listResult.Add(color);
                }
            }
            return listResult;
        }

        /// <summary>
        /// 获取颜色灰度值
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private static double GetGray(Color color)
        {
            return 0.30 * color.R + 0.59 * color.G + 0.11 * color.B;
        }

        /// <summary>
        /// 在RGB空间上通过公式计算出加权的欧式距离
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static double GetEuclideanDistance(Color x, Color y)
        {
            var m = (x.R + y.R) / 2.0;
            var r = Math.Pow(x.R - y.R, 2);
            var g = Math.Pow(x.G - y.G, 2);
            var b = Math.Pow(x.B - y.B, 2);

            return Math.Sqrt((2 + m / 256) * r + 4 * g + (2 + (255 - m) / 256) * b);
        }
    }
}
