namespace GigaPenterEngine.Core;

public class Entity
{
    private List<IComponent> components = new List<IComponent>();

    public T AddComponent<T>(T component) where T : IComponent
    {
        component.Parent = this;
        components.Add(component);
        return component;
    }
    
    public T GetComponent<T>() where T : IComponent
    {
        return components.OfType<T>().FirstOrDefault();
    }
    
    public List<T> GetComponents<T>() where T : IComponent
    {
        return components.OfType<T>().ToList();
    }
}