using System;
using System.Collections.Generic;

namespace minesweeper
{
    public enum GameStatus
    {
        InProgress,
        GameLost,
        GameWon
    }

    public class GameBoard
    {
        private readonly int row;
        private readonly int col;
        private readonly int numberOfBombs;
        private int[][] gameGrid;
        private bool[][] gameGridStatus;
        private GameStatus status;

        public GameStatus Status
        {
            get
            {
                return status;
            }
        }


        public GameBoard(int row, int col, int numberOfBombs)
        {
            this.row = row;
            this.col = col;
            this.numberOfBombs = numberOfBombs;
            
        }

        public void InitGame()
        {
            status = GameStatus.InProgress;
            gameGrid = new int[row][];
            gameGridStatus = new bool[row][];

            for (int r = 0; r < this.row; r++)
            {
                gameGrid[r] = new int[this.col];
                gameGridStatus[r] = new bool[this.col];
            }
            this.GenerateNewBoard();
        }

        private void GenerateNewBoard()
        {
            Random r = new Random();
            for(int n = 0; n < numberOfBombs;)
            {
                int bombRow = r.Next(0, row);
                int bombCol = r.Next(0, col);
                if(gameGrid[bombRow][bombCol] != int.MinValue)
                {
                    gameGrid[bombRow][bombCol] = int.MinValue;
                    this.UpdateNeighbourBombCount(bombRow, bombCol);
                    n++;
                }
            }
        }

        private void UpdateNeighbourBombCount(int brow, int bcol)
        {
            for(int r = -1; r <= 1; r++)
            {
                int rowToUpdate = brow + r;
                for (int c = -1; c <= 1; c++)
                {
                    int colToUpdate = bcol + c;
                    if(rowToUpdate>=0 && rowToUpdate<row
                        && colToUpdate>=0&& colToUpdate < col
                        && gameGrid[rowToUpdate][colToUpdate]!=int.MinValue)
                    {
                        gameGrid[rowToUpdate][colToUpdate]++;
                    }
                }
            }
        }

        public void Click(int clickedRow, int clickedCol)
        {
            if(gameGrid[clickedRow][clickedCol] == int.MinValue)
            {
                this.status = GameStatus.GameLost;
            }
            else
            {
                this.RevealTiles(clickedRow, clickedCol);
                this.CheckStatus();
                this.PrintBoard();
            }
        }

        private void CheckStatus()
        {
            GameStatus currStatus = GameStatus.GameWon;
            for(int r = 0; r < row; r++)
            {
                for(int c = 0; c < col; c++)
                {
                    if(gameGrid[r][c] != int.MinValue && gameGridStatus[r][c] == false)
                    {
                        currStatus = GameStatus.InProgress;
                        break;
                    }
                }
            }
            this.status = currStatus;
        }

        private void RevealTiles(int startRow, int startCol)
        {
            Queue<(int r, int c)> q = new Queue<(int r, int c)>();
            q.Enqueue((startRow, startCol));

            while (q.Count != 0)
            {
                var currentTile = q.Dequeue();
                if(gameGrid[currentTile.r][currentTile.c] != int.MinValue)
                {
                    gameGridStatus[currentTile.r][currentTile.c] = true;
                    if(gameGrid[currentTile.r][currentTile.c] == 0)
                    {
                        for (int r = -1; r <= 1; r++)
                        {
                            int rowToReveal = currentTile.r + r;
                            for (int c = -1; c <= 1; c++)
                            {
                                int colToReveal = currentTile.c + c;
                                if (rowToReveal >= 0 && rowToReveal < row
                                    && colToReveal >= 0 && colToReveal < col &&
                                    !gameGridStatus[rowToReveal][colToReveal])
                                {
                                    q.Enqueue((r = rowToReveal, c = colToReveal));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void PrintColumnNumbers()
        {
            for (int nc = 0; nc < col; nc++)
            {
                if (nc == 0)
                {
                    WriteCell(" ");
                }
                WriteCell((nc + 1).ToString());
            }
            Console.WriteLine();
        }

        private void PrintRowNumber(int r)
        {
            WriteCell((r + 1).ToString());
        }

        private void WriteCell(string cellValue)
        {
            Console.Write(cellValue.PadLeft(row.ToString().Length+1, ' '));
        }

        public void PrintBoard()
        {
            for(int r = 0; r < row; r++)
            {
                if (r == 0)
                {
                    PrintColumnNumbers();
                }

                for(int c=0; c< col; c++)
                {
                    if(c == 0)
                    {
                        PrintRowNumber(r);
                    }
                    
                    if (gameGridStatus[r][c]
                        || this.Status == GameStatus.GameWon
                        || this.Status == GameStatus.GameLost)
                    {
                        
                        switch (gameGrid[r][c])
                        {
                            case int.MinValue:
                                WriteCell("*");
                                break;
                            case 0:
                                WriteCell(".");
                                break;
                            default:
                                WriteCell(gameGrid[r][c].ToString());
                                break;
                        }
                    }
                    else
                    {
                        WriteCell("H");
                    }
                    
                }
                Console.WriteLine();
            }
        }
    }
}
