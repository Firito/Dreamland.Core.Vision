using System.Collections.Generic;

namespace Dreamland.Core.Vision.Match
{
    /// <summary>
    ///     匹配配置
    /// </summary>
    public abstract class MatchArgument
    {
        /// <summary>
        ///     开启匹配信息控制台输出
        /// </summary>
        public const string ConsoleOutput = nameof(ConsoleOutput);

        /// <summary>
        ///     开启匹配预览
        /// </summary>
        public const string PreviewMatchResult = nameof(PreviewMatchResult);

        /// <summary>
        /// 提供一些额外配置
        /// <para><see cref="Dictionary{TKey,TValue}"/>的每一项中的Key代表配置项，Value代表配置项的参数</para>
        /// </summary>
        /// <value>
        /// <para><see cref="ConsoleOutput"/> 为 true 时开启控制台输出;</para>
        /// <para><see cref="PreviewMatchResult"/> 为 true 时开启匹配结果的预览</para>
        /// </value>
        public Dictionary<string, object> ExtensionConfig { get; set; } = new Dictionary<string, object>();

        /// <summary>
        ///     拓展功能是否开启
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public bool IsExtensionConfigEnabled(string configName)
        {
            //如果开启了匹配结果预览，则显示匹配结果
            return ExtensionConfig != null && ExtensionConfig.TryGetValue(configName, out var isEnabled) && isEnabled is true;
        }
    }
}
