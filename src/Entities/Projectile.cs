using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarControlMelee
{
    public class Projectile : EntityBase
    {
        public float Radius = 4f;
        public int Damage = 2;
        public float LifeTime = 2f;
        public Ship Owner;

        public bool IsAlive => LifeTime > 0f;

        public Projectile(Vector2 pos, Vector2 vel, Ship owner)
        {
            Position = pos;
            Velocity = vel;
            Owner = owner;
            GravityMultiplier = 0.2f;
        }

        public void Update(GameTime gameTime, MeleeGame meleeGame)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            meleeGame.ApplyGravity(this, dt);

            Position += Velocity * dt;
            LifeTime -= dt;

            meleeGame.WrapPosition(ref Position);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            float scale = (Radius * 2f) / texture.Width;
            StarControlMelee.Graphics.SpriteBatchExtensions.DrawCentered(spriteBatch, texture, Position, 0f, Color.Yellow, scale);
        }
    }
}
