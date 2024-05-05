using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
namespace TicTacToe
{
    class ReadWrite
    {
        private string _textFile;
        private string[] _lines;

        private int _highScore;

        public int scored = 0;

        //Initialize ReadWrite Class.
        public ReadWrite()
        {
            _textFile = "score.txt";
            _lines = System.IO.File.ReadAllLines(_textFile);
            _highScore = int.Parse(_lines[0]);
        }

        //Get the info from users
        public void GetTheUserInfo(List<string> names, List<int> scores, List<string> passwords)
        {
            for (int i = 1; i < _lines.Count(); i++)
            {
                if (i > 0)
                {
                    string[] sort = _lines[i].Split(",");
                    names.Add(sort[0]);
                    scores.Add(int.Parse(sort[1]));
                    passwords.Add(sort[2]);
                    Console.WriteLine(i+ ". "+ sort[0] + "'s score is " + sort[1]);
                }
                
            }
        }
        public void IsUserHere(List<string> names, List<int> scores, List<string> passwords, string name, string password)
        {
            string matchScore = "";
            int matchNum = 0;
            for (int i = 0; i < names.Count(); i++)
            {               
                if (names[i].Contains(name) && passwords[i].Contains(password)) // (you use the word "contains". either equals or indexof might be appropriate)
                {
                    matchNum = i;
                    Console.WriteLine(matchNum + "-" + names[matchNum] + "-" +passwords[matchNum]);
                }
            }
            if (password == passwords[matchNum])
            {
                matchScore = $"{scores[matchNum]}";
            }
            else
            {
                matchScore = "0";
                CreateNewUser(names, scores, passwords, name, password, int.Parse(matchScore));
            }
            scored = int.Parse(matchScore);
        }

        public void CreateNewUser(List<string> names, List<int> scores, List<string> passwords, string name, string password, int score)
        {
            names.Add(name);
            scores.Add(score);
            passwords.Add(password);
            for (int i = 0; i < names.Count(); i++)
            {               
                Console.WriteLine($"{names[i]} {scores[i]} {passwords[i]}");
            }
        }

        //display score as if it were an arcade game.
        public void ScoreDisplay(int score)
        {
            Console.WriteLine($"High Score: {_highScore}");
            Console.WriteLine($"Current Score: {score}");
        }

        public void UpdateHighScore(int score, List<string> names, List<int> scores, List<string> passwords)
        {
            if (score > _highScore)
            {
                _highScore = score;
                Console.WriteLine($"You achieved a new HIGH SCORE! Which is {_highScore} points.");
                WriteFile(names, scores, passwords);
            }
            else
            {
                Console.WriteLine("");
                ScoreDisplay(score);
            }
            WriteFile(names, scores, passwords);
        }
        public void AmendChangesToScore(List<string> names, List<int> scores, List<string> passwords, string name, string password, int score, int newScore)
        {   
            for (int i = 0; i < names.Count(); i++)
            {   
                if (name == names[i] && password == passwords[i]) 
                {
                    scores[i] = newScore;
                }
            }
        }
        public void WriteFile(List<string> names, List<int> scores, List<string> passwords)
        {
            using (StreamWriter outputFile = new StreamWriter(_textFile))
            {
                outputFile.WriteLine(_highScore);
                for (int i = 0; i < names.Count(); i++)
                {
                    outputFile.WriteLine($"{names[i]},{scores[i]},{passwords[i]}");
                }
            }
        }
    }
}