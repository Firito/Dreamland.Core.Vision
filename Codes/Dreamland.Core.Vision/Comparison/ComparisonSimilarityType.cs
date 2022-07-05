// ReSharper disable InconsistentNaming

namespace Dreamland.Core.Vision.Comparison
{
    /// <summary>
    ///     使用的图片相似度算法
    /// </summary>
    public enum ComparisonSimilarityType
    {
        /// <summary>
        /// 使用像素对比计算图片相似度
        /// </summary>
        PIXEL_CONTRAST,
        /// <summary>
        /// 使用哈希算法(图片像素灰度值)计算图片相似度
        /// </summary>
        HASH_GRAY,
        /// <summary>
        /// 使用欧氏距离(像素比较)计算图片相似度，适合图片大小完全相等的图片
        /// </summary>
        EUCLIDEAN_DISTANCE,
    }
}
