//-----------------------------------------------------------------------
// <copyright file="Utility.cs" company="Leamware">
//     Copyright (c) Leamware. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Brownie
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Utility
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Checks if a value is in the specified range.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>Whether the value is in the range.</returns>
        public static bool InRange<T>(this T value, T min, T max)
                    where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
                return false;
            else if (value.CompareTo(max) > 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Converts a string to a <see cref="Character" /> array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="foreColor">The foreground color.</param>
        /// <param name="backColor">The background color.</param>
        /// <returns>The Character array.</returns>
        public static Character[] ToCharacters(this string value, int foreColor, int backColor)
        {
            var result = new Character[value.Length];

            for (int i = 0; i < result.Length; i++)
                result[i] = new Character(value[i], foreColor, backColor);

            return result;
        }

        /// <summary>
        /// Wraps an array to a size.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="size">The size.</param>
        /// <returns>The array of objects.</returns>
        public static T[][] Wrap<T>(T[] value, Vector2 size)
        {
            var result = new List<T[]>();

            if (size == default(Vector2))
            {
                result.Add(value);
                return result.ToArray();
            }

            while (value.Length > size.X)
            {
                result.Add(value.Take((int)size.X).ToArray());
                value = value.Skip((int)size.X).ToArray();

                if (size.Y > 0 && result.Count >= size.Y)
                    return result.ToArray();
            }

            // add the remaining bit if not empty
            if (value.Length > 0)
                result.Add(value);

            return result.ToArray();
        }

        /// <summary>
        /// Wraps a string to a size.
        /// </summary>
        /// <param name="value">The the_string.</param>
        /// <param name="width">The width.</param>
        /// <param name="maxLines">The maximum lines.</param>
        /// <returns>The wrapped string.</returns>
        public static string WrapText(this string value, int width, int maxLines = -1)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // ensure the width is larger than zero.
            if (width <= 0)
                return string.Empty;

            // Parse each line of text
            int lines = 0;
            int next;
            for (int i = 0; i < value.Length; i = next)
            {
                // Find end of line
                int eol = value.IndexOf(Environment.NewLine, i);

                if (eol == -1)
                    next = eol = value.Length;
                else
                    next = eol + Environment.NewLine.Length;

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > i)
                {
                    do
                    {
                        int len = eol - i;

                        if (len > width)
                            len = BreakLine(value, i, width);

                        stringBuilder.Append(value, i, len);

                        if (maxLines > 0 && ++lines >= maxLines)
                            return stringBuilder.ToString();

                        stringBuilder.AppendLine();

                        // Trim whitespace following break
                        i += len;

                        while (i < eol && char.IsWhiteSpace(value[i]))
                            i++;
                    }
                    while (eol > i);
                }
                else
                    stringBuilder.AppendLine(); // Empty line
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Locates position to break the given line so as to avoid
        /// breaking words.
        /// </summary>
        /// <param name="text">String that contains line of text</param>
        /// <param name="pos">Index where line of text starts</param>
        /// <param name="max">Maximum line length</param>
        /// <returns>The modified line length</returns>
        public static int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max - 1;
            while (i >= 0 && !char.IsWhiteSpace(text[pos + i]))
                i--;

            if (i < 0)
                return max; // No whitespace found; break at maximum length
                            // Find start of whitespace
            while (i >= 0 && char.IsWhiteSpace(text[pos + i]))
                i--;

            // Return length of text before whitespace
            return i + 1;
        }
    }
}
