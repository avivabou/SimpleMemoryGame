using System;
using System.Collections.Generic;
using System.Drawing;

namespace B20_Ex05_Aviv_207597980_Shir_307926238
{
    public class Player
    {
        private string m_Name;

        public Color Color { get; set; } = Color.Transparent;

        public int Score { get; private set; } = 0;

        /// <summary>
        /// Player constructor.
        /// Creates new Player object with the given name.
        /// </summary>
        /// <param name="i_playerName">The name of player.</param>
        public Player(string i_playerName)
        {
            m_Name = i_playerName;
        }

        /// <summary>
        /// Increase score by 1.
        /// </summary>
        public void IncreaseScore()
        {
            Score++;
        }

        /// <summary>
        /// Translate the object into a string.
        /// </summary>
        /// <returns>Player name.</returns>
        public override string ToString()
        {
            return m_Name;
        }
    }
}
