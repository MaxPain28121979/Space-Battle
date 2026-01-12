using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace StarControlMelee
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _pixel;
        private Texture2D _planetTexture;
        private Texture2D _shipTexture;
        private Texture2D _projectileTexture;
        private SpriteFont? _font;

        private MeleeGame _meleeGame;

        private KeyboardState _prevKeyboard;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _meleeGame = new MeleeGame(
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            // prefer loading static PNGs from Content/Textures; fall back to procedural generation
            var texturesDir = System.IO.Path.Combine(Content.RootDirectory, "Textures");
            var shipFile = System.IO.Path.Combine(texturesDir, "ship_high.png");
            var projFile = System.IO.Path.Combine(texturesDir, "proj_high.png");
            var planetFile = System.IO.Path.Combine(texturesDir, "planet.png");

            if (System.IO.File.Exists(shipFile) && System.IO.File.Exists(projFile))
            {
                using (var fs = System.IO.File.OpenRead(shipFile))
                    _shipTexture = Texture2D.FromStream(GraphicsDevice, fs);
                using (var fs = System.IO.File.OpenRead(projFile))
                    _projectileTexture = Texture2D.FromStream(GraphicsDevice, fs);
            }
            else
            {
                _shipTexture = StarControlMelee.Graphics.TextureFactory.CreateShipTexture(GraphicsDevice, 128, 64);
                _projectileTexture = StarControlMelee.Graphics.TextureFactory.CreateProjectileTexture(GraphicsDevice, 24);
            }

            if (System.IO.File.Exists(planetFile))
            {
                using (var fs = System.IO.File.OpenRead(planetFile))
                    _planetTexture = Texture2D.FromStream(GraphicsDevice, fs);
            }
            else
            {
                _planetTexture = StarControlMelee.Graphics.TextureFactory.CreatePlanetTexture(GraphicsDevice, 256);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Escape))
                Exit();

            float p1Thrust = kb.IsKeyDown(Keys.W) ? 1f : 0f;
            float p1Turn = (kb.IsKeyDown(Keys.A) ? -1f : 0f) + (kb.IsKeyDown(Keys.D) ? 1f : 0f);
            bool p1Fire = kb.IsKeyDown(Keys.Space) && !_prevKeyboard.IsKeyDown(Keys.Space);

            float p2Thrust = kb.IsKeyDown(Keys.Up) ? 1f : 0f;
            float p2Turn = (kb.IsKeyDown(Keys.Left) ? -1f : 0f) + (kb.IsKeyDown(Keys.Right) ? 1f : 0f);
            bool p2Fire = kb.IsKeyDown(Keys.RightControl) && !_prevKeyboard.IsKeyDown(Keys.RightControl);

            if (_meleeGame.Ships.Count >= 2)
            {
                var s1 = _meleeGame.Ships[0];
                var s2 = _meleeGame.Ships[1];

                if (s1.IsAlive)
                    s1.Update(gameTime, p1Thrust, p1Turn, p1Fire, _meleeGame);

                if (s2.IsAlive)
                    s2.Update(gameTime, p2Thrust, p2Turn, p2Fire, _meleeGame);
            }

            _meleeGame.Update(gameTime);

            bool someoneDead = false;
            foreach (var ship in _meleeGame.Ships)
                if (!ship.IsAlive) someoneDead = true;

            if (someoneDead && kb.IsKeyDown(Keys.Enter) && !_prevKeyboard.IsKeyDown(Keys.Enter))
                _meleeGame.ResetMatch();

            _prevKeyboard = kb;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _meleeGame.Planet.Draw(_spriteBatch, _planetTexture);

            foreach (var ship in _meleeGame.Ships)
                if (ship.IsAlive)
                    ship.Draw(_spriteBatch, _shipTexture);

            foreach (var proj in _meleeGame.Projectiles)
                proj.Draw(_spriteBatch, _projectileTexture);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
