using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ludo
{
    public class Player
    {
        public string Color { get; private set; }

        
        public List<Figure> Figures { get; private set; }
        public List<Field> FieldWay { get; private set; }
        public List<Field> Homes { get; private set; }

        public Player(string color, int offSet, List<Field> mainField)
        {
            Figures = new List<Figure>();
            FieldWay = new List<Field>();
            Homes = new List<Field>();



            Color = color;

            Debug.Assert(mainField.Count == 40);

            for(var i = 0; i < mainField.Count; i++)
            {
                if (i + offSet < mainField.Count - 1)
                {
                    FieldWay.Add(mainField[i + offSet]);
                }
                else
                {
                    FieldWay.Add(mainField[i + offSet - (mainField.Count -1)]);
                }
            }

            FieldWay[0].NeedsToMove = Color;

            for (int i = 0; i < 4; i++)
            {
                Figures.Add(new Figure(Color));

                var Home = new Field();
                Home.IsHome = true;
                FieldWay.Add(Home);
                Homes.Add(Home);
            }
        }

        public bool decideToMove(int diceNumber, out int Rolls)
        {
            Rolls = 0;

            if (!AllAtHome() && diceNumber != 6)
            {
                Rolls = 2;
                return true;
            }

            if (MoveFromStart(diceNumber)) return false;

            if (MoveFromHome(diceNumber)) return false;

            //can kick some one
            if (KickPlayer(diceNumber)) return false;

            Dictionary<Figure, int> possibleChoices = new Dictionary<Figure, int>();

            GetPossibleChoices(diceNumber, possibleChoices);

            RemovePossibleChoices(possibleChoices);

            if (possibleChoices.Count >= 1)
            {
                Move(diceNumber, possibleChoices.Keys.First());

                if (diceNumber == 6)
                {
                    Rolls = 0;
                    return true;
                }
            }

            return false;
        }

        private bool AllAtHome()
        {
            foreach (var figure in Figures)
            {
                if (figure.StandingOn != null)
                {
                    return false;
                }
            }

            return true;
        }

        private void RemovePossibleChoices(Dictionary<Figure, int> possibleChoices)
        {
            foreach (var figure in Figures)
            {
                if (figure.StandingOn != null)
                {
                    Dictionary<Figure, int> possibleChoicesCopy = new Dictionary<Figure, int>(possibleChoices);
                    foreach (var KeyValue in possibleChoicesCopy)
                    {
                        int start = FieldWay.IndexOf(KeyValue.Key.StandingOn);
                        int end = KeyValue.Value;
                        if (start < FieldWay.IndexOf(figure.StandingOn) && end > FieldWay.IndexOf(figure.StandingOn))
                        {
                            possibleChoices.Remove(KeyValue.Key);
                        }
                    }

                    foreach (var KeyValue in possibleChoicesCopy)
                    {
                        if (KeyValue.Value == FieldWay.IndexOf(KeyValue.Key.StandingOn))
                        {
                            possibleChoices.Remove(KeyValue.Key);
                        } 
                    }
                }
            }
        }

        private void GetPossibleChoices(int diceNumber, Dictionary<Figure, int> possibleChoices)
        {
            foreach (var figure in Figures)
            {
                if (figure.StandingOn != null)
                {
                    int possibleIndex = FieldWay.IndexOf(figure.StandingOn) + diceNumber - 1;
                    if (possibleIndex > FieldWay.Count - 1 )
                    {
                        continue;
                    }
                    if (FieldWay[possibleIndex].HasPlayer)
                    {
                        continue;
                    }

                    possibleChoices.Add(figure, possibleIndex);
                }
            }
        }

        private bool KickPlayer(int diceNumber)
        {
            foreach (var figure in Figures)
            {
                if (figure.StandingOn != null)
                {
                    int possibleIndex = FieldWay.IndexOf(figure.StandingOn) + diceNumber - 1;
                    if (possibleIndex > FieldWay.Count - 1)
                    {
                        continue;
                    }
                    if (FieldWay[possibleIndex].HasPlayer && FieldWay[possibleIndex].Player.Color != Color)
                    {
                        figure.Move(FieldWay[0]);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool MoveFromHome(int diceNumber)
        {
            if (diceNumber == 6)
            {
                foreach (var figure in Figures)
                {
                    if (figure.StandingOn == null)
                    {
                        if (FieldWay[0].Player != null)
                        {
                            if (FieldWay[0].Player.Color == this.Color)
                            {
                                return false;
                            }
                        }
                        figure.Move(FieldWay[0]);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool MoveFromStart(int diceNumber)
        {
            foreach (var figure in Figures)
            {
                if (figure.StandingOn != null)
                {
                    if (figure.StandingOn.NeedsToMove == Color)
                    {
                        if (this.Move(diceNumber, figure))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                        
                    }
                }
            }

            return false;
        }

        private bool Move(int number, Figure figure)
        {
            int moveToo = number;

            if (figure.StandingOn != null)
            {
                moveToo += FieldWay.IndexOf(figure.StandingOn) - 1;
            }

            if (FieldWay[moveToo].Player != null)
            {
                if (FieldWay[moveToo].Player.Color == this.Color)
                {
                    return false;
                }
            }

            figure.Move(FieldWay[moveToo]);

            return true;

        } 

        public bool HasWon()
        {
            foreach (var home in Homes)
            {
                if (!home.HasPlayer)
                {
                    return false;
                }
            }
            //Console.WriteLine(Color);
            return true;
        }
        
    }
}
