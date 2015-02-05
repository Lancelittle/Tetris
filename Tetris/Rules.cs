using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tetris
{
    class Rules
    {
        public int score { get; private set; }
        public int level { get; private set; }
        private int levelCounter;
        private int toNextLevel;
        public bool nextLevel { get; set; }
        private int[,] board = {
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0}};

        public Rules()
        {
            nextLevel = false;
            level = 1;
            score = 0;
            levelCounter = 0;
            toNextLevel = 1;
        }

        /*
         * Checks for finished rows, increases score and calls LevelCheck()
         */
        public List<int> RowCheck()
        {
            List<int> rows = new List<int>();
            int counter = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                if (board[i, counter] + board[i, counter + 1] + board[i, counter + 2] + board[i, counter + 3] +
                     board[i, counter + 4] + board[i, counter + 5] + board[i, counter + 6] +
                         board[i, counter + 7] + board[i, counter + 8] + board[i, counter + 9] == 10)
                {
                    rows.Add(i);
                    MovesBlocksDown(i);
                    score += 100;
                    levelCounter++;
                    LevelCheck();
                }
            }
            return rows;
        }

        /*
         * Increases level if criteria is met
         */
        public void LevelCheck()
        {
            if (levelCounter >= toNextLevel)
            {
                level++;
                toNextLevel++;
                levelCounter = 0;
                nextLevel = true;
            }
        }

        /*
         * Checks to see if top row has a block
         */
        public bool GameOver()
        {
            bool gameOver = false;
            int counter = 0;
            if (board[0, counter] == 1 || board[0, counter + 1] == 1 || board[0, counter + 2] == 1 ||
                board[0, counter + 3] == 1 || board[0, counter + 4] == 1 || board[0, counter + 5] == 1 ||
                     board[0, counter + 6] == 1 || board[0, counter + 7] == 1 ||
                         board[0, counter + 8] == 1 || board[0, counter + 9] == 1)
            {
                gameOver = true;
            }
            return gameOver;
        }

        /*
         * Checks if cell has block, or is out of bounds
         */
        public bool MoveCheck(int row, int col)
        {
            bool b = false;
            if (col < 0 || col > 9 || row < 0 || row > 19)
            {
                return true;
            }
            if (board[row, col] == 1)
            {
                b = true;
            }

            return b;
        }

        /*
         * Takes col given and loops from top row to bottom until it hits a 1, then returns the row above the 1.
         */
        public int BottomCheck(Points point)
        {
            int count = 0;
            int row = 0;

            while (count < 19)
            {
                if (board[count, (int)point.COL] == 1)
                    return count - 1;
                count++;
            }
            if (count == 19)
                row = 19;
            else
                row = count - 1;

            return row;
        }

        /*
         * Add blocks to boards
         */
        public bool AddToBoard(Points[] point)
        {
            foreach (Points p in point)
            {
                board[(int)p.ROW, (int)p.COL] = 1;
            }
            return true;
        }

        /*
         * Shifts rows down by the given row.
         */
        private void MovesBlocksDown(int row)
        {
            int rowCleared = row;

            for (int i = rowCleared; i > 0; i--)
            {
                for (int j = 0; j < 10; j++)
                {
                    board[i, j] = board[i - 1, j];
                }
            }

            for (int i = 0; i < 10; i++)
            {
                board[0, i] = 0;
            }
        }
    }
}