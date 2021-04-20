// ReSharper disable All

namespace Dreamland.Core.Vision.Match
{
    /// <summary>
    ///     模版匹配算法
    /// </summary>
    /// <value>openCV支持的模版匹配算法</value>
    public enum MatchTemplateType
    {
        /// <summary>
        ///     平方差匹配
        /// <para>
        ///     平方差匹配就是通过计算每个像素点的差的平方的和，和数学中统计里面的平方差类似。
        /// </para>
        /// </summary>
        /// <value>值越大，匹配越差。</value>
        SQDIFF,

        /// <summary>
        ///     标准平方差匹配
        /// <para>
        ///     对 <see cref="SQDIFF"/> 算法进行了标准化处理，经过处理后，上面的值就不会太大。
        /// </para>
        /// </summary>
        /// <value>值越大，匹配越差。</value>
        SQDIFF_NORMED,

        /// <summary>
        ///     相关匹配
        /// <para>
        ///     采用模板和图像间的乘法操作，所以较大的数表示匹配程度较高，0标识最坏的匹配效果。
        /// </para>
        /// </summary>
        /// <value>值越大，匹配越好。</value>
        CCORR,

        /// <summary>
        ///     标准相关匹配
        /// <para>
        ///     对 <see cref="CCORR"/> 算法进行了标准化处理，经过处理后，上面的值就不会太大。
        /// </para>
        /// </summary>
        /// <value>值越大，匹配越好。</value>
        CCORR_NORMED,

        /// <summary>
        ///     相关性系数匹配
        /// <para>
        ///     将模版对其均值的相对值与图像对其均值的相关值进行匹配，1表示完美匹配，-1表示糟糕的匹配，0表示没有任何相关性(随机序列)。
        /// </para>
        /// </summary>
        /// <value>值越大，匹配越好。</value>
        CCOEFF,

        /// <summary>
        ///     标准相关性系数匹配
        /// <para>
        ///     对 <see cref="CCOEFF"/> 算法进行了标准化处理，经过处理后，上面的值就不会太大。
        /// </para>
        /// </summary>
        /// <value>值越大，匹配越好。</value>
        CCOEFF_NORMED,
    }
}
