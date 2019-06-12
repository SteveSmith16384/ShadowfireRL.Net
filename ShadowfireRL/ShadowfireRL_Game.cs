using System.Collections.Generic;
using System.Drawing;
using RLNET;
using RoguelikeFramework;
using RoguelikeFramework.view;
using RogueLikeMapBuilder;
using ShadowfireRL.systems;

namespace ShadowfireRL {

    public class ShadowfireRL_Game : AbstractRoguelike, IDataForView, IDebugSettings {

        private ShadowfireEntityFactory entityFactory;

        public static void Main() {
            new ShadowfireRL_Game();
        }


        private ShadowfireRL_Game() : base(DefaultRLView._messageHeight) {
            new EnemyAISystem(this.ecs);
            this.gameLog.Add("Welcome");
            this.SelectUnit(1);

            this.view.Start();
        }


        protected override void CreateData() {
            this.entityFactory = new ShadowfireEntityFactory(this, this.ecs, this.mapData);

            int mapWidth = 75;
            int mapHeight = 65;

            csMapbuilder builder = new csMapbuilder(mapWidth, mapHeight, 16, new Size(5, 5), new Size(15, 15), 3, 5);

            this.createMap(builder, mapWidth, mapHeight);

            int unitStartX = mapWidth / 2;
            int unitStartY = mapHeight / 2;


            this.currentUnit = this.entityFactory.CreatePlayersUnit('S', "Syylk", 1, unitStartX, unitStartY, 100);
            var gun = this.entityFactory.CreateGunItem();
            this.entityFactory.AddEntityToUnit(gun, this.currentUnit);

            this.entityFactory.CreatePlayersUnit('M', "Manto", 2, unitStartX + 1, unitStartY + 1, 100);

            var gun2 = this.entityFactory.CreateGunItem();
            this.entityFactory.AddEntityToMap(gun2, unitStartX, unitStartY + 1);

            var grenade = this.entityFactory.CreateGrenadeItem();
            this.entityFactory.AddEntityToMap(grenade, unitStartX + 3, unitStartY + 1);

            for (int i = 1; i < builder.rctBuiltRooms.Count; i++) {
                Rectangle r = builder.rctBuiltRooms[i];
                this.entityFactory.createEnemyUnit($"Enemy {i}", r.X, r.Y, 90);
            }
        }


        public void createMap(csMapbuilder builder, int w, int h) {
            //while (true) {
            if (builder.Build_OneStartRoom()) {
                //break;
            }
            //}

            this.mapData.map = new List<AbstractEntity>[w, h];
            for (int y = 0; y < this.mapData.getHeight(); y++) {
                for (int x = 0; x < this.mapData.getWidth(); x++) {
                    this.mapData.map[x, y] = new List<AbstractEntity>();
                    if (builder.map[x, y] == 1) {
                        this.entityFactory.CreateWallMapSquare(x, y, RLColor.Cyan);
                    } else if (builder.map[x, y] == 2) {
                        this.entityFactory.CreateDoorMapSquare(x, y, RLColor.Yellow);
                    } else {
                        this.entityFactory.CreateFloorMapSquare(x, y);
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


        public override bool drawEverything() {
            return ShadowfireSettings.DEBUG_DRAW_ALL;
        }

    }

}
