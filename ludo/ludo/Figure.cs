using System;
using System.Collections.Generic;
using System.Text;

namespace ludo
{
    public class Figure
    {
        public string Color { get; private set; }
        public Field StandingOn { get; set; }

        public Figure(string color)
        {
            Color = color;
            StandingOn = null;
        }

        public void Move(Field field)
        {
            field.Move(this);
        }

        public void GoBackToHome()
        {
            StandingOn = null;
        }
    }
}
