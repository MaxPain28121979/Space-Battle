using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarControlMelee.Systems
{
    public static class UIManager
    {
        public static void DrawOverlay(SpriteBatch sb, Texture2D pixel, MeleeGame game)
        {
            // draw small health bars above ships
            foreach (var ship in game.Ships)
            {
                int barW = 60;
                int barH = 6;
                var pos = new Vector2(ship.Position.X - barW / 2f, ship.Position.Y - ship.Radius - 12f);
                var bg = new Rectangle((int)pos.X, (int)pos.Y, barW, barH);
                sb.Draw(pixel, bg, Color.DarkGray);

                float healthPct = (float)ship.Health / ship.MaxHealth;
                var fg = new Rectangle((int)pos.X, (int)pos.Y, (int)(barW * healthPct), barH);
                sb.Draw(pixel, fg, ship.Color);
            }
        }
    }
}
