using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarControlMelee
{
    public class Planet
    {
        public Vector2 Position;
        public float Radius = 60f;
        public float GravityStrength = 90000f;

        public Planet(Vector2 pos)
        {
            Position = pos;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            int diameter = (int)(Radius * 2f);
            var dest = new Microsoft.Xna.Framework.Rectangle(
                (int)(Position.X - Radius),
                (int)(Position.Y - Radius),
                diameter,
                diameter);

            spriteBatch.Draw(pixel, dest, Color.Green);
        }
    }
}
