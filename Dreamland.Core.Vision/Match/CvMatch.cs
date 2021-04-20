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
        /// <param name="sourceImage">原始图片</param>
        /// <param name="searchImage">需要查找的图片（宽高不得超过<see cref="sourceImage"/>）</param>
        /// <param name="threshold"> 相似度匹配的阈值
        ///     <para>
        ///         在<see cref="MatchTemplateType.SQDIFF"/>和<see cref="MatchTemplateType.SQDIFF_NORMED"/>模式下，当相识度大于该阈值的时候，就忽略掉；
        ///         在其他<see cref="MatchTemplateType"/>模式下，当相识度小于该阈值的时候，就忽略掉；
        ///     </para>
        /// </param>
        /// <param name="maxCount">最大的匹配数</param>
        /// <param name="type">匹配算法</param>
        /// <returns></returns>
        public static MatchResult TemplateMatch(string sourceImage, string searchImage, double threshold = 0.5, uint maxCount = 1, MatchTemplateType type = MatchTemplateType.CCOEFF_NORMED)
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
                throw new ArgumentException("需要查找的图片sourceImage，宽高不得超过searchImage。");
            }
            
            var matchModes = MatchTemplate.ConvertToMatchModes(type);
            return MatchTemplate.Match(sourceMat, searchMat, threshold, maxCount, matchModes);
        }

        /// <summary>
        ///     进行模版匹配
        /// </summary>
        /// <param name="sourceImage">原始图片</param>
        /// <param name="searchImage">需要查找的图片（宽高不得超过<see cref="sourceImage"/>）</param>
        /// <param name="threshold"> 相似度匹配的阈值
        ///     <para>
        ///         在<see cref="MatchTemplateType.SQDIFF"/>和<see cref="MatchTemplateType.SQDIFF_NORMED"/>模式下，当相识度大于该阈值的时候，就忽略掉；
        ///         在其他<see cref="MatchTemplateType"/>模式下，当相识度小于该阈值的时候，就忽略掉；
        ///     </para>
        /// </param>
        /// <param name="maxCount">最大的匹配数</param>
        /// <param name="type">匹配算法</param>
        /// <returns></returns>
        public static MatchResult TemplateMatch(Bitmap sourceImage, Bitmap searchImage, double threshold = 0.5, uint maxCount = 1, MatchTemplateType type = MatchTemplateType.CCOEFF_NORMED)
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
                throw new ArgumentException("需要查找的图片sourceImage，宽高不得超过searchImage。");
            }
            
            var matchModes = MatchTemplate.ConvertToMatchModes(type);
            return MatchTemplate.Match(sourceMat, searchMat, threshold, maxCount, matchModes);
        }

        /// <summary>
        ///     进行模版匹配
        /// </summary>
        /// <param name="sourceImageData">原始图片</param>
        /// <param name="searchImageData">需要查找的图片（宽高不得超过<see cref="sourceImageData"/>）</param>
        /// <param name="threshold"> 相似度匹配的阈值
        ///     <para>
        ///         在<see cref="MatchTemplateType.SQDIFF"/>和<see cref="MatchTemplateType.SQDIFF_NORMED"/>模式下，当相识度大于该阈值的时候，就忽略掉；
        ///         在其他<see cref="MatchTemplateType"/>模式下，当相识度小于该阈值的时候，就忽略掉；
        ///     </para>
        /// </param>
        /// <param name="maxCount">最大的匹配数</param>
        /// <param name="type">匹配算法</param>
        /// <returns></returns>
        public static MatchResult TemplateMatch(byte[] sourceImageData, byte[] searchImageData, double threshold = 0.5, uint maxCount = 1, MatchTemplateType type = MatchTemplateType.CCOEFF_NORMED)
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
                throw new ArgumentException("需要查找的图片searchImageData，宽高不得超过sourceImageData。");
            }
            
            var matchModes = MatchTemplate.ConvertToMatchModes(type);
            return MatchTemplate.Match(sourceMat, searchMat, threshold, maxCount, matchModes);
        }
    }
}
