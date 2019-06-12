using AlienRL.components;
using RLNET;
using RoguelikeFramework;
using RoguelikeFramework.components;
using RoguelikeFramework.models;

namespace AlienRL {

    public class AlienEntityFactory : AbstractEntityFactory {

        public AlienEntityFactory(AbstractRoguelike _game, BasicEcs _ecs, MapData _map_data) : base(_game, _ecs, _map_data) {
        }


        public AbstractEntity CreateAlien(int x, int y) {
            AbstractEntity e = new AbstractEntity("Alien");
            e.AddComponent(new PositionComponent(e, this.map_data, x, y, true, true));
            e.AddComponent(new MovementDataComponent());
            e.AddComponent(new GraphicComponent('A', RLColor.Green, RLColor.Black, ' ', 10));
            e.AddComponent(new AlienComponent());
            e.AddComponent(new MobDataComponent(1, 150));
            e.AddComponent(new AttackAttackableComponent(30, 30));
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity CreateJones(int x, int y) {
            AbstractEntity e = new AbstractEntity("Jones");
            e.AddComponent(new PositionComponent(e, this.map_data, x, y, false, true));
            e.AddComponent(new MovementDataComponent());
            e.AddComponent(new GraphicComponent('J', RLColor.Brown, RLColor.Black, ' ', 10));
            e.AddComponent(new JonesTheCatComponent());
            e.AddComponent(new CarryableComponent(2));            
            e.AddComponent(new MobDataComponent(1, 175));
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity CreateSelfDestructConsole(int x, int y) {
            AbstractEntity e = new AbstractEntity("SelfDestruct");
            e.AddComponent(new PositionComponent(e, this.map_data, x, y, true, true));
            e.AddComponent(new GraphicComponent('D', RLColor.White, RLColor.Red, 'D', 0));
            e.AddComponent(new MapsquareData(true, false));
            e.AddComponent(new ComputerConsoleComponent());
            
            this.ecs.entities.Add(e);
            return e;
        }


    }

}
