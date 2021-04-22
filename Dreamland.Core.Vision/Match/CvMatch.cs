using System;
using System.Drawing;
using System.IO;
using OpenCvSharp;

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
        /// <param name="threshold"> 相似度匹配的阈值
        ///     <para>
        ///         在<see cref="TemplateMatchType.SQDIFF"/>和<see cref="TemplateMatchType.SQDIFF_NORMED"/>模式下，当相识度大于该阈值的时候，就忽略掉；
        ///         在其他<see cref="TemplateMatchType"/>模式下，当相识度小于该阈值的时候，就忽略掉；
        ///     </para>
        /// </param>
        /// <param name="maxCount">最大的匹配数</param>
        /// <param name="type">匹配算法</param>
        /// <returns></returns>
        public static TemplateMatchResult TemplateMatch(string sourceImage, string searchImage, double threshold = 0.5, uint maxCount = 1, TemplateMatchType type = TemplateMatchType.CCOEFF_NORMED)
        {
            if (!File.Exists(sourceImage))
            {
                throw new FileNotFoundException(sourceImage);
            }

            if (!File.Exists(searchImage))
            {
                throw new FileNotFoundException(searchImage);
            }

            using var sourceMat = new Mat(sourceImage, ImreadModes.AnyColor);
            using var searchMat = new Mat(searchImage, ImreadModes.AnyColor);

            if (sourceMat.Width < searchMat.Width || sourceMat.Height < searchMat.Height)
            {
                throw new ArgumentException("对应的训练（模板）图像sourceImage，宽高不得超过searchImage。");
            }
            
            return TemplateMatch(sourceMat, searchMat, threshold, maxCount, type);
        }

        /// <summary>
        ///     进行模版匹配
        /// </summary>
        /// <param name="sourceImage">对应的查询（原始）图像</param>
        /// <param name="searchImage">对应的训练（模板）图像（宽高不得超过被查询图像）</param>
        /// <param name="threshold"> 相似度匹配的阈值
        ///     <para>
        ///         在<see cref="TemplateMatchType.SQDIFF"/>和<see cref="TemplateMatchType.SQDIFF_NORMED"/>模式下，当相识度大于该阈值的时候，就忽略掉；
        ///         在其他<see cref="TemplateMatchType"/>模式下，当相识度小于该阈值的时候，就忽略掉；
        ///     </para>
        /// </param>
        /// <param name="maxCount">最大的匹配数</param>
        /// <param name="type">匹配算法</param>
        /// <returns></returns>
        public static TemplateMatchResult TemplateMatch(Bitmap sourceImage, Bitmap searchImage, double threshold = 0.5, uint maxCount = 1, TemplateMatchType type = TemplateMatchType.CCOEFF_NORMED)
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
            
            var matchModes = Match.TemplateMatch.ConvertToMatchModes(type);
            if (sourceMat.Type() != searchMat.Type())
            {
                using var sourceMatC1 = new Mat(sourceMat.Rows, sourceMat.Cols, MatType.CV_8UC1);
                using var searchMatC1 = new Mat(searchMat.Rows, searchMat.Cols, MatType.CV_8UC1);
                Cv2.CvtColor(sourceMat, sourceMatC1, ColorConversionCodes.BGR2GRAY);
                Cv2.CvtColor(searchMat, searchMatC1, ColorConversionCodes.BGR2GRAY);
                return TemplateMatch(sourceMatC1, searchMatC1, threshold, maxCount, type);
            }
            else
            {
                return TemplateMatch(sourceMat, searchMat, threshold, maxCount, type);
            }
        }

        /// <summary>
        ///     进行模版匹配
        /// </summary>
        /// <param name="sourceImageData">对应的查询（原始）图像</param>
        /// <param name="searchImageData">对应的训练（模板）图像（宽高不得超过被查询图像）</param>
        /// <param name="threshold"> 相似度匹配的阈值
        ///     <para>
        ///         在<see cref="TemplateMatchType.SQDIFF"/>和<see cref="TemplateMatchType.SQDIFF_NORMED"/>模式下，当相识度大于该阈值的时候，就忽略掉；
        ///         在其他<see cref="TemplateMatchType"/>模式下，当相识度小于该阈值的时候，就忽略掉；
        ///     </para>
        /// </param>
        /// <param name="maxCount">最大的匹配数</param>
        /// <param name="type">匹配算法</param>
        /// <returns></returns>
        public static TemplateMatchResult TemplateMatch(byte[] sourceImageData, byte[] searchImageData, double threshold = 0.5, uint maxCount = 1, TemplateMatchType type = TemplateMatchType.CCOEFF_NORMED)
        {
            if (sourceImageData == null)
            {
                throw new ArgumentNullException(nameof(sourceImageData));
            }

            if (searchImageData == null)
            {
                throw new ArgumentNullException(nameof(searchImageData));
            }

            using var sourceMat = Mat.FromImageData(sourceImageData, ImreadModes.AnyColor);
            using var searchMat = Mat.FromImageData(searchImageData, ImreadModes.AnyColor);

            if (sourceMat.Width < searchMat.Width || sourceMat.Height < searchMat.Height)
            {
                throw new ArgumentException("对应的训练（模板）图像searchImageData，宽高不得超过sourceImageData。");
            }
            
            return TemplateMatch(sourceMat, searchMat, threshold, maxCount, type);
        }

        /// <summary>
        ///     进行模版匹配
        /// </summary>
        /// <param name="sourceImage">对应的查询（原始）图像</param>
        /// <param name="searchImage">对应的训练（模板）图像（宽高不得超过被查询图像）</param>
        /// <param name="threshold"> 相似度匹配的阈值
        ///     <para>
        ///         在<see cref="TemplateMatchType.SQDIFF"/>和<see cref="TemplateMatchType.SQDIFF_NORMED"/>模式下，当相识度大于该阈值的时候，就忽略掉；
        ///         在其他<see cref="TemplateMatchType"/>模式下，当相识度小于该阈值的时候，就忽略掉；
        ///     </para>
        /// </param>
        /// <param name="maxCount">最大的匹配数</param>
        /// <param name="type">匹配算法</param>
        /// <returns></returns>
        public static TemplateMatchResult TemplateMatch(Mat sourceImage, Mat searchImage, double threshold = 0.5, uint maxCount = 1, TemplateMatchType type = TemplateMatchType.CCOEFF_NORMED)
        {
            var matchModes = Match.TemplateMatch.ConvertToMatchModes(type);
            return Match.TemplateMatch.Match(sourceImage, searchImage, threshold, maxCount, matchModes);
        }

        /// <summary>
        ///     进行特征点匹配
        /// </summary>
        /// <param name="sourceImage">对应的查询（原始）图像</param>
        /// <param name="searchImage">对应的训练（模板）图像（宽高不得超过被查询图像）</param>
        /// <param name="featureMatchType">特征点匹配算法</param>
        /// <param name="argument">匹配参数（可选）</param>
        /// <returns></returns>
        internal static FeatureMatchResult FeatureMatch(string sourceImage, string searchImage, FeatureMatchType featureMatchType = FeatureMatchType.Sift, FeatureMatchArgument argument = null)
        {
            if (!File.Exists(sourceImage))
            {
                throw new FileNotFoundException(sourceImage);
            }

            if (!File.Exists(searchImage))
            {
                throw new FileNotFoundException(searchImage);
            }

            using var sourceMat = new Mat(sourceImage, ImreadModes.AnyColor);
            using var searchMat = new Mat(searchImage, ImreadModes.AnyColor);

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
        internal static FeatureMatchResult FeatureMatch(Bitmap sourceImage, Bitmap searchImage, FeatureMatchType featureMatchType = FeatureMatchType.Sift, FeatureMatchArgument argument = null)
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
            
            if (sourceMat.Type() != searchMat.Type())
            {
                using var sourceMatC1 = new Mat(sourceMat.Rows, sourceMat.Cols, MatType.CV_8UC1);
                using var searchMatC1 = new Mat(searchMat.Rows, searchMat.Cols, MatType.CV_8UC1);
                Cv2.CvtColor(sourceMat, sourceMatC1, ColorConversionCodes.BGR2GRAY);
                Cv2.CvtColor(searchMat, searchMatC1, ColorConversionCodes.BGR2GRAY);
                return FeatureMatch(sourceMatC1, searchMatC1, featureMatchType, argument);
            }
            else
            {
                return FeatureMatch(sourceMat, searchMat, featureMatchType, argument);
            }
        }

        /// <summary>
        ///     进行特征点匹配
        /// </summary>
        /// <param name="sourceImageData">对应的查询（原始）图像</param>
        /// <param name="searchImageData">对应的训练（模板）图像（宽高不得超过被查询图像）</param>
        /// <param name="featureMatchType">特征点匹配算法</param>
        /// <param name="argument">匹配参数（可选）</param>
        /// <returns></returns>
        internal static FeatureMatchResult FeatureMatch(byte[] sourceImageData, byte[] searchImageData, FeatureMatchType featureMatchType = FeatureMatchType.Sift, FeatureMatchArgument argument = null)
        {
            if (sourceImageData == null)
            {
                throw new ArgumentNullException(nameof(sourceImageData));
            }

            if (searchImageData == null)
            {
                throw new ArgumentNullException(nameof(searchImageData));
            }

            using var sourceMat = Mat.FromImageData(sourceImageData, ImreadModes.AnyColor);
            using var searchMat = Mat.FromImageData(searchImageData, ImreadModes.AnyColor);

            if (sourceMat.Width < searchMat.Width || sourceMat.Height < searchMat.Height)
            {
                throw new ArgumentException("对应的训练（模板）图像searchImageData，宽高不得超过sourceImageData。");
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
        internal static FeatureMatchResult FeatureMatch(Mat sourceImage, Mat searchImage,
            FeatureMatchType featureMatchType = FeatureMatchType.Sift, FeatureMatchArgument argument = null)
        {
            return argument == null 
                ? Match.FeatureMatch.Match(sourceImage, searchImage, featureMatchType) 
                : Match.FeatureMatch.Match(sourceImage, searchImage, featureMatchType, argument);
        }
    }
}
