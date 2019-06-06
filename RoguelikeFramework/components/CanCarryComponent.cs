﻿using System.Collections.Generic;

namespace RoguelikeFramework.components {

    public class CanCarryComponent : AbstractComponent {

        public List<AbstractEntity> items = new List<AbstractEntity>();
        public AbstractEntity CurrentItem;
        public float maxWeight;

        public CanCarryComponent(float max) {
            this.maxWeight = max;
        }

    }

}
