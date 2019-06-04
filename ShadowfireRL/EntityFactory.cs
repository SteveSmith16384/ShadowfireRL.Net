using RLNET;
using ShadowfireRL.components;
using ShadowfireRL.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowfireRL
{
    public class EntityFactory
    {
        private BasicEcs ecs;
        private MapData map_data;

        public EntityFactory(BasicEcs _ecs, MapData _map_data)
        {
            this.ecs = _ecs;
            this.map_data = _map_data;
        }


        public AbstractEntity createFloorMapSquare(int x, int y)
        {
            AbstractEntity e = new AbstractEntity("Floor");
            e.addComponent(new PositionComponent(e, this.map_data, x, y, false));
            e.addComponent(new GraphicComponent('.', RLColor.White, RLColor.Black));
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity createWallMapSquare(int x, int y)
        {
            AbstractEntity e = new AbstractEntity("Wall");
            e.addComponent(new PositionComponent(e, this.map_data, x, y, true));
            e.addComponent(new GraphicComponent('#', RLColor.Green, RLColor.Green));
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity createPlayersUnit(String name, int x, int y)
        {
            AbstractEntity e = new AbstractEntity(name);
            e.addComponent(new PositionComponent(e, this.map_data, x, y, true));
            e.addComponent(new MovementDataComponent());
            //e.addComponent(new CanSeeForPlayerComponent());
            e.addComponent(new GraphicComponent('1', RLColor.Green, RLColor.Black));
            //e.addComponent(new CanCarryComponent());
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity createEnemyUnit(int x, int y)
        {
            AbstractEntity e = new AbstractEntity("Enemy");
            e.addComponent(new PositionComponent(e, this.map_data, x, y, true));
            e.addComponent(new MovementDataComponent());
            e.addComponent(new GraphicComponent('E', RLColor.Red, RLColor.Black));
            //e.addComponent(new CanCarryComponent());
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity createGunItem(int x, int y)
        {
            AbstractEntity e = new AbstractEntity("Gun");
            e.addComponent(new PositionComponent(e, this.map_data, x, y, false));
            e.addComponent(new GraphicComponent('L', RLColor.Yellow, RLColor.Black));
            //e.addComponent(new CarryableComponent(1f));
            this.ecs.entities.Add(e);
            return e;
        }


        public AbstractEntity createMedikitItem(int x, int y)
        {
            AbstractEntity e = new AbstractEntity("Medikit");
            e.addComponent(new PositionComponent(e, this.map_data, x, y, false));
            e.addComponent(new GraphicComponent('+', RLColor.Yellow, RLColor.Black));
            //e.addComponent(new CarryableComponent(.3f));
            this.ecs.entities.Add(e);
            return e;
        }


    }

}