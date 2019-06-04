﻿using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowfireRL
{
    class Game
    {
        // The screen height and width are in number of tiles
        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70;
        private static RLRootConsole _rootConsole;

        // The map console takes up most of the screen and is where the map will be drawn
        private static readonly int _mapWidth = 80;
        private static readonly int _mapHeight = 48;
        private static RLConsole _mapConsole;

        // Below the map console is the message console which displays attack rolls and other information
        private static readonly int _messageWidth = 80;
        private static readonly int _messageHeight = 11;
        private static RLConsole _messageConsole;

        // The stat console is to the right of the map and display player and monster stats
        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 70;
        private static RLConsole _statConsole;

        // Above the map is the inventory console which shows the players equipment, abilities, and items
        private static readonly int _inventoryWidth = 80;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;

        private BasicEcs ecs;
        private EntityFactory entityFactory;
        private MapData mapData;
        private AbstractEntity currentUnit;
        private RLMouse prevRLMouse;

        public static void Main()
        {
            new Game();
        }


        private Game()
        {
            ecs = new BasicEcs();
            mapData = new MapData();
            entityFactory = new EntityFactory(ecs, mapData);

            CreateGame();

            // This must be the exact name of the bitmap font file we are using or it will error.
            string fontFileName = "terminal8x8.png";
            // The title will appear at the top of the console window
            string consoleTitle = "RougeSharp V3 Tutorial - Level 1";
            // Tell RLNet to use the bitmap font that we specified and that each tile is 8 x 8 pixels
            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);

            // Initialize the sub consoles that we will Blit to the root console
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statConsole = new RLConsole(_statWidth, _statHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);


            // Set up a handler for RLNET's Update event
            _rootConsole.Update += OnRootConsoleUpdate;
            // Set up a handler for RLNET's Render event
            _rootConsole.Render += OnRootConsoleRender;
            // Begin RLNET's game loop
            _rootConsole.Run();
        }


        private void CreateGame()
        {
            mapData.createMap(this.entityFactory, 50, 50);

            currentUnit = this.entityFactory.createPlayersUnit("Syylk", 5, 5);
        }


        private void SingleGameLoop()
        {

        }


        /*
         * Returns whether a game loop should be run.
         */
        private bool HandleKeyInput(RLKeyPress keyPress)
        {
            switch (keyPress.Char)
            {
            }
            return false;
        }


        private void HandleMouseEvent(RLMouse mouse)
        {

        }


        // Event handler for RLNET's Update event
        private void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            // Set background color and text for each console 
            // so that we can verify they are in the correct positions
            _mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, RLColor.Black);
            _mapConsole.Print(1, 1, "Map", RLColor.White);

            _mapConsole.Set(2, 2, RLColor.Red, RLColor.Green, 'L');
            RLCell cell = new RLCell(RLColor.Yellow, RLColor.Brown, 'X');
            _mapConsole.Set(3, 3, cell);

            _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, RLColor.Gray);
            _messageConsole.Print(1, 1, "Messages", RLColor.White);

            _statConsole.SetBackColor(0, 0, _statWidth, _statHeight, RLColor.Brown);
            _statConsole.Print(1, 1, "Stats", RLColor.White);

            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, RLColor.Cyan);
            _inventoryConsole.Print(1, 1, "Inventory", RLColor.White);

            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();
            if (keyPress != null)
            {
                _mapConsole.Print(10, 10, "Key: " + keyPress.ToString(), RLColor.White);
                if (this.HandleKeyInput(keyPress))
                {
                    this.SingleGameLoop();
                }
            }


            var str = _rootConsole.Mouse.X + "," + _rootConsole.Mouse.Y;
            _mapConsole.Print(10, 10, "Mouse: " + str, RLColor.White);

            if (this.prevRLMouse == null || (this.prevRLMouse.X != _rootConsole.Mouse.X || this.prevRLMouse.Y != _rootConsole.Mouse.Y || this.prevRLMouse.LeftPressed != _rootConsole.Mouse.LeftPressed))
            {
                this.HandleMouseEvent(_rootConsole.Mouse);
            }
        }


        // Event handler for RLNET's Render event
        private void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            // Blit the sub consoles to the root console in the correct locations
            RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
            RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
            RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
            RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

            // Tell RLNET to draw the console that we set
            _rootConsole.Draw();
        }

    }

}

