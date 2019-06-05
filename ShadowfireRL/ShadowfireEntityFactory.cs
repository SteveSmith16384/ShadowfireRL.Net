using RLNET;
using System;
using RoguelikeFramework.models;
using RoguelikeFramework.components;

namespace ShadowfireRL {
    public class ShadowfireEntityFactory {

        private BasicEcs ecs;
        private MapData map_data;
        private ShadowfireRL_Game game;

        public ShadowfireEntityFactory(ShadowfireRL_Game _game, BasicEcs _ecs, MapData _map_data) {
            this.game = _game;
            this.ecs = _ecs;
            this.map_data = _map_data;
        }


        public AbstractEntity createFloorMapSquare(int x, int y) {
            AbstractEntity e = new AbstractEntity("Floor");
            e.addComponent(new PositionComponent(e, this.map_data, x, y, false));
            e.addComponent(new GraphicComponent('.', RLColor.White, RLColor.Black));
            e.addComponent(new MapsquareData(false));
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity createWallMapSquare(int x, int y) {
            AbstractEntity e = new AbstractEntity("Wall");
            e.addComponent(new PositionComponent(e, this.map_data, x, y, true));
            e.addComponent(new GraphicComponent('#', RLColor.Green, RLColor.Green));
            e.addComponent(new MapsquareData(true));
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity createPlayersUnit(String name, int num, int x, int y) {
            AbstractEntity e = new AbstractEntity(name);
            e.addComponent(new PositionComponent(e, this.map_data, x, y, true));
            e.addComponent(new MovementDataComponent());
            //e.addComponent(new CanSeeForPlayerComponent());
            e.addComponent(new GraphicComponent('1', RLColor.Green, RLColor.Black));
            e.addComponent(new CanCarryComponent(10));
            e.addComponent(new PlayersUnitData(num));

            this.ecs.entities.Add(e);

            this.game.playersUnits.Add(num, e);

            return e;
        }


        public AbstractEntity createEnemyUnit(int x, int y) {
            AbstractEntity e = new AbstractEntity("Enemy");
            e.addComponent(new PositionComponent(e, this.map_data, x, y, true));
            e.addComponent(new MovementDataComponent());
            e.addComponent(new GraphicComponent('E', RLColor.Red, RLColor.Black));
            e.addComponent(new CanCarryComponent(99));
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity createGunItem(int x, int y) {
            AbstractEntity e = new AbstractEntity("Gun");
            e.addComponent(new PositionComponent(e, this.map_data, x, y, false));
            e.addComponent(new GraphicComponent('L', RLColor.Yellow, RLColor.Black));
            e.addComponent(new CarryableComponent(1f));
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity createMedikitItem(int x, int y) {
            AbstractEntity e = new AbstractEntity("Medikit");
            e.addComponent(new PositionComponent(e, this.map_data, x, y, false));
            e.addComponent(new GraphicComponent('+', RLColor.Yellow, RLColor.Black));
            e.addComponent(new CarryableComponent(.3f));
            this.ecs.entities.Add(e);
            return e;
        }


    }

}