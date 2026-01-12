using Microsoft.Xna.Framework;

namespace StarControlMelee.Systems
{
    public static class Physics
    {
        public static void ApplyGravity(EntityBase entity, Planet planet, float dt)
        {
            Vector2 dir = planet.Position - entity.Position;
            float distSq = dir.LengthSquared();

            if (distSq < 1f)
                return;

            float force = planet.GravityStrength / distSq;
            dir.Normalize();

            entity.Velocity += dir * force * entity.GravityMultiplier * dt;
        }

        public static void WrapPosition(ref Vector2 pos, int screenWidth, int screenHeight)
        {
            if (pos.X < 0) pos.X += screenWidth;
            if (pos.X > screenWidth) pos.X -= screenWidth;
            if (pos.Y < 0) pos.Y += screenHeight;
            if (pos.Y > screenHeight) pos.Y -= screenHeight;
        }
    }
}
