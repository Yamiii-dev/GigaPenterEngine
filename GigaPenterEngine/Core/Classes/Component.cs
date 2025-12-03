namespace GigaPenterEngine.Core;

// Registry of all created components, used inside systems to access specific component types
public static class ComponentRegistry
{
    private static readonly Dictionary<Type, List<IComponent>> _components = new Dictionary<Type, List<IComponent>>();

    public static void Register(IComponent component)
    {
        var type = component.GetType();
        if (!_components.ContainsKey(type))
            _components[type] = new List<IComponent>();
        
        _components[type].Add(component);
    }

    public static void Unregister(IComponent component)
    {
        if (_components.ContainsKey(component.GetType()))
        {
            if(_components[component.GetType()].Contains(component))
                _components[component.GetType()].Remove(component);
        }
    }

    public static List<T> GetComponents<T>() where T : IComponent
    {
        if(_components.ContainsKey(typeof(T)))
            return _components[typeof(T)].Cast<T>().ToList();
        else
            return new List<T>();
    }
}

public abstract class Component : IComponent
{
    public Entity Parent { get; set; }
    protected Component()
    {
        // Register yourself to the Registry on init
        ComponentRegistry.Register(this);
    }

    public void Dispose()
    {
        // Remove yourself from the Registry if disposed.
        ComponentRegistry.Unregister(this);
    }
    
    ~Component()
    {
        // Dispose if the GC gets rid of the component.
        Dispose();
    }
}