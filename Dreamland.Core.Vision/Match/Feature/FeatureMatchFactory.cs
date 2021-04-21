using System;
using System.Linq;
using System.Reflection;

namespace Dreamland.Core.Vision.Match
{
    internal static class FeatureMatchFactory
    {
        /// <summary>
        ///     根据<see cref="FeatureMatchType"/>创建<see cref="IFeatureProvider"/>的实例
        /// </summary>
        /// <param name="featureType"></param>
        /// <returns></returns>
        public static IFeatureProvider CreateFeatureProvider(FeatureMatchType featureType)
        {
            //通过反射获取所有的 IFeatureProvider
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeof(IFeatureProvider)) && !x.IsAbstract && x.IsClass).ToList();

            if (!types.Any())
            {
                return null;
            }

            var type = types.FirstOrDefault(x => GetMathFeatureTypeFromAttribute(x) == featureType);
            if (type == null)
            {
                return null;
            }

            return Activator.CreateInstance(type) as IFeatureProvider;
        }

        /// <summary>
        ///     从<see cref="Type"/>中获取<see cref="FeatureMatchType"/>
        /// </summary>
        /// <returns></returns>
        internal static FeatureMatchType GetMathFeatureTypeFromAttribute(Type type)
        {
            var attribute = type.GetCustomAttribute(typeof(FeatureProviderTypeAttribute));
            if (!(attribute is FeatureProviderTypeAttribute featureProviderType))
            {
                return FeatureMatchType.Unknown;
            }

            return featureProviderType.FeatureMatchType;
        }
    }
}
