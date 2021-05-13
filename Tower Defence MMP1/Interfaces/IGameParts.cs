//MultiMediaTechnology 
//FHS 45891
//MultiMediaProjekt 1
//Benjamin Kunz

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tower_Defence.Sprites;

namespace Tower_Defence
{
    public interface IGameParts
    {
        void Update(GameTime gameTime, List<IGameParts> gameParts, List<Tower> backgroundTowers = null);

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    }
}
