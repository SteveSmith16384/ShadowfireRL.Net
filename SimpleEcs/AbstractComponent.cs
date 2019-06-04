public abstract class AbstractComponent {

	public AbstractComponent() {
	}

	
	public string getName() {
		return this.GetType().ToString();
	}
}
