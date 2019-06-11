using SimpleEcs;
using System;
using System.Collections.Generic;
using System.Linq;

public class BasicEcs {

    //public List<AbstractSystem> systems = new List<AbstractSystem>();
    private Dictionary<string, AbstractSystem> systems = new Dictionary<string, AbstractSystem>();
    public List<AbstractEntity> entities = new List<AbstractEntity>(); // todo - use dictionary with entity id
    private IEcsEventListener eventListener;

    public BasicEcs(IEcsEventListener _eventListener) {
        this.eventListener = _eventListener;

    }


    public void AddSystem(AbstractSystem system) {
        this.systems.Add(system.GetType().Name, system);
    }


    
    /*public AbstractSystem GetSystem(Type cl) {
            //MovementSystem ms = (MovementSystem)this.ecs.systems.Single(x => x.GetType() == typeof(MovementSystem));
            return this.systems.Single(x => x.GetType() == cl);
        }*/


    public AbstractSystem GetSystem(string name) {
        //if (this.systems.ContainsKey(name)) {
            return this.systems[name];
        /*} else {
            return null;
        }*/
    }





    public void process() {
        // Remove any entities
        foreach(AbstractEntity entity in this.entities) {
            if (entity.markForRemoval) {
                this.entities.Remove(entity);
                this.eventListener.EntityRemoved(entity);
            }
        }

		foreach(AbstractSystem system in this.systems.Values) {
            if (system.runEachLoop) {
                system.Process(this.entities);
            }
		}
	}

}
