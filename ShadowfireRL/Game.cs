using RLNET;
using ShadowfireRL.components;
using ShadowfireRL.models;
using ShadowfireRL.systems;
using ShadowfireRL.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowfireRL {

    public class Game {
        private BasicEcs ecs;
        private EntityFactory entityFactory;
        private MapData mapData;
        private AbstractEntity currentUnit;
        private DefaultRLView view;
        private DrawingSystem ds;

        public static void Main() {
            new Game();
        }


        private Game() {
            this.ecs = new BasicEcs();
            this.mapData = new MapData();
            this.entityFactory = new EntityFactory(this.ecs, this.mapData);

            this.ecs.systems.Add(new MovementSystem(this.mapData));

            this.CreateGameData();

            this.view = new DefaultRLView(this);
            ds = new DrawingSystem(view, mapData);
            ds.process();
            this.view.Start();

        }


        private void CreateGameData() {
            this.mapData.createMap(this.entityFactory, 50, 50);

            this.currentUnit = this.entityFactory.createPlayersUnit("Syylk", 5, 5);
        }


        private void SingleGameLoop() {
            this.ecs.process();
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
            }

            if (action_performed) {
                this.SingleGameLoop();
            }
            ds.process();
        }


        public void HandleMouseEvent(RLMouse mouse) {

        }



    }

}

