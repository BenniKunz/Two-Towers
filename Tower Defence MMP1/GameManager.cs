using System;
using System.Collections.Generic;
using System.Text;
using Tower_Defence.Enums;

namespace Tower_Defence
{
    public sealed class GameManager
    {

        public static GameManager GameManagerInstance { get { return Nested.gameManagerInstance; } }

        public Difficulty Difficulty { get; set; }
        public int StoppedEnemies { get; set; }
        public bool MathOperationIsUsed { get; set; }
        public bool EnemyIsHit { get; set; }
        private GameManager()
        {
            
        }

        private class Nested
        {
            static Nested()
            {

            }
            internal static readonly GameManager gameManagerInstance = new GameManager();
        }
        

    }
}
