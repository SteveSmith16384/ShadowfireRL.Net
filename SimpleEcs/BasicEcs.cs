using SimpleEcs;
using System;
using System.Collections.Generic;
using System.Linq;

public class BasicEcs {

    private Dictionary<string, AbstractSystem> systems = new Dictionary<string, AbstractSystem>();
    public List<AbstractEntity> entities = new List<AbstractEntity>(); // todo - use dictionary with entity id
    private IEcsEventListener eventListener;

    public BasicEcs(IEcsEventListener _eventListener) {
        this.eventListener = _eventListener;

    }


    public void AddSystem(AbstractSystem system) {
        this.systems.Add(system.GetType().Name, system);
    }


    
    public AbstractSystem GetSystem(string name) {
            return this.systems[name];
    }

    public void process() {
        // Remove any entities
        //foreach(AbstractEntity entity in this.entities) {
        for (int i = this.entities.Count - 1; i >= 0; i--) {
            var entity = this.entities[i];
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
