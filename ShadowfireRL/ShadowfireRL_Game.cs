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

            int mapWidth = 50;
            int mapHeight = 50;

            this.createMap(mapWidth, mapHeight);

            int unitStartX = mapWidth / 2;
            int unitStartY = mapHeight / 2;


            this.currentUnit = this.entityFactory.CreatePlayersUnit("Syylk", 1, unitStartX, unitStartY);
            var gun = this.entityFactory.createGunItem();
            this.entityFactory.AddEntityToUnit(gun, this.currentUnit);

            this.entityFactory.CreatePlayersUnit("Manto", 2, unitStartX + 1, unitStartY + 1);

            var gun2 = this.entityFactory.createGunItem();
            this.entityFactory.AddEntityToMap(gun2, unitStartX, unitStartY + 1);

            var grenade = this.entityFactory.createGrenadeItem();
            this.entityFactory.AddEntityToMap(grenade, unitStartX + 3, unitStartY + 1);

            this.entityFactory.createEnemyUnit("Zoff", 20, 20); // todo
        }


        public void createMap(int w, int h) {
            csMapbuilder builder = null;
            //while (true) {
            builder = new csMapbuilder(w, h);
            if (builder.Build_OneStartRoom()) {
                //break;
            }
            //}

            this.mapData.map = new List<AbstractEntity>[w, h];
            for (int y = 0; y < this.mapData.getHeight(); y++) {
                for (int x = 0; x < this.mapData.getWidth(); x++) {
                    this.mapData.map[x, y] = new List<AbstractEntity>();
                    if (builder.map[x, y] == 1) {
                        this.entityFactory.createWallMapSquare(x, y);
                    } else if (builder.map[x, y] == 2) {
                        this.entityFactory.createDoorMapSquare(x, y);
                    } else {
                        this.entityFactory.createFloorMapSquare(x, y);
                    }
                }
            }
        }


        protected override List<string> GetStatsFor_Sub(AbstractEntity e) {
            var str = new List<string>();
            str.Add($"Stats for {e.name}");
            // todo
            return str;
        }

    }

}
