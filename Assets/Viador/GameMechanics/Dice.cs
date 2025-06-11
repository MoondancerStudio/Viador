using System;

namespace Viador.GameMechanics
{
    public class Dice
    {
        public static Dice D10 = new();
        private int _diceCount;
        private int _diceSides;
        
        private Random _random;

        public Dice(int diceSides = 10, int diceCount = 1)
        {
            this._diceCount = diceCount;
            this._diceSides = diceSides;
            
            this._random = new Random();
        }

        public virtual int Roll()
        {
            return _random.Next(_diceCount, _diceCount * _diceSides);
        }
    }
}