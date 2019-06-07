using System.Collections.Generic;

public abstract class AbstractSystem {

	public AbstractSystem() {
	}

	
	public virtual void process(List<AbstractEntity> entities) {
		foreach (AbstractEntity entity in entities) {
			this.processEntity(entity);
		}
	}
	
	
	public virtual void processEntity(AbstractEntity entity) {
		// Override if required
	}


}
