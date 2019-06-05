using System.Collections.Generic;

public class AbstractEntity {

    private static int next_id = 0;

    public readonly int id;
    public readonly string name;
    public Dictionary<string, AbstractComponent> components = new Dictionary<string, AbstractComponent>();

    public AbstractEntity(string _name) {
        this.id = next_id++;
        this.name = _name;
    }


    public void addComponent(AbstractComponent component) {
        this.components.Add(component.GetType().Name, component);
    }


    public AbstractComponent getComponent(string name) {
        if (this.components.ContainsKey(name)) {
            return this.components[name];
        } else {
            return null;
        }
    }
}
