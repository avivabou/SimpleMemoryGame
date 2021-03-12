using System;
using System.Collections.Generic;
using System.Drawing;

namespace MemoryGame
{
    public class ComputerPlayer : Player
    {
        private static readonly string sr_ComputerPlayerName = "COMPUTER";
        private Point m_FirstCell, m_SecondCell;
        private Dictionary<int, char> m_KnownCells;
        private List<int> m_Found;
        private Size m_BoardSize;
        private int m_Moves = 0;
        private Random m_Randomer = new Random();

        public bool PlayedTwice
        {
            get
            {
                return m_Moves == 2;
            }
        }

        /// <summary>
        /// ComputerPlayer constructor.
        /// </summary>
        /// <param name="i_boardSize">The size of the board this player going to play at.</param>
        public ComputerPlayer(Size i_boardSize) : base(sr_ComputerPlayerName)
        {
            m_KnownCells = new Dictionary<int, char>();
            m_BoardSize = i_boardSize;
            m_Found = new List<int>();
        }

        /// <summary>
        /// Update known cells.
        /// </summary>
        /// <param name="i_x">X cordinate of the cell.</param>
        /// <param name="i_y">Y cordinate of the cell.</param>
        /// <param name="i_letter">The letter of the cell.</param>
        public void ShownCell(int i_x, int i_y, char i_letter)
        {
            int index = getUniqueIndex(i_x, i_y);

            m_KnownCells[index] = i_letter;
        }

        /// <summary>
        /// Remove the given cells from the known cells.
        /// </summary>
        /// <param name="i_firstSelection">First cell.</param>
        /// <param name="i_secondSelection">Second cell.</param>
        public void CoupleFound(Point i_firstSelection, Point i_secondSelection)
        {
            int firstIndex = getUniqueIndex(i_firstSelection);
            int secondIndex = getUniqueIndex(i_secondSelection);

            m_KnownCells.Remove(firstIndex);
            m_KnownCells.Remove(secondIndex);
            m_Found.Add(firstIndex);
            m_Found.Add(secondIndex);
        }

        /// <summary>
        /// Ask the computer player to choose cell to reveal.
        /// </summary>
        /// <returns>The point of the cell to reveal.</returns>
        public Point GetCellsToReveal()
        {
            Point toReveal = m_FirstCell;
            bool found = true;

            if (m_Moves == 0)
            {
                found = findFirst();
                toReveal = m_FirstCell;
            }
            else if (m_Moves == 1)
            {
                bool isSameAsFirst = (m_FirstCell.X == m_SecondCell.X) && (m_FirstCell.Y == m_SecondCell.Y);

                if ((m_SecondCell.X == -1) || isSameAsFirst)
                {
                    found = findSecond(m_KnownCells[getUniqueIndex(m_FirstCell)]);
                }
                else
                {
                    found = true;
                }

                toReveal = m_SecondCell;
                m_SecondCell = new Point(-1, -1);
            }

            if (!found)
            {
                toReveal = getRandomly();
            }

            m_Moves++;

            return toReveal;
        }

        /// <summary>
        /// Initialize the moves counter of the computer player.
        /// </summary>
        public void SetThisTurn()
        {
            m_Moves = 0;
        }

        /// <summary>
        /// Choosing first cell if there are 2 cells on self memory with the same letter.
        /// Updating m_FirstCell to be the found first cell.
        /// </summary>
        /// <returns>True if matching couple were found.</returns>
        private bool findFirst()
        {
            bool found = false;
            List<int> Keys = new List<int>(m_KnownCells.Keys);

            foreach (int key in Keys)
            {
                char current_letter = m_KnownCells[key];

                m_FirstCell = getPointByIndex(key);
                if (findSecond(current_letter))
                {
                    m_KnownCells[key] = current_letter;
                    found = true;
                    break;
                }
                else
                {
                    m_SecondCell = new Point(-1, -1);
                }
            }

            return found;
        }

        /// <summary>
        /// Choosing second cell which it letter is the same as the first cell who's chosen.
        /// Updating m_SecondCell to be the found second cell.
        /// If could not find match, m_secondCell = Point (-1, -1).
        /// </summary>
        /// <param name="i_firstLetter">The letter of the first cell.</param>
        /// <returns>True if match were found, otherwise False.</returns>
        private bool findSecond(char i_firstLetter)
        {
            bool found = false;
            List<int> Keys = new List<int>(m_KnownCells.Keys);

            Keys.Remove(getUniqueIndex(m_FirstCell));
            foreach (int key in Keys)
            {
                if (m_KnownCells[key] == i_firstLetter)
                {
                    found = true;
                    m_SecondCell = getPointByIndex(key);
                    break;
                }
            }

            return found;
        }

        /// <summary>
        /// Choose wisely random cell.
        /// </summary>
        /// <returns>A randomize cell which didn't reveal yet.</returns>
        private Point getRandomly()
        {
            List<int> goodPointsToChoose = new List<int>();
            Point choosen;

            for (int key = 0; key < m_BoardSize.Width * m_BoardSize.Height; key++)
            {
                if ((!m_KnownCells.ContainsKey(key)) && (!m_Found.Contains(key)))
                {
                    goodPointsToChoose.Add(key);
                }
            }

            if (goodPointsToChoose.Count == 0)
            {
                choosen = new Point(-1, -1);
            }
            else
            {
                choosen = getPointByIndex(goodPointsToChoose[m_Randomer.Next(goodPointsToChoose.Count)]);
                if (m_Moves == 0)
                {
                    m_FirstCell = choosen;
                }
            }

            return choosen;
        }

        /// <summary>
        /// Get unique integer for the given point under the maximal values of the given board size.
        /// </summary>
        /// <param name="i_point">A point.</param>
        /// <returns>Unique integer value.</returns>
        private int getUniqueIndex(Point i_point)
        {
            return (i_point.X * m_BoardSize.Height) + i_point.Y;
        }

        /// <summary>
        /// Get unique integer for the given cordinates under the maximal values of the given board size.
        /// </summary>
        /// <param name="i_x">X cordinate.</param>
        /// <param name="i_y">Y cordinate.</param>
        /// <returns>Unique integer value.</returns>
        private int getUniqueIndex(int i_x, int i_y)
        {
            return (i_x * m_BoardSize.Height) + i_y;
        }

        /// <summary>
        /// Restore the point by it unique value.
        /// </summary>
        /// <param name="i_value">The unique value to restore.</param>
        /// <returns>The point that presented by the given value.</returns>
        private Point getPointByIndex(int i_value)
        {
            return new Point(i_value / m_BoardSize.Height, i_value % m_BoardSize.Height);
        }
    }
}
