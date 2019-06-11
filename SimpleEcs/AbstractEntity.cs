using System.Collections.Generic;

public class AbstractEntity {

    private static int next_id = 0;

    public readonly int id;
    public readonly string name;
    private Dictionary<string, AbstractComponent> components = new Dictionary<string, AbstractComponent>();
    public bool markForRemoval = false;

    public AbstractEntity(string _name) {
        this.id = next_id++;
        this.name = _name;
    }


    public void AddComponent(AbstractComponent component) {
        this.components.Add(component.GetType().Name, component);
    }


    public void RemoveComponent(string name) {
        this.components.Remove(name);

    }


    public AbstractComponent GetComponent(string name) {
        if (this.components.ContainsKey(name)) {
            return this.components[name];
        } else {
            return null;
        }
    }


    public Dictionary<string, AbstractComponent> GetComponents() {
        return this.components;
    }


}
