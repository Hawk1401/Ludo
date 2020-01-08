using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ludo
{
    public class Field
    {
        public string NeedsToMove { get; set; }

        public bool IsHome { get; set; }
        public string ID { get; private set; }
        public bool HasPlayer { get; private set; }
        public Figure Player { get; private set; }

        public Field()
        {
            ID = Guid.NewGuid().ToString();
        }

        public void Move(Figure figure)
        {
            if (this.Player != null)
            {
                this.Player.GoBackToHome();
            }

            this.Player = figure;
            HasPlayer = true;

            if (figure.StandingOn != null)
            {
                figure.StandingOn.MovedAway();
            }

            figure.StandingOn = this;
        }

        public void MovedAway()
        {
            this.Player = null;
            HasPlayer = false;
        }

    }
}
