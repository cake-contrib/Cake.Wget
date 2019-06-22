using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Cake.Wget
{
    /// <summary>
    /// Extends <see cref="WgetSettings" /> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class WgetSettingsExtension
    {
#pragma warning disable IDE0060 // Remove unused parameter
        /// <summary>
        /// Reads a value of <see cref="WgetArgumentNameAttribute"/>.
        /// </summary>
        /// <param name="settings">Configuration class, <see cref="WgetSettings"/>.</param>
        /// <param name="propertyName">Name of the property which attribute value should be read.</param>
        /// <returns>Value of <see cref="WgetArgumentNameAttribute"/> of the <paramref name="propertyName"/>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when property <paramref name="propertyName"/>
        /// or attribute <see cref="WgetArgumentNameAttribute"/> was not found.</exception>
        public static string GetArgumentName(this WgetSettings settings, string propertyName)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            var propertyInfo = typeof(WgetSettings).GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found", nameof(propertyName));
            }

            // ReSharper disable once UseNegatedPatternMatching
            var attribute = propertyInfo.GetCustomAttribute(typeof(WgetArgumentNameAttribute)) as WgetArgumentNameAttribute;
            if (attribute == null)
            {
                throw new ArgumentException($"Property '{propertyName}' has no attribute {nameof(WgetArgumentNameAttribute)}", nameof(propertyName));
            }

            return attribute.Name;
        }
    }
}
