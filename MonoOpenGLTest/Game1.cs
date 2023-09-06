using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.VectorDraw;

namespace MonoOpenGLTest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private PrimitiveBatch _primitiveBatch;
        private PrimitiveDrawing _primitiveDrawing;

        private Matrix _localProjection;
        private Matrix _localView;

        private SpriteFont font;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _primitiveBatch = new PrimitiveBatch(GraphicsDevice);
            _primitiveDrawing = new PrimitiveDrawing(_primitiveBatch);

            _localProjection = Matrix.CreateOrthographicOffCenter(0f, GraphicsDevice.Viewport.Width, 0, GraphicsDevice.Viewport.Height, 0f, 1f);
            _localView = Matrix.Identity;

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("font");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            float w = GraphicsDevice.Viewport.Width;
            float h = GraphicsDevice.Viewport.Height;

            _localProjection = Matrix.CreateOrthographicOffCenter(0f, w, h, 0, 0f, 1f);
            _localView = Matrix.Identity;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _primitiveBatch.Begin(ref _localProjection, ref _localView);
            _primitiveDrawing.DrawSegment(new Vector2(0, h * 0.5f), new Vector2(w, h * 0.5f), Color.Red);
            _primitiveDrawing.DrawSegment(new Vector2(w * 0.5f, 0), new Vector2(w * 0.5f, h), Color.Blue);
            _primitiveBatch.End();

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            string text = "Hello World!\nSecond line";
            Vector2 textSize = font.MeasureString(text);
            Vector2 position = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
            _spriteBatch.DrawString(font, text, position, Color.White);
            _spriteBatch.DrawString(font, text, position, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.DrawRectangle(new RectangleF(position, textSize), Color.Yellow);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}