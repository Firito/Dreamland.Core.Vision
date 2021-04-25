using OpenCvSharp;
using System;
using System.Drawing;
using System.IO;

namespace Dreamland.Core.Vision.Match
{
    /// <summary>
    ///     提供基于OpenCv的图像匹配方法
    /// </summary>
    public static class CvMatch
    {
        /// <summary>
        ///     进行模版匹配
        /// </summary>
        /// <param name="sourceImage">对应的查询（原始）图像</param>
        /// <param name="searchImage">对应的训练（模板）图像（宽高不得超过被查询图像）</param>
        /// <param name="type">匹配算法</param>
        /// <param name="argument">匹配参数（可选）</param>
        /// <returns></returns>
        public static TemplateMatchResult TemplateMatch(string sourceImage, string searchImage, TemplateMatchType type = TemplateMatchType.CCOEFF_NORMED, TemplateMatchArgument argument = null)
        {
            if (!File.Exists(sourceImage))
            {
                throw new FileNotFoundException(sourceImage);
            }

            if (!File.Exists(searchImage))
            {
                throw new FileNotFoundException(searchImage);
            }

            using var sourceMat = new Mat(sourceImage);
            using var searchMat = new Mat(searchImage);

            if (sourceMat.Width < searchMat.Width || sourceMat.Height < searchMat.Height)
            {
                throw new ArgumentException("对应的训练（模板）图像sourceImage，宽高不得超过searchImage。");
            }

            return TemplateMatch(sourceMat, searchMat, type, argument);
        }

        /// <summary>
        ///     进行模版匹配
        /// </summary>
        /// <param name="sourceImage">对应的查询（原始）图像</param>
        /// <param name="searchImage">对应的训练（模板）图像（宽高不得超过被查询图像）</param>
        /// <param name="type">匹配算法</param>
        /// <param name="argument">匹配参数（可选）</param>
        /// <returns></returns>
        public static TemplateMatchResult TemplateMatch(Bitmap sourceImage, Bitmap searchImage, TemplateMatchType type = TemplateMatchType.CCOEFF_NORMED, TemplateMatchArgument argument = null)
        {
            if (sourceImage == null)
            {
                throw new ArgumentNullException(nameof(sourceImage));
            }

            if (searchImage == null)
            {
                throw new ArgumentNullException(nameof(searchImage));
            }

            using var sourceMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(sourceImage);
            using var searchMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(searchImage);

            if (sourceMat.Width < searchMat.Width || sourceMat.Height < searchMat.Height)
            {
                throw new ArgumentException("对应的训练（模板）图像sourceImage，宽高不得超过searchImage。");
            }

            return TemplateMatch(sourceMat, searchMat, type, argument);
        }

        /// <summary>
        ///     进行模版匹配
        /// </summary>
        /// <param name="sourceImage">对应的查询（原始）图像</param>
        /// <param name="searchImage">对应的训练（模板）图像（宽高不得超过被查询图像）</param>
        /// <param name="type">匹配算法</param>
        /// <param name="argument">匹配参数（可选）</param>
        /// <returns></returns>
        public static TemplateMatchResult TemplateMatch(Mat sourceImage, Mat searchImage, TemplateMatchType type = TemplateMatchType.CCOEFF_NORMED, TemplateMatchArgument argument = null)
        {
            if (searchImage.Empty())
            {
                throw new ArgumentOutOfRangeException(nameof(searchImage), "不允许使用空的查询（原始）图像进行识别");
            }

            if (sourceImage.Empty())
            {
                throw new ArgumentOutOfRangeException(nameof(searchImage), "不允许使用空的查询（原始）图像进行识别");
            }

            if (sourceImage.Type() == searchImage.Type())
                return Match.TemplateMatch.Match(sourceImage, searchImage, type, argument);

            using var sourceMat = new Mat(sourceImage.Rows, sourceImage.Cols, MatType.CV_8UC1);
            using var searchMat = new Mat(searchImage.Rows, searchImage.Cols, MatType.CV_8UC1);
            Cv2.CvtColor(sourceImage, sourceMat, ColorConversionCodes.BGR2GRAY);
            Cv2.CvtColor(searchImage, searchMat, ColorConversionCodes.BGR2GRAY);
            return Match.TemplateMatch.Match(sourceMat, searchImage, type, argument);
        }

        /// <summary>
        ///     进行特征点匹配
        /// </summary>
        /// <param name="sourceImage">对应的查询（原始）图像</param>
        /// <param name="searchImage">对应的训练（模板）图像（宽高不得超过被查询图像）</param>
        /// <param name="featureMatchType">特征点匹配算法</param>
        /// <param name="argument">匹配参数（可选）</param>
        /// <returns></returns>
        public static FeatureMatchResult FeatureMatch(string sourceImage, string searchImage, FeatureMatchType featureMatchType = FeatureMatchType.Sift, FeatureMatchArgument argument = null)
        {
            if (!File.Exists(sourceImage))
            {
                throw new FileNotFoundException(sourceImage);
            }

            if (!File.Exists(searchImage))
            {
                throw new FileNotFoundException(searchImage);
            }

            using var sourceMat = new Mat(sourceImage);
            using var searchMat = new Mat(searchImage);

            if (sourceMat.Width < searchMat.Width || sourceMat.Height < searchMat.Height)
            {
                throw new ArgumentException("对应的训练（模板）图像sourceImage，宽高不得超过searchImage。");
            }

            return FeatureMatch(sourceMat, searchMat, featureMatchType, argument);
        }

        /// <summary>
        ///     进行特征点匹配
        /// </summary>
        /// <param name="sourceImage">对应的查询（原始）图像</param>
        /// <param name="searchImage">对应的训练（模板）图像（宽高不得超过被查询图像）</param>
        /// <param name="featureMatchType">特征点匹配算法</param>
        /// <param name="argument">匹配参数（可选）</param>
        /// <returns></returns>
        public static FeatureMatchResult FeatureMatch(Bitmap sourceImage, Bitmap searchImage, FeatureMatchType featureMatchType = FeatureMatchType.Sift, FeatureMatchArgument argument = null)
        {
            if (sourceImage == null)
            {
                throw new ArgumentNullException(nameof(sourceImage));
            }

            if (searchImage == null)
            {
                throw new ArgumentNullException(nameof(searchImage));
            }

            using var sourceMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(sourceImage);
            using var searchMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(searchImage);

            if (sourceMat.Width < searchMat.Width || sourceMat.Height < searchMat.Height)
            {
                throw new ArgumentException("对应的训练（模板）图像sourceImage，宽高不得超过searchImage。");
            }

            return FeatureMatch(sourceMat, searchMat, featureMatchType, argument);
        }

        /// <summary>
        ///     进行特征点匹配
        /// </summary>
        /// <param name="sourceImage">对应的查询（原始）图像</param>
        /// <param name="searchImage">对应的训练（模板）图像（宽高不得超过被查询图像）</param>
        /// <param name="featureMatchType">特征点匹配算法</param>
        /// <param name="argument">匹配参数（可选）</param>
        /// <returns></returns>
        public static FeatureMatchResult FeatureMatch(Mat sourceImage, Mat searchImage,
            FeatureMatchType featureMatchType = FeatureMatchType.Sift, FeatureMatchArgument argument = null)
        {
            if (searchImage.Empty())
            {
                throw new ArgumentOutOfRangeException(nameof(searchImage), "不允许使用空的查询（原始）图像进行识别");
            }

            if (sourceImage.Empty())
            {
                throw new ArgumentOutOfRangeException(nameof(searchImage), "不允许使用空的查询（原始）图像进行识别");
            }

            if (sourceImage.Type() == searchImage.Type())
                return argument == null
                    ? Match.FeatureMatch.Match(sourceImage, searchImage, featureMatchType)
                    : Match.FeatureMatch.Match(sourceImage, searchImage, featureMatchType, argument);

            using var sourceMat = new Mat(sourceImage.Rows, sourceImage.Cols, MatType.CV_8UC1);
            using var searchMat = new Mat(searchImage.Rows, searchImage.Cols, MatType.CV_8UC1);
            Cv2.CvtColor(sourceImage, sourceMat, ColorConversionCodes.BGR2GRAY);
            Cv2.CvtColor(searchImage, searchMat, ColorConversionCodes.BGR2GRAY);
            return argument == null
                ? Match.FeatureMatch.Match(sourceMat, sourceMat, featureMatchType)
                : Match.FeatureMatch.Match(sourceMat, sourceMat, featureMatchType, argument);
        }
    }
}
