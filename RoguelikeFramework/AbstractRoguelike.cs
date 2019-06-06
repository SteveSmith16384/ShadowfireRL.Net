using RLNET;
using RoguelikeFramework.components;
using RoguelikeFramework.models;
using RoguelikeFramework.systems;
using RoguelikeFramework.view;
using System.Collections.Generic;
using System.Text;

namespace RoguelikeFramework {

    public abstract class AbstractRoguelike : IDataForView {

        public enum InputMode { Normal, SelectItemFromInv, SelectDestination, SelectThrowTarget };

        protected BasicEcs ecs;
        protected MapData mapData;
        protected GameLog gameLog;
        private string hoverText;
        private List<Point> line;
        private InputMode currentInputMode = InputMode.Normal;
        protected DefaultRLView view;

        private DrawingSystem drawingSystem;
        private CheckMapVisibilitySystem checkVisibilitySystem;

        protected AbstractEntity currentUnit;
        public Dictionary<int, AbstractEntity> playersUnits = new Dictionary<int, AbstractEntity>();

        public AbstractRoguelike(int maxLogEntries) {
            this.ecs = new BasicEcs();
            this.mapData = new MapData();
            this.gameLog = new GameLog(maxLogEntries);

            this.CreateData();

            this.ecs.systems.Add(new MovementSystem(this.mapData));

            this.view = new DefaultRLView(this);
            this.drawingSystem = new DrawingSystem(this.view, this);
            this.drawingSystem.process();

            this.checkVisibilitySystem = new CheckMapVisibilitySystem(this.mapData);
            this.ecs.systems.Add(new ShootOnSightSystem(this.checkVisibilitySystem, this.ecs.entities));

            this.checkVisibilitySystem.process(this.playersUnits.Values);
        }


        protected abstract void CreateData();


        public void HandleKeyInput(RLKeyPress keyPress) {
            bool action_performed = false;
            bool unit_moved = false; // Do we need to recalc visible squares?

            switch (keyPress.Key) {

                case RLKey.Number1:
                    this.SelectUnit(1);
                    break;
                case RLKey.Number2:
                    this.SelectUnit(2);
                    break;
                case RLKey.Number3:
                    this.SelectUnit(3);
                    break;
                case RLKey.Number4:
                    this.SelectUnit(4);
                    break;
                case RLKey.Number5:
                    this.SelectUnit(5);
                    break;
                case RLKey.Number6:
                    this.SelectUnit(6);
                    break;

                case RLKey.Up: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.getComponent(nameof(MovementDataComponent));
                        m.offY = -1;
                        action_performed = true;
                        unit_moved = true;
                        break;
                    }
                case RLKey.Down: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.getComponent(nameof(MovementDataComponent));
                        m.offY = 1;
                        action_performed = true;
                        unit_moved = true;
                        break;
                    }
                case RLKey.Left: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.getComponent(nameof(MovementDataComponent));
                        m.offX = -1;
                        action_performed = true;
                        unit_moved = true;
                        break;
                    }
                case RLKey.Right: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.getComponent(nameof(MovementDataComponent));
                        m.offX = 1;
                        action_performed = true;
                        unit_moved = true;
                        break;
                    }

                case RLKey.W: {
                        action_performed = true;
                        unit_moved = true;
                        break;
                    }
            }

            if (action_performed) {
                this.SingleGameLoop();
            }
            if (unit_moved) {
                this.checkVisibilitySystem.process(this.playersUnits.Values);
            }
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
                this.hoverText = this.getSquareDesc(mouse.X, mouse.Y);
                if (this.currentInputMode == InputMode.SelectDestination) {
                    this.line = Misc.line(0, 0, mouse.X, mouse.Y);
                } else {
                    this.line = null;
                }
            }
        }


        private string getSquareDesc(int x, int y) {
            if (x < this.mapData.map.GetLength(0) && y < this.mapData.map.GetLength(1)) {
                var sb = new StringBuilder();
                var ents = this.mapData.map[x, y];
                //return string.Join(", ", ents);
                foreach (var e in ents) {
                    sb.Append(e.name + ";");
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


        public void repaint2() {
            this.drawingSystem.process();
        }


        public List<string> GetStatsFor(AbstractEntity e) {
            return this.GetStatsFor_Sub(e);
        }

        protected abstract List<string> GetStatsFor_Sub(AbstractEntity e);
    }
}
