using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForRead.Model
{
    /// <summary>
    /// Structure of application color.
    /// </summary>
    public class ColorOption
    {
        /// <summary>
        /// Name of color.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// HEX-value of color.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initialise instant of color.
        /// </summary>
        /// <param name="name">Color name.</param>
        /// <param name="value">HEX-value of color.</param>
        public ColorOption(string name, string value)
        {
            Name = name;
            Value = value;
        }

    }
}
