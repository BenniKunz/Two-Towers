using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace Tower_Defence
{
    public interface IGameParts
    {
        void Update(GameTime gameTime, List<IGameParts> gameParts);

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    }
}
