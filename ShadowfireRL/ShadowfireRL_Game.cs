using System.Collections.Generic;
using RoguelikeFramework;
using RoguelikeFramework.view;
using ShadowfireRL.systems;

namespace ShadowfireRL {

    public class ShadowfireRL_Game : AbstractRoguelike, IDataForView {

        private ShadowfireEntityFactory entityFactory;

        public static void Main() {
            new ShadowfireRL_Game();
        }


        private ShadowfireRL_Game() : base(DefaultRLView._messageHeight) {
            this.ecs.systems.Add(new UnitAISystem());
            this.gameLog.Add("Welcome");
            this.SelectUnit(1);

            this.view.Start();
        }


        protected override void CreateData() {
            this.entityFactory = new ShadowfireEntityFactory(this, this.ecs, this.mapData);

            this.createMap(this.entityFactory, 50, 50);

            this.currentUnit = this.entityFactory.createPlayersUnit("Syylk", 1, 5, 5);
            this.entityFactory.createPlayersUnit("Manto", 2, 7, 7);
        }


        public void createMap(ShadowfireEntityFactory factory, int w, int h) {
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


        protected override List<string> GetStatsFor_Sub(AbstractEntity e) {
            var str = new List<string>();
            str.Add($"Stats for {e.name}");
            return str;
        }

    }

}
