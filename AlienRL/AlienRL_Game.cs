using AlienRL.components;
using AlienRL.systems;
using RLNET;
using RoguelikeFramework;
using RoguelikeFramework.systems;
using RoguelikeFramework.view;
using RogueLikeMapBuilder;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace AlienRL {


    public class AlienRL_Game : AbstractRoguelike, IDataForView, IDebugSettings {

        private AlienEntityFactory entityFactory;

        public static void Main() {
            new AlienRL_Game();
        }


        private AlienRL_Game() : base(DefaultRLView._messageHeight) {
            new AlienAISystem(this.ecs, this.checkVisibilitySystem, this.ecs.entities);
            new JonesTheCatSystem(this.ecs);

            this.gameLog.Add("USS Nostromo");

            this.SelectUnit(1);

            this.view.Start();
        }


        protected override void CreateData() {
            this.entityFactory = new AlienEntityFactory(this, this.ecs, this.mapData);

            int mapWidth = 75;
            int mapHeight = 65;

            csMapbuilder builder = new csMapbuilder(mapWidth, mapHeight, 10, new Size(7, 7), new Size(11, 11), 2, 5);

            this.CreateMap(builder, mapWidth, mapHeight);

            Rectangle startRoom = builder.rctBuiltRooms[0];
            int unitStartX = startRoom.X;
            int unitStartY = startRoom.Y;

            // Create crew
            this.currentUnit = this.entityFactory.CreatePlayersUnit('D', "Dallas", 1, unitStartX, unitStartY, 100);
            var gun = this.entityFactory.CreateGunItem();
            this.entityFactory.AddEntityToUnit(gun, this.currentUnit);

            this.entityFactory.CreatePlayersUnit('A', "Ash", 2, unitStartX + 1, unitStartY + 1, 100);

            var gun2 = this.entityFactory.CreateGunItem();
            this.entityFactory.AddEntityToMap(gun2, unitStartX, unitStartY + 1);

            // Jones
            this.entityFactory.CreateJones(builder.rctBuiltRooms[2].X, builder.rctBuiltRooms[2].Y);

            // Alien
            this.entityFactory.CreateAlien(builder.rctBuiltRooms[1].X, builder.rctBuiltRooms[1].Y);

        }


        public void CreateMap(csMapbuilder builder, int w, int h) {
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
                        this.entityFactory.CreateWallMapSquare(x, y, RLColor.Green);
                    } else if (builder.map[x, y] == 2) {
                        this.entityFactory.CreateDoorMapSquare(x, y, RLColor.Brown);
                    } else {
                        this.entityFactory.CreateFloorMapSquare(x, y);
                    }
                }
            }


            MovementSystem ms = (MovementSystem)this.ecs.GetSystem(nameof(MovementSystem));
            Point p = ms.GetRandomAccessibleSquare();
            this.entityFactory.CreateSelfDestructConsole(p.X, p.Y);
        }


        protected override List<string> GetStatsFor_Sub(AbstractEntity e) {
            var str = new List<string>();
            str.Add($"Stats for {e.name}");
            // todo
            return str;
        }


        protected override void MouseClicked(int x, int y) {
            base.MouseClicked(x, y);
            var sdc = this.mapData.GetSingleComponent(x, y, nameof(SelfDestructConsole));
            if (sdc != null) {
                Console.WriteLine("Self Destruct Activated!");
            }

            var lepc = this.mapData.GetSingleComponent(x, y, nameof(LaunchEscapePodComponent));
            if (lepc != null) {
                Console.WriteLine("Escape Pod Launched!");
            }
        }

    }

}
