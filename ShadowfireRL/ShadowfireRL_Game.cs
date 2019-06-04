using ShadowfireRL.view;
using System.Collections.Generic;
using RoguelikeFramework.view;
using RoguelikeFramework.systems;
using ShadowfireRL.systems;
using RoguelikeFramework;

namespace ShadowfireRL {

    public class ShadowfireRL_Game : AbstractRoguelike, IDataForView {

        private EntityFactory entityFactory;
        private DefaultRLView view;
        private DrawingSystem drawingSystem;

        public static void Main() {
            new ShadowfireRL_Game();
        }


        private ShadowfireRL_Game() {
            this.entityFactory = new EntityFactory(this.ecs, this.mapData);

            this.ecs.systems.Add(new MovementSystem(this.mapData));

            this.CreateGameData();

            this.view = new DefaultRLView(this);
            this.drawingSystem = new DrawingSystem(this.view, this);
            this.drawingSystem.process();
            this.view.Start();

        }


        private void CreateGameData() {
            this.createMap(this.entityFactory, 50, 50);

            this.currentUnit = this.entityFactory.createPlayersUnit("Syylk", 5, 5);
        }


        public void createMap(EntityFactory factory, int w, int h) {
            this.mapData.map = new List<AbstractEntity>[w, h];
            for (int y = 0; y < this.mapData.getHeight(); y++) {
                for (int x = 0; x < this.mapData.getWidth(); x++) {
                    this.mapData.map[x, y] = new List<AbstractEntity>();
                    if (Misc.random.Next(100) > 10) {
                        factory.createFloorMapSquare(x, y);
                    } else {
                        factory.createWallMapSquare(x, y);
                    }
                }
            }
        }

        protected override void repaint() {
            this.drawingSystem.process();
        }
    }

}

