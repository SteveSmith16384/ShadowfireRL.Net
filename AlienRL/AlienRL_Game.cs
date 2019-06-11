using AlienRL.systems;
using RoguelikeFramework;
using RoguelikeFramework.view;
using RogueLikeMapBuilder;
using System.Collections.Generic;
using System.Drawing;

namespace AlienRL {


    public class AlienRL_Game : AbstractRoguelike, IDataForView, IDebugSettings {

        private AlienEntityFactory entityFactory;

        public static void Main() {
            new AlienRL_Game();
        }


        private AlienRL_Game() : base(DefaultRLView._messageHeight) {
            this.ecs.systems.Add(new AlienAISystem(this.checkVisibilitySystem, this.ecs.entities));

            this.gameLog.Add("USS Nostromo");

            this.SelectUnit(1);

            this.view.Start();
        }


        protected override void CreateData() {
            this.entityFactory = new AlienEntityFactory(this, this.ecs, this.mapData);

            int mapWidth = 75;
            int mapHeight = 65;

            csMapbuilder builder = new csMapbuilder(mapWidth, mapHeight, 10, new Size(7, 7), new Size(11, 11), 2, 5);

            this.createMap(builder, mapWidth, mapHeight);

            Rectangle startRoom = builder.rctBuiltRooms[0];
            int unitStartX = startRoom.X;
            int unitStartY = startRoom.Y;

            this.currentUnit = this.entityFactory.CreatePlayersUnit("Dallas", 1, unitStartX, unitStartY, 100);
            var gun = this.entityFactory.CreateGunItem();
            this.entityFactory.AddEntityToUnit(gun, this.currentUnit);

            this.entityFactory.CreatePlayersUnit("Ash", 2, unitStartX + 1, unitStartY + 1, 100);

            var gun2 = this.entityFactory.CreateGunItem();
            this.entityFactory.AddEntityToMap(gun2, unitStartX, unitStartY + 1);

            this.entityFactory.CreateAlien(builder.rctBuiltRooms[1].X, builder.rctBuiltRooms[1].Y);

            this.entityFactory.CreateJones(builder.rctBuiltRooms[2].X, builder.rctBuiltRooms[2].Y);
        }


        public void createMap(csMapbuilder builder, int w, int h) {
            //csMapbuilder builder = null;
            //while (true) {
            //builder = new csMapbuilder(w, h);
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


        public override bool drawEverything() {
            return AlienSettings.DEBUG_DRAW_ALL;
        }

    }

}
