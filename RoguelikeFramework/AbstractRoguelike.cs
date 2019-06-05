using OpenTK;
using RLNET;
using RoguelikeFramework.components;
using RoguelikeFramework.models;
using System;
using System.Collections.Generic;

namespace RoguelikeFramework {
    public abstract class AbstractRoguelike {

        public enum InputMode {Normal, SelectDestination, SelectThrowTarget };

        protected BasicEcs ecs;
        protected MapData mapData;
        protected GameLog gameLog;
        private string hoverText;
        private List<Point> line;
        private InputMode inputMode = InputMode.Normal;

        protected AbstractEntity currentUnit;
        public Dictionary<int, AbstractEntity> playersUnits = new Dictionary<int, AbstractEntity>();

        public AbstractRoguelike(int maxLogEntries) {
            this.ecs = new BasicEcs();
            this.mapData = new MapData();
            this.gameLog = new GameLog(maxLogEntries);
        }


        public void HandleKeyInput(RLKeyPress keyPress) {
            bool action_performed = false;

            switch (keyPress.Key) {
                case RLKey.Keypad1:
                    this.currentUnit = this.playersUnits[1];
                    break;
                case RLKey.Keypad2:
                    this.currentUnit = this.playersUnits[2];
                    break;
                case RLKey.Up: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.getComponent(nameof(MovementDataComponent));
                        m.offY = 1;
                        break;
                    }
                case RLKey.Down: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.getComponent(nameof(MovementDataComponent));
                        m.offY = -1;
                        break;
                    }
                case RLKey.Left: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.getComponent(nameof(MovementDataComponent));
                        m.offX = 1;
                        break;
                    }
                case RLKey.Right: {
                        MovementDataComponent m = (MovementDataComponent)this.currentUnit.getComponent(nameof(MovementDataComponent));
                        m.offX = -1;
                        break;
                    }
                case RLKey.W: {
                        action_performed = true;
                        break;
                    }
            }

            if (action_performed) {
                this.SingleGameLoop();
            }
            this.repaint();
            }


        protected abstract void repaint();


        private void SingleGameLoop() {
            this.ecs.process();
        }


        public void HandleMouseEvent(RLMouse mouse) {
            if (mouse.LeftPressed) {

            } else {
                this.hoverText = "todo";
                this.line = Misc.line(0, 0, mouse.X, mouse.Y);

            }
            this.repaint();
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

    }
}
