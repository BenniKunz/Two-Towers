using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tower_Defence.Animations
{
    public class Animation
    {
        public Texture2D Texture { get; set; }
        public int CurrentColumn { get; set; }
        public int CurrentRow { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public int FrameHeight { get { return Texture.Height / RowCount; } }
        public float FrameSpeed { get; set; }
        public int FrameWidth { get { return Texture.Width / ColumnCount; } }
        public Animation(Texture2D texture, int rowCount, int columnCount)
        {
            Texture = texture;
            RowCount = rowCount;
            ColumnCount = columnCount;

            FrameSpeed = 0.05f;
        }
    }
}