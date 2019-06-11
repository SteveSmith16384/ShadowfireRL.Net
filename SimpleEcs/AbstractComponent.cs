public abstract class AbstractComponent {

	public AbstractComponent() {
	}

	
	public string GetName() {
		return this.GetType().ToString();
	}
}
