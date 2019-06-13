using AlienRL.components;
using AlienRL.systems;
using RLNET;
using RoguelikeFramework;
using RoguelikeFramework.components;
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

            // Create crew
            Point p = this.GetRandomSq();
            this.currentUnit = this.entityFactory.CreatePlayersUnit('B', "Bret", 1, p.X, p.Y, 100);
            p = this.GetRandomSq();
            this.entityFactory.CreatePlayersUnit('A', "Ash", 1, p.X, p.Y, 100);
            p = this.GetRandomSq();
            this.entityFactory.CreatePlayersUnit('K', "Kane", 1, p.X, p.Y, 100);
            p = this.GetRandomSq();
            this.entityFactory.CreatePlayersUnit('L', "Lambert", 1, p.X, p.Y, 100);
            p = this.GetRandomSq();
            this.entityFactory.CreatePlayersUnit('D', "Dallas", 1, p.X, p.Y, 100);
            p = this.GetRandomSq();
            this.entityFactory.CreatePlayersUnit('R', "Ripley", 1, p.X, p.Y, 100);
            p = this.GetRandomSq();
            this.entityFactory.CreatePlayersUnit('P', "Parker", 1, p.X, p.Y, 100);
            //var gun = this.entityFactory.CreateGunItem();
            //this.entityFactory.AddEntityToUnit(gun, this.currentUnit);

            // Add weapons and equipment
            var gun2 = this.entityFactory.CreateGunItem();
            p = this.GetRandomSq();
            this.entityFactory.AddEntityToMap(gun2, p.X, p.Y);

            // Jones
            p = this.GetRandomSq();
            this.entityFactory.CreateJones(p.X, p.Y);

            // Alien
            p = this.GetRandomSq();
            AlienEntityFactory.CreateAlien(this.ecs, this.mapData, p.X, p.Y);

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


            Point p = this.GetRandomSq();
            this.entityFactory.CreateSelfDestructConsole(p.X, p.Y);
        }


        private Point GetRandomSq() {
            MovementSystem ms = (MovementSystem)this.ecs.GetSystem(nameof(MovementSystem));
            Point p = ms.GetRandomAccessibleSquare();
            return p;
        }


        protected override List<string> GetStatsFor_Sub(AbstractEntity e) {
            var str = new List<string>();
            str.Add($"Stats for {e.name}");
            // todo - stats

            CanCarryComponent ccc = (CanCarryComponent)e.GetComponent(nameof(CanCarryComponent));
            foreach (var item in ccc.GetItems()) {
                str.Add($"{item.name}");
            }
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
