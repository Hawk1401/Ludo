using System;
using System.Collections.Generic;
using System.Threading;

namespace ludo
{
    public class Program
    {
        static void Main(string[] args)
        {
            for (int j = 0; j < 1; j++)
            {


                Random r = new Random();
                List<string> won = new List<string>();
                for (int i = 0; i < 1000000; i++)
                {
                    var play = new PlayField(r);
                    won.Add(play.Start());
                }

                int RedIndex = 0;
                int YellowIndex = 0;
                int GreenIndex = 0;
                int BlueIndex = 0;
                foreach (var win in won)
                {
                    switch (win)
                    {
                        case "Red":
                            RedIndex++;
                            break;
                        case "Blue":
                            BlueIndex++;
                            break;
                        case "Green":
                            GreenIndex++;
                            break;
                        case "Yellow":
                            YellowIndex++;
                            break;
                    }
                }

                Console.WriteLine("Red = " + RedIndex.ToString());
                Console.WriteLine("Blue = " + BlueIndex.ToString());
                Console.WriteLine("Green = " + GreenIndex.ToString());
                Console.WriteLine("Yellow = " + YellowIndex.ToString());
            }
        }
    }
}
