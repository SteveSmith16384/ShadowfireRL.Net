using RLNET;
using RoguelikeFramework.components;
using RoguelikeFramework.models;
using RoguelikeFramework.systems;
using RoguelikeFramework.view;
using System.Collections.Generic;
using System.Text;

namespace RoguelikeFramework {

    public abstract class AbstractRoguelike : IDataForView {

        public enum InputMode { Normal, SelectItemFromInv, SelectItemFromFloor, SelectMapPoint };
        public enum InputSubMode { None, ThrowingItem, DroppingItem, PickingUpItem };

        protected BasicEcs ecs;
        protected MapData mapData;
        protected GameLog gameLog;
        private string hoverText;
        private List<Point> line;
        private InputMode currentInputMode = InputMode.Normal;
        private InputSubMode currentInputSubMode = InputSubMode.None;
        protected DefaultRLView view;

        // Systems
        private DrawingSystem drawingSystem;
        private CheckMapVisibilitySystem checkVisibilitySystem;
        private PickupItemSystem pickupItemSystem;

        protected AbstractEntity currentUnit;
        public Dictionary<int, AbstractEntity> playersUnits = new Dictionary<int, AbstractEntity>();

        protected Dictionary<int, AbstractEntity> menuitemList; // For when selecting item to pick up etc...

        public AbstractRoguelike(int maxLogEntries) {
            this.ecs = new BasicEcs();
            this.mapData = new MapData();
            this.gameLog = new GameLog(maxLogEntries);

            this.CreateData();

            this.view = new DefaultRLView(this);
            this.drawingSystem = new DrawingSystem(this.view, this);
            this.drawingSystem.process();

            this.checkVisibilitySystem = new CheckMapVisibilitySystem(this.mapData);
            this.ecs.systems.Add(new ShootOnSightSystem(this.checkVisibilitySystem, this.ecs.entities));

            this.checkVisibilitySystem.process(this.playersUnits.Values);
            this.ecs.systems.Add(new MovementSystem(this.mapData, this.checkVisibilitySystem));
            this.pickupItemSystem = new PickupItemSystem();

        }


        protected abstract void CreateData();


        public void HandleKeyInput(RLKeyPress keyPress) {
            bool action_performed = false;
            //bool unit_moved = false; // Do we need to recalc visible squares?

            switch (keyPress.Key) {

                case RLKey.Number1:
                case RLKey.Number2:
                case RLKey.Number3:
                case RLKey.Number4:
                case RLKey.Number5:
                case RLKey.Number6:
                case RLKey.Number7:
                case RLKey.Number8:
                case RLKey.Number9:
                    if (this.currentInputMode == InputMode.SelectItemFromFloor) {
                        if (this.currentInputSubMode == InputSubMode.PickingUpItem) {
                            //todo pickupItemSystem.
                        }
                    } else if (this.currentInputMode == InputMode.SelectItemFromInv) {
                    } else {
                        this.SelectUnit(keyPress.Key - RLKey.Number0);
                    }
                    break;

                case RLKey.Up: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.getComponent(nameof(MovementDataComponent));
                        m.offY = -1;
                        action_performed = true;
                        //unit_moved = true;
                        break;
                    }
                case RLKey.Down: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.getComponent(nameof(MovementDataComponent));
                        m.offY = 1;
                        action_performed = true;
                        //unit_moved = true;
                        break;
                    }
                case RLKey.Left: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.getComponent(nameof(MovementDataComponent));
                        m.offX = -1;
                        action_performed = true;
                        //unit_moved = true;
                        break;
                    }
                case RLKey.Right: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.getComponent(nameof(MovementDataComponent));
                        m.offX = 1;
                        action_performed = true;
                        //unit_moved = true;
                        break;
                    }

                case RLKey.Space: {
                        action_performed = true;
                        break;
                    }

                case RLKey.P: {
                        this.currentInputMode = InputMode.SelectItemFromFloor;
                        this.currentInputSubMode = InputSubMode.PickingUpItem;
                        break;
                    }
            }

            if (action_performed) {
                this.SingleGameLoop();
            }
            //if (unit_moved) {
            this.checkVisibilitySystem.process(this.playersUnits.Values);
            //}
        }


        protected void SelectUnit(int num) {
            this.currentUnit = this.playersUnits[num];
            this.gameLog.Add(this.currentUnit.name + " selected");
        }


        private void SingleGameLoop() {
            this.ecs.process();
        }


        public void HandleMouseEvent(RLMouse mouse) {
            if (mouse.LeftPressed) {

            } else {
                this.hoverText = this.GetSquareDesc(mouse.X, mouse.Y);
                if (this.currentInputMode == InputMode.SelectMapPoint) {
                    this.line = Misc.line(0, 0, mouse.X, mouse.Y);
                } else {
                    this.line = null;
                }
            }
        }


        private string GetSquareDesc(int x, int y) {
            if (x < this.mapData.map.GetLength(0) && y < this.mapData.map.GetLength(1)) {
                var sb = new StringBuilder();
                var ents = this.mapData.map[x, y];
                //return string.Join(", ", ents);
                foreach (var e in ents) {
                    sb.Append(e.name + "; ");
                }

                return sb.ToString();
            }
            return "";
        }


        public MapData GetMapData() {
            return this.mapData;
        }


        public string GetHoverText() {
            return this.hoverText;
        }


        public List<string> GetLog() {
            return this.gameLog.GetEntries();
        }


        public List<Point> GetLine() {
            return this.line;
        }

        public Dictionary<int, AbstractEntity> GetUnits() {
            return this.playersUnits;
        }


        public AbstractEntity GetCurrentUnit() {
            return this.currentUnit;
        }


        public void Repaint() {
            this.drawingSystem.process();
        }


        public List<string> GetStatsFor(AbstractEntity e) {
            return this.GetStatsFor_Sub(e);
        }

        protected abstract List<string> GetStatsFor_Sub(AbstractEntity e);

        public Dictionary<int, AbstractEntity> GetItemSelectionList() {
            return this.menuitemList;
        }
    }
}
