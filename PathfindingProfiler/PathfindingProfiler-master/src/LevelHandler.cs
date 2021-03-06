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
using System.Windows.Forms;

namespace Pathfinder
{
    /// <summary>
    /// The level handler component handles the current level. 
    /// </summary>
    class LevelHandler : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Spritebatch
        SpriteBatch spriteBatch;

        // The currently loaded level.
        private static Level level;

        // Tile Textures
        private Texture2D tileTexture1;
        private Texture2D tileTexture2;
        private Texture2D playerTexture;
        private Texture2D botTexture;

        public static void SetMap(string mapFile)
        {
            level.LoadMap(mapFile);
        }

        public static void SetPathfindingAlgorithm(PathfinderAlgorithm algorithm)
        {
            level.SetPathfindingAlgorithm(algorithm);
        }

        public static void SetPlayerPosition(Coord2 pos)
        {
            level.SetPlayerPosition(pos);
        }

        public static void SetBotPosition(Coord2 pos)
        {
            level.SetBotPosition(pos);
        }

        public LevelHandler(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create sprite batch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures
            tileTexture1    = Game.Content.Load<Texture2D>("Images/tile1");
            tileTexture2    = Game.Content.Load<Texture2D>("Images/tile2");
            playerTexture   = Game.Content.Load<Texture2D>("Images/target");
            botTexture      = Game.Content.Load<Texture2D>("Images/ai");

            level = new Level(new Map(tileTexture1, tileTexture2),
                              new Player(playerTexture, 0, 0),
                              new AiBotBlank(botTexture, 0, 0));

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            level.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            level.Update(gameTime);

            // Check if user wishes to return to menu
            if(InputHandler.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                if (MessageBox.Show("Return to the menu?", "Return to Menu", MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == DialogResult.Yes)
                    Main.SetState(typeof(Menu));
            }

            base.Update(gameTime);
        }

        public static TestResult RunTest(PathfinderAlgorithm algorithm, Coord2 startPos, Coord2 targetPos)
        {
            // Set the pathfinding algorithm
            Level.SetPathfindingAlgorithm(algorithm);

            // Get Start Time
            DateTime startTime = DateTime.Now;

            // Find Path
            Level.Map.pathfinder.Build(startPos, targetPos);

            // Calculate time taken
            DateTime finishTime = DateTime.Now;
            TimeSpan timeTaken = finishTime - startTime;

            // Return results
            return new TestResult(timeTaken.Ticks, Level.Map.pathfinder.GetPath().Count);
        }

        public static Level Level { get { return level; } }
    }
}
