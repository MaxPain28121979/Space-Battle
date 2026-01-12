using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace StarControlMelee.Systems
{
    public static class CollisionSystem
    {
        public static void HandleCollisions(MeleeGame game)
        {
            for (int i = game.Projectiles.Count - 1; i >= 0; i--)
            {
                var proj = game.Projectiles[i];
                foreach (var ship in game.Ships)
                {
                    if (!ship.IsAlive || ship == proj.Owner)
                        continue;

                    if (Vector2.Distance(ship.Position, proj.Position) < ship.Radius + proj.Radius)
                    {
                        ship.TakeDamage(proj.Damage);
                        game.Projectiles.RemoveAt(i);
                        break;
                    }
                }
            }

            foreach (var ship in game.Ships)
            {
                if (!ship.IsAlive) continue;

                if (Vector2.Distance(ship.Position, game.Planet.Position) < game.Planet.Radius + ship.Radius)
                    ship.TakeDamage(9999);
            }

            for (int i = game.Projectiles.Count - 1; i >= 0; i--)
            {
                if (Vector2.Distance(game.Projectiles[i].Position, game.Planet.Position) < game.Planet.Radius + game.Projectiles[i].Radius)
                    game.Projectiles.RemoveAt(i);
            }
        }
    }
}
