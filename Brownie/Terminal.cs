//-----------------------------------------------------------------------
// <copyright file="Terminal.cs" company="Leamware">
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
    /// Terminal for displaying colored characters.
    /// </summary>
    public class Terminal
    {
        /// <summary>
        /// The underlying character data.
        /// </summary>
        private List<List<Character>> characters;

        /// <summary>
        /// Initializes a new instance of the <see cref="Terminal" /> class.
        /// </summary>
        /// <param name="defaultColor">The default color.</param>
        /// <param name="size">The size.</param>
        public Terminal(int defaultColor, Vector2 size)
        {
            DefaultForeColor = defaultColor;

            characters = new List<List<Character>>();

            for (int x = 0; x < size.X; x++)
            {
                characters.Add(new List<Character>());

                for (int y = 0; y < size.Y; y++)
                    characters[x].Add(new Character(' ', DefaultForeColor, DefaultBackColor));
            }
        }

        /// <summary>
        /// Gets the default foreground color.
        /// </summary>
        /// <value>
        /// The default foreground color.
        /// </value>
        public int DefaultForeColor { get; }

        /// <summary>
        /// Gets the default background color.
        /// </summary>
        /// <value>
        /// The default background color.
        /// </value>
        public int DefaultBackColor { get; }

        /// <summary>
        /// Gets the characters.
        /// </summary>
        /// <value>
        /// The characters.
        /// </value>
        public IReadOnlyList<IReadOnlyList<Character>> Characters
        {
            get
            {
                var result = new List<IReadOnlyList<Character>>();
                foreach (var column in characters)
                    result.Add(column.AsReadOnly());

                return result.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public Vector2 Size
        {
            get
            {
                return new Vector2(characters.Count, characters.First().Count);
            }

            set
            {
                if (Size.X > value.X)
                    characters = characters.Take((int)value.X).ToList();
                else if (Size.X < value.X)
                {
                    for (int x = 0; x < value.X - Size.X; x++)
                    {
                        characters.Add(new List<Character>());

                        for (int y = 0; y < Size.Y; y++)
                            characters[x].Add(new Character(' ', DefaultForeColor, DefaultBackColor));
                    }
                }

                if (Size.Y > value.Y)
                {
                    for (int x = 0; x < Size.X; x++)
                        characters[x] = characters[x].Take((int)value.Y).ToList();
                }
                else if (Size.Y < value.Y)
                {
                    for (int x = 0; x < Size.X; x++)
                    {
                        for (int y = 0; y < value.Y - Size.Y; y++)
                            characters[x].Add(new Character(' ', DefaultForeColor, DefaultBackColor));
                    }
                }
            }
        }

        /// <summary>
        /// Writes the <see cref="Character"/> at the specified position.
        /// </summary>
        /// <param name="value">The <see cref="Character"/>.</param>
        /// <param name="position">The position.</param>
        public void Write(Character value, Vector2 position)
        {
            if (position.X.InRange(0, Size.X - 1) && position.Y.InRange(0, Size.Y - 1))
                characters[(int)position.X][(int)position.Y] = value;
        }

        /// <summary>
        /// Writes the <see cref="Character" /> array starting at the specified position.
        /// </summary>
        /// <param name="value">The <see cref="Character"/> array.</param>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        public void Write(Character[] value, Vector2 position, Vector2 size = default(Vector2))
        {
            var valueArray = Utility.Wrap(value, size);

            for (int y = 0; y < valueArray.Length; y++)
            {
                for (int x = 0; x < valueArray[y].Length; x++)
                {
                    if ((position.X + x).InRange(0, Size.X - 1) && position.Y.InRange(0, Size.Y - 1))
                        characters[(int)position.X + x][(int)position.Y] = valueArray[y][x];
                }
            }
        }

        /// <summary>
        /// Writes the <see cref="char"/> at the specified position.
        /// </summary>
        /// <param name="value">The <see cref="char"/>.</param>
        /// <param name="position">The position.</param>
        public void Write(char value, Vector2 position)
        {
            if (position.X.InRange(0, Size.X - 1) && position.Y.InRange(0, Size.Y - 1))
            {
                Character character = characters[(int)position.X][(int)position.Y];
                characters[(int)position.X][(int)position.Y] =
                    new Character(value, character.ForeColor, character.BackColor);
            }
        }

        /// <summary>
        /// Writes the <see cref="string" /> starting at the specified position.
        /// </summary>
        /// <param name="value">The <see cref="string" />.</param>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        public void Write(string value, Vector2 position, Vector2 size = default(Vector2))
        {
            var valueArray = Utility.Wrap(value.ToCharArray(), size);

            for (int y = 0; y < valueArray.Length; y++)
            {
                for (int x = 0; x < valueArray[y].Length; x++)
                {
                    if ((position.X + x).InRange(0, Size.X - 1) && position.Y.InRange(0, Size.Y - 1))
                    {
                        Character character = characters[(int)position.X + x][(int)position.Y];
                        characters[(int)position.X + x][(int)position.Y] =
                            new Character(valueArray[y][x], character.ForeColor, character.BackColor);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the foreground and background colors at the specified position.
        /// </summary>
        /// <param name="foreColor">The foreground color.</param>
        /// <param name="backColor">The background color.</param>
        /// <param name="position">The position.</param>
        public void SetColor(int foreColor, int backColor, Vector2 position)
        {
            if (position.X.InRange(0, Size.X - 1) && position.Y.InRange(0, Size.Y - 1))
            {
                Character character = characters[(int)position.X][(int)position.Y];
                characters[(int)position.X][(int)position.Y] =
                    new Character(character.Char, foreColor, backColor);
            }
        }

        /// <summary>
        /// Sets the foreground and background colors at the specified position.
        /// </summary>
        /// <param name="foreColors">The foreground colors.</param>
        /// <param name="backColors">The background colors.</param>
        /// <param name="position">The position.</param>
        public void SetColor(int[] foreColors, int[] backColors, Vector2 position)
        {
            for (int i = 0; i < Math.Max(foreColors.Length, backColors.Length); i++)
            {
                if ((position.X + i).InRange(0, Size.X - 1) && position.Y.InRange(0, Size.Y - 1))
                {
                    Character character = characters[(int)position.X + i][(int)position.Y];

                    if (foreColors?.Length > i)
                    {
                        if (backColors?.Length > i)
                        {
                            characters[(int)position.X + i][(int)position.Y] =
                                new Character(character.Char, foreColors[i], backColors[i]);
                        }
                        else
                        {
                            characters[(int)position.X + i][(int)position.Y] =
                                new Character(character.Char, foreColors[i], character.BackColor);
                        }
                    }
                    else if (backColors?.Length > i)
                    {
                        characters[(int)position.X + i][(int)position.Y] =
                            new Character(character.Char, foreColors[i], backColors[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the foreground and background colors at the specified position.
        /// </summary>
        /// <param name="foreColors">The foreground colors.</param>
        /// <param name="backColors">The background colors.</param>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        public void Write(int[] foreColors, int[] backColors, Vector2 position, Vector2 size = default(Vector2))
        {
            var foreColorsArray = Utility.Wrap(foreColors, size);

            for (int y = 0; y < foreColorsArray.Length; y++)
            {
                for (int x = 0; x < foreColorsArray[y].Length; x++)
                {
                    if ((position.X + x).InRange(0, Size.X - 1) && position.Y.InRange(0, Size.Y - 1))
                    {
                        Character character = characters[(int)position.X + x][(int)position.Y];
                        characters[(int)position.X + x][(int)position.Y] =
                            new Character(character.Char, foreColorsArray[y][x], character.BackColor);
                    }
                }
            }

            var backColorsArray = Utility.Wrap(backColors, size);

            for (int y = 0; y < backColorsArray.Length; y++)
            {
                for (int x = 0; x < backColorsArray[y].Length; x++)
                {
                    if ((position.X + x).InRange(0, Size.X - 1) && position.Y.InRange(0, Size.Y - 1))
                    {
                        Character character = characters[(int)position.X + x][(int)position.Y];
                        characters[(int)position.X + x][(int)position.Y] =
                            new Character(character.Char, character.ForeColor, backColorsArray[y][x]);
                    }
                }
            }
        }

        /// <summary>
        /// Reads the Character at the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The character.</returns>
        public Character Read(Vector2 position)
        {
            if (position.X.InRange(0, Size.X - 1) && position.Y.InRange(0, Size.Y - 1))
                return characters[(int)position.X][(int)position.Y];

            return default(Character);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                    Write(new Character(' ', DefaultForeColor, DefaultBackColor), new Vector2(x, y));
            }
        }

        /// <summary>
        /// Clears the row.
        /// </summary>
        /// <param name="row">The row.</param>
        public void ClearRow(int row)
        {
            for (int x = 0; x < Size.X; x++)
                Write(new Character(' ', DefaultForeColor, DefaultBackColor), new Vector2(x, row));
        }

        /// <summary>
        /// Clears the column.
        /// </summary>
        /// <param name="column">The column.</param>
        public void ClearColumn(int column)
        {
            for (int y = 0; y < Size.Y; y++)
                Write(new Character(' ', DefaultForeColor, DefaultBackColor), new Vector2(column, y));
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++)
                    result.Append(Read(new Vector2(x, y)));

                result.AppendLine();
            }

            return result.ToString();
        }
    }
}
