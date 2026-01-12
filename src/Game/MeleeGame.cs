using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace StarControlMelee
{
    public class MeleeGame
    {
        public List<Ship> Ships = new();
        public List<Projectile> Projectiles = new();

        public int ScreenWidth;
        public int ScreenHeight;

        public Planet Planet;

        public MeleeGame(int width, int height)
        {
            ScreenWidth = width;
            ScreenHeight = height;

            Planet = new Planet(new Vector2(width / 2f, height / 2f));

            ResetMatch();
        }

        public void ResetMatch()
        {
            StarControlMelee.Systems.SpawnSystem.ResetMatch(this);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = Projectiles.Count - 1; i >= 0; i--)
            {
                var proj = Projectiles[i];
                proj.Update(gameTime, this);
                if (!proj.IsAlive)
                    Projectiles.RemoveAt(i);
            }

            // delegate collision handling to the collision system
            StarControlMelee.Systems.CollisionSystem.HandleCollisions(this);
        }

        public void ApplyGravity(EntityBase entity, float dt)
        {
            StarControlMelee.Systems.Physics.ApplyGravity(entity, Planet, dt);
        }

        public void WrapPosition(ref Vector2 pos)
        {
            StarControlMelee.Systems.Physics.WrapPosition(ref pos, ScreenWidth, ScreenHeight);
        }
    }
}
