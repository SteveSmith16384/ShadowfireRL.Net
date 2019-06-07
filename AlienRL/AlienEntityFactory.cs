﻿using RLNET;
using RoguelikeFramework;
using RoguelikeFramework.components;
using RoguelikeFramework.models;

namespace AlienRL {

    public class AlienEntityFactory : AbstractEntityFactory {

        public AlienEntityFactory(AbstractRoguelike _game, BasicEcs _ecs, MapData _map_data) : base(_game, _ecs, _map_data) {
        }


        public AbstractEntity createAlien(int x, int y) {
            AbstractEntity e = new AbstractEntity("Alien");
            e.AddComponent(new PositionComponent(e, this.map_data, x, y, true));
            e.AddComponent(new MovementDataComponent());
            e.AddComponent(new GraphicComponent('A', RLColor.Green, RLColor.Black, ' '));
            e.AddComponent(new MobDataComponent(1));
            this.ecs.entities.Add(e);
            return e;
        }


    }

}
