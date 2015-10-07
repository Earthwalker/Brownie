//-----------------------------------------------------------------------
// <copyright file="Character.cs" company="Leamware">
//     Copyright (c) Leamware. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Brownie
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a char with a color value.
    /// </summary>
    public struct Character
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Character" /> struct.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="foreColor">Color of the foreground.</param>
        /// <param name="backColor">Color of the background.</param>
        public Character(char character, int foreColor, int backColor)
        {
            Char = character;
            ForeColor = foreColor;
            BackColor = backColor;
        }

        /// <summary>
        /// Gets or sets the underlying char.
        /// </summary>
        /// <value>
        /// The underlying char.
        /// </value>
        public char Char { get; set; }

        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        /// <value>
        /// The foreground color.
        /// </value>
        public int ForeColor { get; set; }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        /// <value>
        /// The background color.
        /// </value>
        public int BackColor { get; set; }
    }
}
