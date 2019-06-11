using System.Collections.Generic;

public abstract class AbstractSystem {

    public bool runEachLoop;
    protected BasicEcs ecs;

    public AbstractSystem(BasicEcs _ecs, bool _runEachLoop) {
        this.ecs = _ecs;
        this.runEachLoop = _runEachLoop;

        this.ecs.AddSystem(this);

    }

	
	public virtual void Process(List<AbstractEntity> entities) {
		foreach (AbstractEntity entity in entities) {
			this.ProcessEntity(entity);
		}
	}
	
	
	public virtual void ProcessEntity(AbstractEntity entity) {
		// Override if required
	}


}
