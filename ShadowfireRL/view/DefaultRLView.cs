using RLNET;
using RoguelikeFramework;

namespace ShadowfireRL.view {
    public class DefaultRLView //: IGameView
    {
        // The screen height and width are in number of tiles
        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70;
        public RLRootConsole _rootConsole;

        // The map console takes up most of the screen and is where the map will be drawn
        private static readonly int _mapWidth = 80;
        private static readonly int _mapHeight = 48;
        public RLConsole _mapConsole;

        // Below the map console is the message console which displays attack rolls and other information
        private static readonly int _messageWidth = 80;
        private static readonly int _messageHeight = 11;
        public RLConsole _messageConsole;

        // The stat console is to the right of the map and display player and monster stats
        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 70;
        public RLConsole _statConsole;

        // Above the map is the inventory console which shows the players equipment, abilities, and items
        /*private static readonly int _inventoryWidth = 80;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;*/

        private ShadowfireRL_Game game;
        private Mouse prevRLMouse = new Mouse();// RLMouse(0, 0, false, false);

        public DefaultRLView(ShadowfireRL_Game _game) {
            this.game = _game;

            // This must be the exact name of the bitmap font file we are using or it will error.
            string fontFileName = "terminal8x8.png";
            // The title will appear at the top of the console window
            string consoleTitle = "RougeSharp V3 Tutorial - Level 1";
            // Tell RLNet to use the bitmap font that we specified and that each tile is 8 x 8 pixels
            this._rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);

            // Initialize the sub consoles that we will Blit to the root console
            this._mapConsole = new RLConsole(_mapWidth, _mapHeight);
            this._messageConsole = new RLConsole(_messageWidth, _messageHeight);
            this._statConsole = new RLConsole(_statWidth, _statHeight);
            //_inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);


            // Set up a handler for RLNET's Update event
            this._rootConsole.Update += this.OnRootConsoleUpdate;
            // Set up a handler for RLNET's Render event
            this._rootConsole.Render += this.OnRootConsoleRender;
            // Begin RLNET's game loop
        }


        public void Start() {
            this._rootConsole.Run();
        }


        // Event handler for RLNET's Update event
        private void OnRootConsoleUpdate(object sender, UpdateEventArgs e) {
            // Set background color and text for each console 
            // so that we can verify they are in the correct positions
            this._mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, RLColor.Black);
            this._mapConsole.Print(1, 1, "Map", RLColor.White);

            /*this._mapConsole.Set(2, 2, RLColor.Red, RLColor.Green, 'L');
            RLCell cell = new RLCell(RLColor.Yellow, RLColor.Brown, 'X');
            this._mapConsole.Set(3, 3, cell);*/

            this._messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, RLColor.Gray);
            this._messageConsole.Print(1, 1, "Messages", RLColor.White);

            this._statConsole.SetBackColor(0, 0, _statWidth, _statHeight, RLColor.Brown);
            this._statConsole.Print(1, 1, "Stats", RLColor.White);

            //_inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, RLColor.Cyan);
            //_inventoryConsole.Print(1, 1, "Inventory", RLColor.White);

            game.repaint2();

            RLKeyPress keyPress = this._rootConsole.Keyboard.GetKeyPress();
            if (keyPress != null) {
                this._mapConsole.Print(10, 10, "Key: " + keyPress.ToString(), RLColor.White);
                this.game.HandleKeyInput(keyPress);
            }


            //var str = _rootConsole.Mouse.X + "," + _rootConsole.Mouse.Y;
            //_mapConsole.Print(10, 10, "Mouse: " + str, RLColor.White);
            if (this.prevRLMouse.x != this._rootConsole.Mouse.X || this.prevRLMouse.y != this._rootConsole.Mouse.Y || this.prevRLMouse.left != this._rootConsole.Mouse.LeftPressed) {
                this.game.HandleMouseEvent(this._rootConsole.Mouse);
                this.prevRLMouse.x = this._rootConsole.Mouse.X;
                this.prevRLMouse.y = this._rootConsole.Mouse.Y;
                this.prevRLMouse.left = this._rootConsole.Mouse.GetLeftClick();
            }
        }


        // Event handler for RLNET's Render event
        private void OnRootConsoleRender(object sender, UpdateEventArgs e) {
            // Blit the sub consoles to the root console in the correct locations
            RLConsole.Blit(this._mapConsole, 0, 0, _mapWidth, _mapHeight, this._rootConsole, 0, 0);
            RLConsole.Blit(this._statConsole, 0, 0, _statWidth, _statHeight, this._rootConsole, _mapWidth, 0);
            RLConsole.Blit(this._messageConsole, 0, 0, _messageWidth, _messageHeight, this._rootConsole, 0, _screenHeight - _messageHeight);
            //RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

            // Tell RLNET to draw the console that we set
            this._rootConsole.Draw();
        }

    }
}
