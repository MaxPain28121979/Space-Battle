using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarControlMelee.Graphics
{
    public static class SpriteBatchExtensions
    {
        public static void DrawCentered(this SpriteBatch sb, Texture2D texture, Vector2 position, float rotation, Color color, float scale = 1f)
        {
            var origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            sb.Draw(texture, position, null, color, rotation, origin, scale, SpriteEffects.None, 0f);
        }

        public static void DrawRect(this SpriteBatch sb, Texture2D texture, Rectangle dest, Color color)
        {
            sb.Draw(texture, dest, color);
        }
    }
}
