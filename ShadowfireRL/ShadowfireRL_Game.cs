using ShadowfireRL.view;
using System.Collections.Generic;
using ShadowfireRL.systems;
using RoguelikeFramework.systems;
using RoguelikeFramework;
using RoguelikeFramework.view;

namespace ShadowfireRL {

    public class ShadowfireRL_Game : AbstractRoguelike, IDataForView {

        private ShadowfireEntityFactory entityFactory;
        private ShadowfireRLView view;
        private DrawingSystem drawingSystem;

        public static void Main() {
            new ShadowfireRL_Game();
        }


        private ShadowfireRL_Game() : base(ShadowfireRLView._messageHeight) {
            this.entityFactory = new ShadowfireEntityFactory(this, this.ecs, this.mapData);

            this.ecs.systems.Add(new MovementSystem(this.mapData));

            this.CreateGameData();

            this.gameLog.Add("Welcome");

            this.view = new ShadowfireRLView(this);
            this.drawingSystem = new DrawingSystem(this.view, this);
            this.drawingSystem.process();
            this.view.Start();

        }


        private void CreateGameData() {
            this.createMap(this.entityFactory, 50, 50);

            this.currentUnit = this.entityFactory.createPlayersUnit("Syylk", 1, 5, 5);
            this.entityFactory.createPlayersUnit("Manto", 2, 7, 7);
        }


        public void createMap(ShadowfireEntityFactory factory, int w, int h) {
            this.mapData.map = new List<AbstractEntity>[w, h];
            for (int y = 0; y < this.mapData.getHeight(); y++) {
                for (int x = 0; x < this.mapData.getWidth(); x++) {
                    this.mapData.map[x, y] = new List<AbstractEntity>();
                    //if (Misc.random.Next(100) > 10) {
                        factory.createFloorMapSquare(x, y);
                    /*} else {
                        factory.createWallMapSquare(x, y);
                    }*/
                }
            }
        }

        protected override void repaint() { // todo - delete this
            //this.drawingSystem.process();
        }

        public void repaint2() {
            this.drawingSystem.process();
        }

    }

}

