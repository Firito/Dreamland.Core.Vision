using System;
using System.Linq;
using System.Reflection;

namespace Dreamland.Core.Vision.Match
{
    internal static class MatchFeatureFactory
    {
        /// <summary>
        ///     根据<see cref="MatchFeatureType"/>创建<see cref="IFeatureProvider"/>的实例
        /// </summary>
        /// <param name="featureType"></param>
        /// <returns></returns>
        public static IFeatureProvider CreateMatchFeature(MatchFeatureType featureType)
        {
            //通过反射获取所有的 IMatchFeature
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
        ///     从<see cref="Type"/>中获取<see cref="MatchFeatureType"/>
        /// </summary>
        /// <returns></returns>
        internal static MatchFeatureType GetMathFeatureTypeFromAttribute(Type type)
        {
            var attribute = type.GetCustomAttribute(typeof(FeatureProviderTypeAttribute));
            if (!(attribute is FeatureProviderTypeAttribute featureProviderType))
            {
                return MatchFeatureType.Unknown;
            }

            return featureProviderType.MatchFeatureType;
        }
    }
}
