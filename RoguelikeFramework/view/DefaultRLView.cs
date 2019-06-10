using RLNET;

namespace RoguelikeFramework.view {

    public class DefaultRLView //: IGameView
    {
        // The screen height and width are in number of tiles
        private const int _screenWidth = 100;
        private const int _screenHeight = 70;
        public RLRootConsole rootConsole;

        // The map console takes up most of the screen and is where the map will be drawn
        private const int _mapWidth = _screenWidth - crewListWidth;
        public const int _mapHeight = _screenHeight - _messageHeight;
        public RLConsole mapConsole;

        // Below the map console is the message console which displays attack rolls and other information
        private const int _messageWidth = 80;
        public const int _messageHeight = 11;
        public RLConsole messageConsole;

        // Above the map is the inventory console which shows the players equipment, abilities, and items
        private const int crewListWidth = 20;
        private const int crewListHeight = 8;
        public RLConsole crewListConsole;

        // The stat console is to the right of the map and display player and monster stats
        private const int _statWidth = crewListWidth;
        private const int _statHeight = 70;
        public RLConsole statConsole;

        private AbstractRoguelike game;
        private Mouse prevRLMouse = new Mouse();

        public DefaultRLView(AbstractRoguelike _game) {
            this.game = _game;

            // This must be the exact name of the bitmap font file we are using or it will error.
            string fontFileName = "terminal8x8.png";
            // The title will appear at the top of the console window
            string consoleTitle = "RougeSharp V3 Tutorial - Level 1";
            // Tell RLNet to use the bitmap font that we specified and that each tile is 8 x 8 pixels
            this.rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);

            // Initialize the sub consoles that we will Blit to the root console
            this.mapConsole = new RLConsole(_mapWidth, _mapHeight);
            this.messageConsole = new RLConsole(_messageWidth, _messageHeight);
            this.statConsole = new RLConsole(_statWidth, _statHeight);
            this.crewListConsole = new RLConsole(crewListWidth, crewListHeight);


            // Set up a handler for RLNET's Update event
            this.rootConsole.Update += this.OnRootConsoleUpdate;
            // Set up a handler for RLNET's Render event
            this.rootConsole.Render += this.OnRootConsoleRender;
            // Begin RLNET's game loop
        }


        public void Start() {
            this.rootConsole.Run();
        }


        // Event handler for RLNET's Update event
        private void OnRootConsoleUpdate(object sender, UpdateEventArgs e) {
            // Set background color and text for each console 
            // so that we can verify they are in the correct positions
            this.mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, RLColor.Black);
            this.mapConsole.Print(1, 1, "Map", RLColor.White);

            /*this._mapConsole.Set(2, 2, RLColor.Red, RLColor.Green, 'L');
            RLCell cell = new RLCell(RLColor.Yellow, RLColor.Brown, 'X');
            this._mapConsole.Set(3, 3, cell);*/

            this.messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, RLColor.Gray);
            this.messageConsole.Print(1, 1, "Messages", RLColor.White);

            this.statConsole.SetBackColor(0, 0, _statWidth, _statHeight, RLColor.Brown);
            this.statConsole.Print(1, 1, "Stats", RLColor.White);

            this.crewListConsole.SetBackColor(0, 0, crewListWidth, crewListHeight, RLColor.Cyan);
            this.crewListConsole.Print(1, 1, "Crew", RLColor.White);

            this.game.Repaint();

            RLKeyPress keyPress = this.rootConsole.Keyboard.GetKeyPress();
            if (keyPress != null) {
                //this.mapConsole.Print(10, 10, "Key: " + keyPress.ToString(), RLColor.White);
                this.game.HandleKeyInput(keyPress);
            }

            //var str = _rootConsole.Mouse.X + "," + _rootConsole.Mouse.Y;
            //_mapConsole.Print(10, 10, "Mouse: " + str, RLColor.White);
            if (this.prevRLMouse.x != this.rootConsole.Mouse.X || this.prevRLMouse.y != this.rootConsole.Mouse.Y || this.prevRLMouse.left != this.rootConsole.Mouse.LeftPressed) {
                this.game.HandleMouseEvent(this.rootConsole.Mouse);
                this.prevRLMouse.x = this.rootConsole.Mouse.X;
                this.prevRLMouse.y = this.rootConsole.Mouse.Y;
                this.prevRLMouse.left = this.rootConsole.Mouse.GetLeftClick();
            }
        }


        // Event handler for RLNET's Render event
        private void OnRootConsoleRender(object sender, UpdateEventArgs e) {
            // Blit the sub consoles to the root console in the correct locations
            RLConsole.Blit(this.mapConsole, 0, 0, _mapWidth, _mapHeight, this.rootConsole, 0, 0);
            RLConsole.Blit(this.crewListConsole, 0, 0, crewListWidth, crewListHeight, this.rootConsole, _mapWidth, 0);
            RLConsole.Blit(this.statConsole, 0, 0, _statWidth, _statHeight, this.rootConsole, _mapWidth, crewListHeight);
            RLConsole.Blit(this.messageConsole, 0, 0, _messageWidth, _messageHeight, this.rootConsole, 0, _screenHeight - _messageHeight);

            // Tell RLNET to draw the console that we set
            this.rootConsole.Draw();
        }

    }
}
