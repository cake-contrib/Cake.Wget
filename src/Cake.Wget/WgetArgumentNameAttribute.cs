using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Cake.Wget.Tests")]

namespace Cake.Wget
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class WgetArgumentNameAttribute : Attribute
    {
        public WgetArgumentNameAttribute(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name), "Wget argument name cannot be null.");
            }
            Name = name.Trim();
        }

        [ExcludeFromCodeCoverage]
        private WgetArgumentNameAttribute()
        {
        }

        public string Name { get; private set; }
    }
}
