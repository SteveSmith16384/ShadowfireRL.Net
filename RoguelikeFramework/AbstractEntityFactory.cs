using RLNET;
using RoguelikeFramework.components;
using RoguelikeFramework.models;
using System;

namespace RoguelikeFramework {

    public abstract class AbstractEntityFactory {

        protected AbstractRoguelike game;
        protected BasicEcs ecs;
        protected MapData map_data;

        public AbstractEntityFactory(AbstractRoguelike _game, BasicEcs _ecs, MapData _map_data) {
            this.game = _game;
            this.ecs = _ecs;
            this.map_data = _map_data;
        }


        public AbstractEntity createFloorMapSquare(int x, int y) {
            AbstractEntity e = new AbstractEntity("Floor");
            e.AddComponent(new PositionComponent(e, this.map_data, x, y, false, true));
            e.AddComponent(new GraphicComponent('.', RLColor.White, RLColor.Black, ' ', 0));
            e.AddComponent(new MapsquareData(false, false));
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity createWallMapSquare(int x, int y) {
            AbstractEntity e = new AbstractEntity("Wall");
            e.AddComponent(new PositionComponent(e, this.map_data, x, y, true, true));
            e.AddComponent(new GraphicComponent('#', RLColor.Green, RLColor.Green, '#', 0));
            e.AddComponent(new MapsquareData(true, false));
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity createDoorMapSquare(int x, int y) {
            AbstractEntity e = new AbstractEntity("Wall");
            e.AddComponent(new PositionComponent(e, this.map_data, x, y, false, true));
            e.AddComponent(new GraphicComponent('D', RLColor.Black, RLColor.Green, 'D', 0));
            e.AddComponent(new MapsquareData(true, false));
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity CreatePlayersUnit(char ch, String name, int num, int x, int y, int aps) {
            AbstractEntity e = new AbstractEntity(name);
            e.AddComponent(new PositionComponent(e, this.map_data, x, y, true, true));
            e.AddComponent(new MovementDataComponent());
            e.AddComponent(new GraphicComponent(ch, RLColor.Green, RLColor.Black, ' ', 10));
            e.AddComponent(new CanCarryComponent(10));
            e.AddComponent(new PlayersUnitData(num));
            //e.AddComponent(new ShootOnSightComponent());
            e.AddComponent(new MobDataComponent(0, aps));
            e.AddComponent(new AttackAttackableComponent(10, 5));

            this.ecs.entities.Add(e);

            this.game.playersUnits.Add(e);

            return e;
        }


        public AbstractEntity AddEntityToMap(AbstractEntity e, int x, int y) {
            e.AddComponent(new PositionComponent(e, this.map_data, x, y, false, true));
            return e;
        }


        public AbstractEntity AddEntityToUnit(AbstractEntity e, AbstractEntity unit) {
            CanCarryComponent c = (CanCarryComponent)unit.GetComponent(nameof(CanCarryComponent));
            e.AddComponent(new PositionComponent(e, this.map_data, 0, 0, false, false));
            c.AddItem(e);
            return e;
        }


        public AbstractEntity CreateGunItem() {
            AbstractEntity gun = new AbstractEntity("Gun");
            gun.AddComponent(new GraphicComponent('L', RLColor.Yellow, RLColor.Black, ' ', 5));
            gun.AddComponent(new CarryableComponent(1f));
            gun.AddComponent(new ItemCanShootComponent(99f, 10f));
            //gun.AddComponent(new UseableComponent());
            
            this.ecs.entities.Add(gun);
            return gun;
        }


        public AbstractEntity CreateGrenadeItem() {
            AbstractEntity grenade = new AbstractEntity("Grenade");
            grenade.AddComponent(new GraphicComponent('o', RLColor.Yellow, RLColor.Black, ' ', 5));
            grenade.AddComponent(new CarryableComponent(1f));
            grenade.AddComponent(new TimerCanBeSetComponent());
            grenade.AddComponent(new ExplodesWhenTimerExpiresComponent(5, 10));
            this.ecs.entities.Add(grenade);
            return grenade;
        }


        public AbstractEntity CreateMedikitItem(int x, int y) {
            AbstractEntity e = new AbstractEntity("Medikit");
            e.AddComponent(new GraphicComponent('m', RLColor.Yellow, RLColor.Black, ' ', 5));
            e.AddComponent(new CarryableComponent(.3f));
            e.AddComponent(new UseableComponent());
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity CreateKnifeItem(int x, int y) {
            AbstractEntity e = new AbstractEntity("Knife");
            e.AddComponent(new GraphicComponent('k', RLColor.Yellow, RLColor.Black, ' ', 5));
            e.AddComponent(new CarryableComponent(.2f));
            e.AddComponent(new UseableComponent());
            e.AddComponent(new ItemIsWeaponComponent(5));
            this.ecs.entities.Add(e);
            return e;
        }

    }

}
