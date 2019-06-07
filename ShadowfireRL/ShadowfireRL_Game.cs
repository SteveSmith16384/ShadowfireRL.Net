using System.Collections.Generic;
using RoguelikeFramework;
using RoguelikeFramework.view;
using RogueLikeMapBuilder;
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

            this.currentUnit = this.entityFactory.CreatePlayersUnit("Syylk", 1, 23, 23);
            this.entityFactory.createGunItemForUnit(this.currentUnit);

            this.entityFactory.CreatePlayersUnit("Manto", 2, 25, 25);
            this.entityFactory.createGunItemForMap(8, 8);

            this.entityFactory.createEnemyUnit("Zoff", 20, 20);
        }


        public void createMap(ShadowfireEntityFactory factory, int w, int h) {
            csMapbuilder builder = new csMapbuilder(w, h);
            builder.Build_OneStartRoom();

            this.mapData.map = new List<AbstractEntity>[w, h];
            for (int y = 0; y < this.mapData.getHeight(); y++) {
                for (int x = 0; x < this.mapData.getWidth(); x++) {
                    this.mapData.map[x, y] = new List<AbstractEntity>();
                    if (builder.map[x, y] == 1) {
                        factory.createWallMapSquare(x, y);
                    } else if (Misc.random.Next(100) > 80) {
                        factory.createDoorMapSquare(x, y);
                    } else {
                        factory.createFloorMapSquare(x, y);
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
