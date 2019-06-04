using System.Collections.Generic;

public class BasicEcs {

	public List<AbstractSystem> systems = new List<AbstractSystem>();
	public List<AbstractEntity> entities = new List<AbstractEntity>(); // todo - use dictionary with entity id
	
	
	public BasicEcs() {

	}
	
	
	public void process() {
		foreach(AbstractSystem system in this.systems) { // todo -loop backwards
			system.process(this.entities);
		}
	}

}
