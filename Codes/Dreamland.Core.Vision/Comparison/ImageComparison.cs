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
            if (argument.Type == ComparisonSimilarityType.HASH_GRAY)
            {
                return CompareSimilarity(image1, image2, argument.Threshold);
            }

            return 0;
        }

        private static double CompareSimilarity(string source, string search, double threshold)
        {
            const int sizeValue = 16;
            var hashes1 = GetGrayHash(source, new Size(sizeValue, sizeValue));
            var hashes2 = GetGrayHash(search, new Size(sizeValue, sizeValue));
            var equalElements = hashes1.Zip(hashes2, (i, j) => i < threshold == j < threshold).Count(eq => eq);
            var percentage = equalElements / Math.Pow(sizeValue, 2);
            return Math.Round(percentage, 2);
        }

        /// <summary>
        ///     获取Hash(灰度值)
        /// </summary>
        /// <param name="bitmapPath"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private static List<double> GetGrayHash(string bitmapPath, Size size)
        {
            using var bitmap = new Bitmap(bitmapPath);
            using var bmpMin = new Bitmap(bitmap, size);
            var listResult = new List<double>(); 
            for (var i = 0; i < bmpMin.Height; i++)
            {
                for (var j = 0; j < bmpMin.Width; j++)
                {
                    var gray = GetGray(bmpMin.GetPixel(i, j));
                    listResult.Add(gray / 255d);
                }
            }
            return listResult;
        }

        private static double GetGray(Color color)
        {
            return 0.30 * color.R + 0.59 * color.G + 0.11 * color.B;
        }
    }
}
