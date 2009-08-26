using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Controllers;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Factories;
using FarseerGames.FarseerPhysics.Interfaces;
using FarseerGames.FarseerPhysics.Mathematics;
using FarseerGames.FarseerPhysics.Dynamics.Joints;

namespace MarioPhysicsTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        PhysicsSimulator phySim;
        Camera2d camera;
        Dictionary<string, Texture2D> assets;
        List<GameObject> gameObjects;

        GameObject player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();

            assets = new Dictionary<string, Texture2D>();
            gameObjects = new List<GameObject>();

            phySim = new PhysicsSimulator(new Vector2(0, -200));

            var ballObject = GameObjectFactory.CreateCircleGameObject(phySim, "ball", 22f, 1);
            ballObject.Center = new Vector2(250, 400);
            ballObject.Geom.FrictionCoefficient = 0;

            FixedAngleJoint asdf = new FixedAngleJoint(ballObject.Body, 0);
            phySim.Add(asdf);

            player = ballObject;
            gameObjects.Add(ballObject);

            var platformObject = GameObjectFactory.CreateRectangleGameObject(phySim, "platform", 256, 64, 6);
            platformObject.IsStatic = true;
            platformObject.Center = new Vector2(250, 300);
            platformObject.Rotation = MathHelper.ToRadians(-30f);

            var platformObject2 = GameObjectFactory.CreateRectangleGameObject(phySim, "platform", 256, 64, 6);
            platformObject2.Center = new Vector2(5, 150);
            platformObject2.Rotation = MathHelper.ToRadians(90f);
            platformObject2.Geom.FrictionCoefficient = 10;

            var floorObject = GameObjectFactory.CreateRectangleGameObject(phySim, "floor", 1024, 12, 1);
            floorObject.Center = new Vector2(500, 0);
            floorObject.IsStatic = true;
            floorObject.Geom.FrictionCoefficient = 1;

            camera = new Camera2d(new Vector2(0, 0), new Vector2(800, 600), new Vector2(100, 100));

            gameObjects.Add(floorObject);
            gameObjects.Add(platformObject2);
            gameObjects.Add(platformObject);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            var ballTex = Content.Load<Texture2D>("ball");
            var platformTex = Content.Load<Texture2D>("platform");
            assets.Add("ball", ballTex);
            assets.Add("platform", platformTex);
            assets.Add("floor", Content.Load<Texture2D>("floor"));
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            var ks = Keyboard.GetState();
            
            camera.KeepOnScreen(player);

            if (ks.IsKeyDown(Keys.Left))
            {
                player.Geom.Body.LinearVelocity = new Vector2(-250, player.Body.LinearVelocity.Y);
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                player.Body.LinearVelocity = new Vector2(250, player.Body.LinearVelocity.Y);
            }

            if (ks.IsKeyDown(Keys.Left) && ks.IsKeyDown(Keys.A))
            {
                player.Body.LinearVelocity = new Vector2(-450, player.Body.LinearVelocity.Y);
            }
            if (ks.IsKeyDown(Keys.Right) && ks.IsKeyDown(Keys.A))
            {
                player.Body.LinearVelocity = new Vector2(450, player.Body.LinearVelocity.Y);
            }

            if (ks.IsKeyDown(Keys.Up))
            {
                player.Body.ApplyImpulse(new Vector2(0, 100));
            }

            float split = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * .001f)*2 / 3f;
            for (int i = 1; i < 3; i++)
            {
                phySim.Update(split);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            foreach (var go in gameObjects)
            {
                if (assets.ContainsKey(go.ResourceName))
                {
                    var asset = assets[go.ResourceName];

                    spriteBatch.Draw(asset, camera.GetScreenPosition(new Vector2(go.Center.X, go.Center.Y)), null, Color.White, -go.Rotation, new Vector2(asset.Width/2, asset.Height/2), 1f, SpriteEffects.None, 1f);
                }
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
