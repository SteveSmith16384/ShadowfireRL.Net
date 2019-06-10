using System.Collections.Generic;

public abstract class AbstractSystem {

	public AbstractSystem() {
	}

	
	public virtual void process(List<AbstractEntity> entities) {
		foreach (AbstractEntity entity in entities) {
			this.ProcessEntity(entity);
		}
	}
	
	
	public virtual void ProcessEntity(AbstractEntity entity) {
		// Override if required
	}


}
