using Dreamland.Core.Vision.Capture.Captors;

namespace Dreamland.Core.Vision.Capture
{
    /// <summary>
    ///     用于创建 <see cref="ICaptor"/> 的工厂
    /// </summary>
    public static class CaptorFactory
    {
        /// <summary>
        ///     创建<see cref="ICaptor"/>
        /// </summary>
        /// <returns></returns>
        public static ICaptor GetCaptor()
        {
            return new GdiCaptor(null);
        }
    }
}
