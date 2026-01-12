using Microsoft.Xna.Framework;

namespace StarControlMelee.Systems
{
    public static class SpawnSystem
    {
        public static void ResetMatch(MeleeGame game)
        {
            game.Ships.Clear();
            game.Projectiles.Clear();

            game.Ships.Add(new Ship(new(game.ScreenWidth * 0.25f, game.ScreenHeight * 0.5f), 0f, Color.CornflowerBlue));
            game.Ships.Add(new Ship(new(game.ScreenWidth * 0.75f, game.ScreenHeight * 0.5f), MathF.PI, Color.OrangeRed));
        }
    }
}
