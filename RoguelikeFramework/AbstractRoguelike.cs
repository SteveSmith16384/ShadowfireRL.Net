using RLNET;
using RoguelikeFramework.components;
using RoguelikeFramework.models;
using System.Collections.Generic;

namespace RoguelikeFramework {
    public abstract class AbstractRoguelike {

        protected BasicEcs ecs;
        protected MapData mapData;
        protected GameLog gameLog;
        private string hoverText;

        protected AbstractEntity currentUnit;

        public AbstractRoguelike() {
            this.ecs = new BasicEcs();
            this.mapData = new MapData();
            this.gameLog = new GameLog();

        }


        public void HandleKeyInput(RLKeyPress keyPress) {
            bool action_performed = false;

            switch (keyPress.Key) {
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
            }
            //this.drawingSystem.process();
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

    }
}
