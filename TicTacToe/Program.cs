// See https://aka.ms/new-console-template for more information
//dotnet run
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
namespace TicTacToe
{
    class Program
    {
        public bool TieStop = false;

        //User info
        public struct User
        {
            public string Name;
            public int Totalscore;
            public string Password;
        }

        //Size of boards
        

        static int[] board = new int[9];
        static int[] bigboard = new int[16];




        //Where everything is consolidated and executed.
        static void Main(string[] args)
        {
            List <string> names = new List <string>();
            List <int> totalscores = new List <int>();
            List <string> passwords = new List <string>(); 
            ReadWrite Write = new ReadWrite();

            Write.GetTheUserInfo(names, totalscores, passwords);

            User user = new User();

            Console.Write("Enter your name: ");
            user.Name = Console.ReadLine();
            Console.Write($"Hello {user.Name}! What is your password? ");
            user.Password = Console.ReadLine();
            
            Write.IsUserHere(names, totalscores, passwords, user.Name, user.Password);
            user.Totalscore = Write.scored;
            Console.WriteLine($"Your current score is {user.Totalscore}");
            int score = 0;
            score += user.Totalscore;

            for (int i = 0; i < 9; i++)
            {
                board[i] = 0;
            }

            for (int i = 0; i < 16; i++)
            {
                bigboard[i] = 0;
            }  
     
            string ContinueGame = "";
            Random randy = new Random();

            
            while(ContinueGame.ToLower() != "no")
            {
                int UserTurn = -1;
                int Think = 0;
                int ComputerTurn = -1;
                int CompUserNum = -1;

                Console.Write("Select two options for empty spaces: display a small dot or numbers. (input \".\" or \"#\") ");
                string EmptySpaceDisplay = Console.ReadLine();
                Console.Write("Choose size of board (3x3 or 4x4): ");
                string SizeOfBoard = Console.ReadLine();
                
                //small board
                if (SizeOfBoard == "3x3")
                {
                    Write.ScoreDisplay(score);
                    PrintBoard(EmptySpaceDisplay);
                    while(CheckForWinner(Think) == 0 && Think < 50)
                    {
                        while(CompUserNum == -1  && Think < 50 || board[CompUserNum] != 0 && Think < 50)
                        {
                            Console.WriteLine("Your turn. Select a spot from 1 to 9 to place an X.");
                            UserTurn = int.Parse(Console.ReadLine());
                            Console.WriteLine("You typed in " + UserTurn + ".");
                            CompUserNum = UserTurn - 1;
                            Think += 1;
                            Console.WriteLine(Think);
                        }
                        board[CompUserNum] = 1;
                        if (Think < 10)
                        {
                            Think = 0;
                        }
                        
                        while(ComputerTurn == -1 && Think < 50 || board[ComputerTurn] != 0 && Think < 50)
                        {
                            Think += 1;

                            ComputerTurn = randy.Next(8);
                            int CompComNum = ComputerTurn + 1;

                            Console.WriteLine("The computer's turn...");
                            Console.WriteLine("The computer selected " + CompComNum + ".");
                            Console.WriteLine(Think);
                        }
                        board[ComputerTurn] = 2;
                        if (Think < 10)
                        {
                            Think = 0;
                        }
                        Write.ScoreDisplay(score);
                        PrintBoard(EmptySpaceDisplay);
                    }
                    if (CheckForWinner(Think) == 1)
                    {
                        Console.WriteLine("  You Win!");
                    }
                    else if  (CheckForWinner(Think) == 2)
                    {
                        Console.WriteLine(" CPU Wins!");
                    }
                    else
                    {
                        Console.WriteLine("    Tie!");
                    }
                    Console.WriteLine("");
                }

                //big board
                else
                {
                    Write.ScoreDisplay(score);
                    PrintBoardThatIsBigger(EmptySpaceDisplay);
                    while(CheckForWinnerOfBiggerChallenge() == 0)
                    {
                        while(CompUserNum == -1 || bigboard[CompUserNum] != 0)
                        {
                            Console.WriteLine("Your turn. Select a spot from 1 to 9 to place an X.");
                            UserTurn = int.Parse(Console.ReadLine());
                            Console.WriteLine("You typed in " + UserTurn + ".");
                            CompUserNum = UserTurn - 1;
                        }
                        bigboard[CompUserNum] = 1;

                        if (CheckForWinnerOfBiggerChallenge() == 0)
                        {
                            while(ComputerTurn == -1 || bigboard[ComputerTurn] != 0)
                            {
                                Think += 1;

                                ComputerTurn = randy.Next(15);
                                int CompComNum = ComputerTurn + 1;

                                Console.WriteLine("The computer's turn...");
                                Console.WriteLine("The computer selected " + CompComNum + ".");
                                Console.WriteLine(Think);
                                
                            }
                            bigboard[ComputerTurn] = 2;
                        }
        
                        Think = 0;
                        Write.ScoreDisplay(score);
                        PrintBoardThatIsBigger(EmptySpaceDisplay);
                    }
                    if (CheckForWinnerOfBiggerChallenge() == 1)
                    {
                        Console.WriteLine("  You Win!");
                    }
                    else if  (CheckForWinnerOfBiggerChallenge() == 2)
                    {
                        Console.WriteLine(" CPU Wins!");
                    }
                    else
                    {
                        Console.WriteLine("    Tie!");
                    }
                    Console.WriteLine("");
                }
                if (CheckForWinnerOfBiggerChallenge() == 1 || CheckForWinner(Think) == 1)
                {
                    if (SizeOfBoard == "3x3")
                    {
                        score = score + 200;
                    }
                    else
                    {
                        score = score + 500;
                    }
                    
                }
                
                Write.AmendChangesToScore(names, totalscores, passwords, user.Name, user.Password, user.Totalscore, score);
                Write.UpdateHighScore(score, names, totalscores, passwords);

                Console.Write("Continue? (input \"yes\" or \"no\") ");
                ContinueGame = Console.ReadLine();
                if (ContinueGame.ToLower() != "no")
                {
                    if (SizeOfBoard == "3x3")
                    {
                        ResetBoard(board);
                    }
                    else
                    {
                        ResetBoard(bigboard);
                    }
                }
            }
            
            

        }

        //Check and see who is the winner.
        private static int CheckForWinner(int Tie)
        {
            //If nobody won return 0
            //top row
            if (board[0] == board[1] && board[1] == board[2])
            {
                return board[0];
            }
            //second row
            if (board[3] == board[4] && board[4] == board[5])
            {
                return board[3];
            }
            //third row
            if (board[6] == board[7] && board[7] == board[8])
            {
                return board[6];
            }
            //first column
            if (board[0] == board[3] && board[3] == board[6])
            {
                return board[0];
            }
            //second column
            if (board[1] == board[4] && board[4] == board[7])
            {
                return board[1];
            }
            //third column
            if (board[2] == board[5] && board[5] == board[8])
            {
                return board[2];
            }
            //first diagonal
            if (board[0] == board[4] && board[4] == board[8])
            {
                return board[0];
            }
            //second diagonal
            if (board[2] == board[4] && board[4] == board[6])
            {
                return board[2];
            }
            
            /*if (board[0] != 0 && board[1] != 0 && board[2] != 0 && board[3] != 0 && board[4] != 0 && board[5] != 0 && board[6] != 0 && board[7] != 0 && board[8] != 0)
            {
                return 3;
            }*/

            if (Tie > 9)
            {
                return 3;
            }

            return 0;
        }
        //same winner function, but for a 4x4 board.
        private static int CheckForWinnerOfBiggerChallenge()
        {
            //top row
            if (bigboard[0] == bigboard[1] && bigboard[1] == bigboard[2] && bigboard[2] == bigboard[3])
            {
                return bigboard[0];
            }
            //second row
            if (bigboard[4] == bigboard[5] && bigboard[5] == bigboard[6] && bigboard[6] == bigboard[7])
            {
                return bigboard[4];
            }
            //third row
            if (bigboard[8] == bigboard[9] && bigboard[9] == bigboard[10] && bigboard[10] == bigboard[11])
            {
                return bigboard[8];
            }
            // fourth row
            if (bigboard[12] == bigboard[13] && bigboard[13] == bigboard[14] && bigboard[14] == bigboard[15])
            {
                return bigboard[12];
            }
            // first column
            if (bigboard[0] == bigboard[4] && bigboard[4] == bigboard[8] && bigboard[8] == bigboard[12])
            {
                return bigboard[0];
            }
            //second column
            if (bigboard[1] == bigboard[5] && bigboard[5] == bigboard[9] && bigboard[9] == bigboard[13])
            {
                return bigboard[1];
            }
            //third column
            if (bigboard[2] == bigboard[6] && bigboard[6] == bigboard[10] && bigboard[10] == bigboard[14])
            {
                return bigboard[2];
            }
            // fourth column
            if (bigboard[3] == bigboard[7] && bigboard[7] == bigboard[11] && bigboard[11] == bigboard[15])
            {
                return bigboard[3];
            }
            // first diagonal
            if (bigboard[0] == bigboard[5] && bigboard[5] == bigboard[10] && bigboard[10] == bigboard[15])
            {
                return bigboard[0];
            }
            // second diagonal
            if (bigboard[3] == bigboard[6] && bigboard[6] == bigboard[9] && bigboard[9] == bigboard[12])
            {
                return bigboard[3];
            }
            return 0;
        }

        //printing the board
        private static void PrintBoard(string SpaceDisplay)
        {
            Console.WriteLine("");
            for (int i = 0; i < 9; i++)
            {
                //Console.WriteLine("Square " + i + " has " + board[i]);
                if (board[i] == 0)
                {
                    if (SpaceDisplay == "#")
                    {
                        int CompedNum = i + 1;
                        Console.Write(" " + CompedNum  + " ");
                    }
                    else
                    {
                        Console.Write(" . ");
                    }

                }
                if (board[i] == 1)
                {
                    Console.Write(" X ");
                }
                if (board[i] == 2)
                {
                    Console.Write(" O ");
                }
                //new line per third character
                if (i == 2 || i == 5)
                {
                    Console.WriteLine("");
                    Console.Write("---+---+---");
                    Console.WriteLine("");
                }
                if (i == 8)
                {
                    Console.WriteLine("");
                }
                if (i == 0 || i == 1 || i == 3 || i == 4 || i == 6 || i == 7)
                {
                    Console.Write("|");
                }
            }
        }

        //printing... a bigger board
        private static void PrintBoardThatIsBigger(string SpaceDisplay)
        {
            Console.WriteLine("");
            for (int i = 0; i < 16; i++)
            {
                if (bigboard[i] == 0)
                {
                    if (SpaceDisplay == "#")
                    {
                        
                        int CompedNum = i + 1;
                        if(CompedNum < 10)
                        {
                            Console.Write(" " + CompedNum  + " ");
                        }
                        else
                        {
                            Console.Write(CompedNum + " ");
                        }
                        
                    }
                    else
                    {
                        Console.Write(" . ");
                    }

                }
                if (bigboard[i] == 1)
                {
                    Console.Write(" X ");
                }
                if (bigboard[i] == 2)
                {
                    Console.Write(" O ");
                }
                //new line per fourth character
                if (i == 3 || i == 7 || i == 11 )
                {
                    Console.WriteLine("");
                    Console.Write("---+---+---+---");
                    Console.WriteLine("");
                }
                if (i == 15)
                {
                    Console.WriteLine("");
                }
                if (i == 0 || i == 1 || i == 2 || i == 4 || i == 5 || i == 6 || i == 8 || i == 9 || i == 10 || i == 12 || i == 13 || i == 14) 
                {
                    Console.Write("|");
                }
            }
        }

        //reset the board's tiles.
        private static void ResetBoard(int[] TheBoard)
        {
            for (int i = 0; i < TheBoard.Count(); i++)
            {
                TheBoard[i] = 0;
            }
        }


    }
}
