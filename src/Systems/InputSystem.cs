using Microsoft.Xna.Framework.Input;

namespace StarControlMelee.Systems
{
    public struct PlayerInput
    {
        public float Thrust;
        public float Turn;
        public bool Fire;
    }

    public static class InputSystem
    {
        public static void GetPlayerInputs(KeyboardState kb, KeyboardState prev, out PlayerInput p1, out PlayerInput p2)
        {
            p1 = new PlayerInput();
            p2 = new PlayerInput();

            p1.Thrust = kb.IsKeyDown(Keys.W) ? 1f : 0f;
            p1.Turn = (kb.IsKeyDown(Keys.A) ? -1f : 0f) + (kb.IsKeyDown(Keys.D) ? 1f : 0f);
            p1.Fire = kb.IsKeyDown(Keys.Space) && !prev.IsKeyDown(Keys.Space);

            p2.Thrust = kb.IsKeyDown(Keys.Up) ? 1f : 0f;
            p2.Turn = (kb.IsKeyDown(Keys.Left) ? -1f : 0f) + (kb.IsKeyDown(Keys.Right) ? 1f : 0f);
            p2.Fire = kb.IsKeyDown(Keys.RightControl) && !prev.IsKeyDown(Keys.RightControl);
        }
    }
}
