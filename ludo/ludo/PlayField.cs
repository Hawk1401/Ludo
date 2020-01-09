using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ludo
{
    public class PlayField
    {
        private Random Dice = null;
        private int MaxRolls = 10;
        private int Rolls = 0;

        private List<int> numbers = new List<int>();

        public int RollTheDice()
        {
            int nummber = Dice.Next(1, 7);
            numbers.Add(nummber);

            if (numbers.Count > 5000)
            {
                var playerHist =  new List<Hist>();

                Dictionary<string, string> translator = new Dictionary<string, string>();

                foreach (var Field in MainField)
                {
                    translator.Add(Field.ID, MainField.IndexOf(Field).ToString());
                }

                foreach (var Player in Players)
                {
                    List<string> moves = new List<string>();
                    foreach (var playerMoves in Player.moves)
                    {
                        if(translator.TryGetValue(playerMoves, out string value))
                        {
                            moves.Add(value);
                        }
                        else
                        {
                            moves.Add(playerMoves);
                        }
                        
                    }
                    var playerHistPart = new Hist
                    {
                        Color = Player.Color,
                        Moves = moves
                    };
                    playerHist.Add(playerHistPart);
                }
                string json = JsonConvert.SerializeObject(playerHist.ToArray());

                string path = Path.GetTempPath() + "ludo";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //write string to file
                System.IO.File.WriteAllText(path + @"\hist.json", json);
            }

            return nummber;
        }



        public List<Player> Players { get; private set; }
        public List<Field> MainField { get; private set; }

        public PlayField(Random dice)
        {
            MainField = new List<Field>();
            Players = new List<Player>();
            Dice = dice;

            for (int i = 0; i < 40; i++)
            {
                MainField.Add(new Field());
            }

            Players.Add(new Player("Red", 0, MainField));
            Players.Add(new Player("Blue", 10, MainField));
            Players.Add(new Player("Green", 20, MainField));
            Players.Add(new Player("Yellow", 30, MainField));
            Shuffle(Players);

        }

        public void Shuffle(List<Player> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Player value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public string Start()
        {
            while (true)
            {
                foreach (var Player in Players)
                {
                    Rolls++;
                    DecideToMove(Player);
                    Rolls = 0;

                    if (Player.HasWon())
                    {
                        return Player.Color;
                    }
                }
            }
        }

        private void DecideToMove(Player Player)
        {
            Player.LogMoves();
            if (Player.decideToMove(RollTheDice(), out int times))
            {
                for (int i = 0; i < times; i++)
                {
                    if (!Player.decideToMove(RollTheDice(), out int j))
                    {
                        Player.LogMoves();
                        return;
                    }

                    Player.LogMoves();
                }
                Player.LogMoves();
            }
        }
    }
}
