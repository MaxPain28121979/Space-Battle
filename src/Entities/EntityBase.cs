using Microsoft.Xna.Framework;

namespace StarControlMelee
{
    public abstract class EntityBase
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float GravityMultiplier = 1f;
    }
}
