using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StarControlMelee
{
    public class Ship : EntityBase
    {
        public float Rotation;
        public float Radius = 80f;

        public float ThrustPower = 150f;
        public float TurnSpeed = 3f;
        public float MaxSpeed = 250f;

        public int MaxHealth = 20;
        public int Health;

        public float FireCooldown = 0.3f;
        private float _fireTimer;

        public Color Color = Color.White;

        public bool IsAlive => Health > 0;

        public Ship(Vector2 startPos, float startRotation, Color color)
        {
            Position = startPos;
            Rotation = startRotation;
            Color = color;
            Health = MaxHealth;
        }

        public void Update(GameTime gameTime, float thrustInput, float turnInput, bool firePressed, MeleeGame meleeGame)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            meleeGame.ApplyGravity(this, dt);

            Rotation += turnInput * TurnSpeed * dt;

            if (thrustInput > 0f)
            {
                Vector2 forward = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
                Velocity += forward * ThrustPower * thrustInput * dt;
            }

            if (Velocity.Length() > MaxSpeed)
            {
                Velocity.Normalize();
                Velocity *= MaxSpeed;
            }

            Position += Velocity * dt;
            Velocity *= 0.99f;

            meleeGame.WrapPosition(ref Position);

            _fireTimer -= dt;
            if (firePressed && _fireTimer <= 0f)
            {
                _fireTimer = FireCooldown;
                FireProjectile(meleeGame);
            }
        }

        private void FireProjectile(MeleeGame meleeGame)
        {
            Vector2 dir = new((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
            Vector2 projPos = Position + dir * (Radius + 5f);
            Vector2 projVel = Velocity + dir * 400f;

            meleeGame.Projectiles.Add(new Projectile(projPos, projVel, this));
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            float scale = (Radius * 2f) / texture.Width;
            StarControlMelee.Graphics.SpriteBatchExtensions.DrawCentered(spriteBatch, texture, Position, Rotation, Color, scale);
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health < 0) Health = 0;
        }
    }
}
