using System;

namespace minesweeper
{
    class Program
    {

        static void Main(string[] args)
        {
            int row;
            int col;
            int numberOfBombs;
            int.TryParse(args[0], out row);
            int.TryParse(args[1], out col);
            int.TryParse(args[2], out numberOfBombs);

            GameBoard g = new GameBoard(row, col, numberOfBombs);
            char c = 'y';
            while (c == 'y') {
                g.InitGame();
                Console.WriteLine();
                g.PrintBoard();
                
                while (g.Status != GameStatus.GameLost && g.Status != GameStatus.GameWon)
                {
                    int cr;
                    int cc;
                    Console.Write("Row:");
                    string scr = Console.ReadLine();

                    Console.Write("Col:");
                    string scc = Console.ReadLine();
                    if (int.TryParse(scr, out cr)
                        && int.TryParse(scc, out cc)
                        && cr <= row && cr > 0
                        && cc <= col && cc > 0)
                    {
                        g.Click(cr - 1, cc - 1);
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid Row and Column");
                    }


                }
                Console.WriteLine("-*--*--*--*--*--*--*--*--*--*--*-");
                g.PrintBoard();
                Console.WriteLine("-*--*--*--*--*--*--*--*--*--*--*-");
                if (g.Status == GameStatus.GameWon)
                {
                    Console.WriteLine("Congratulations, you won the game!!!");
                }
                else
                {
                    Console.WriteLine("Game Over!");
                }
                Console.Write("Want to Play again?");
                c = Console.ReadKey().KeyChar;
            }
            
        }

       
    }

    
}
