using SimpleEcs;
using System.Collections.Generic;

public class BasicEcs {

	public List<AbstractSystem> systems = new List<AbstractSystem>();
	public List<AbstractEntity> entities = new List<AbstractEntity>(); // todo - use dictionary with entity id
    private IEcsEventListener eventListener;

    public BasicEcs(IEcsEventListener _eventListener) {
        this.eventListener = _eventListener;

    }
	
	
	public void process() {
        // Remove any entities
        foreach(AbstractEntity entity in this.entities) {
            if (entity.markForRemoval) {
                this.entities.Remove(entity);
                this.eventListener.EneityRemoved(entity);
            }
        }

		foreach(AbstractSystem system in this.systems) { // todo -loop backwards
			system.process(this.entities);
		}
	}

}
