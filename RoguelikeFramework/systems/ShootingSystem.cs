using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.systems {

    public class ShootingSystem {
        // todo

        public ShootingSystem() {

        }


        public void EntityShotByEntity(AbstractEntity shooter, AbstractEntity target) {
            Console.WriteLine($"Target {target.name} shot by {shooter.name}");
            // todo - add bullet effect
            // todo - calc acc, damage etc...

        }
    }

}
