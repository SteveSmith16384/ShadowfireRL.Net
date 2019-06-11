using RLNET;
using RoguelikeFramework.models;
using RoguelikeFramework.components;
using RoguelikeFramework;

namespace ShadowfireRL {

    public class ShadowfireEntityFactory : AbstractEntityFactory {

        public ShadowfireEntityFactory(AbstractRoguelike _game, BasicEcs _ecs, MapData _map_data) : base (_game, _ecs, _map_data) {
        }


        public AbstractEntity createEnemyUnit(string name, int x, int y, int aps) {
            AbstractEntity e = new AbstractEntity(name);
            e.AddComponent(new PositionComponent(e, this.map_data, x, y, true, true));
            e.AddComponent(new MovementDataComponent());
            e.AddComponent(new GraphicComponent('E', RLColor.Red, RLColor.Black, ' ', 10));
            e.AddComponent(new CanCarryComponent(99));
            e.AddComponent(new ShootOnSightComponent());
            e.AddComponent(new MobDataComponent(1, aps));
            e.AddComponent(new AttackAttackableComponent(6, 5));
            this.ecs.entities.Add(e);
            return e;
        }

    }

}
