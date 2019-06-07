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
            e.AddComponent(new PositionComponent(e, this.map_data, x, y, false));
            e.AddComponent(new GraphicComponent('.', RLColor.White, RLColor.Black, ' '));
            e.AddComponent(new MapsquareData(false, false));
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity createWallMapSquare(int x, int y) {
            AbstractEntity e = new AbstractEntity("Wall");
            e.AddComponent(new PositionComponent(e, this.map_data, x, y, true));
            e.AddComponent(new GraphicComponent('#', RLColor.Green, RLColor.Green, '#'));
            e.AddComponent(new MapsquareData(true, false));
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity createDoorMapSquare(int x, int y) {
            AbstractEntity e = new AbstractEntity("Wall");
            e.AddComponent(new PositionComponent(e, this.map_data, x, y, false));
            e.AddComponent(new GraphicComponent('D', RLColor.Black, RLColor.Green, 'D'));
            e.AddComponent(new MapsquareData(true, false));
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity CreatePlayersUnit(String name, int num, int x, int y) {
            AbstractEntity e = new AbstractEntity(name);
            e.AddComponent(new PositionComponent(e, this.map_data, x, y, true));
            e.AddComponent(new MovementDataComponent());
            e.AddComponent(new GraphicComponent('1', RLColor.Green, RLColor.Black, ' '));
            e.AddComponent(new CanCarryComponent(10));
            e.AddComponent(new PlayersUnitData(num));
            e.AddComponent(new ShootOnSightComponent());
            e.AddComponent(new MobDataComponent(0));

            this.ecs.entities.Add(e);

            this.game.playersUnits.Add(num, e);

            return e;
        }


        public AbstractEntity createEnemyUnit(string name, int x, int y) {
            AbstractEntity e = new AbstractEntity(name);
            e.AddComponent(new PositionComponent(e, this.map_data, x, y, true));
            e.AddComponent(new MovementDataComponent());
            e.AddComponent(new GraphicComponent('E', RLColor.Red, RLColor.Black, ' '));
            e.AddComponent(new CanCarryComponent(99));
            e.AddComponent(new ShootOnSightComponent());
            e.AddComponent(new MobDataComponent(1));
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity AddEntityToMap(AbstractEntity e, int x, int y) {
            e.AddComponent(new PositionComponent(e, this.map_data, x, y, false));
            return e;
        }


        public AbstractEntity AddEntityToUnit(AbstractEntity e, AbstractEntity unit) {
            CanCarryComponent c = (CanCarryComponent)unit.getComponent(nameof(CanCarryComponent));
            c.AddItem(e);
            return e;
        }


        public AbstractEntity createGunItem() {
            AbstractEntity gun = new AbstractEntity("Gun");
            gun.AddComponent(new GraphicComponent('L', RLColor.Yellow, RLColor.Black, ' '));
            gun.AddComponent(new CarryableComponent(1f));
            gun.AddComponent(new ItemCanShootComponent(99f, 10f));
            this.ecs.entities.Add(gun);
            return gun;
        }


        public AbstractEntity createGrenadeItem() {
            AbstractEntity grenade = new AbstractEntity("Grenade");
            grenade.AddComponent(new GraphicComponent('o', RLColor.Yellow, RLColor.Black, ' '));
            grenade.AddComponent(new CarryableComponent(1f));
            grenade.AddComponent(new TimerCanBeSetComponent());
            grenade.AddComponent(new ExplodesWhenTimerExpiresComponent(5, 10));
            this.ecs.entities.Add(grenade);
            return grenade;
        }


        public AbstractEntity createMedikitItem(int x, int y) {
            AbstractEntity e = new AbstractEntity("Medikit");
            e.AddComponent(new PositionComponent(e, this.map_data, x, y, false));
            e.AddComponent(new GraphicComponent('m', RLColor.Yellow, RLColor.Black, ' '));
            e.AddComponent(new CarryableComponent(.3f));
            e.AddComponent(new UseableComponent());
            this.ecs.entities.Add(e);
            return e;
        }


    }

}