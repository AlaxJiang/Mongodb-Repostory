using System;
using System.Collections.Generic;
using System.Text;

namespace AdeSAAS.DataService
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionNameAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the CollectionName class attribute with the desired name.
        /// </summary>
        /// <param name="value">Name of the collection.</param>
        public CollectionNameAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Empty collection name is not allowed", nameof(value));

            Name = value;
        }

        /// <summary>
        ///     Gets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public virtual string Name { get; }
    }
}
