using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MouseAndCreate.Objects;

class GameObjectManager : IGameObjectManager
{
    private ulong _nameCounter = 0;
    private readonly object _objectsLock = new object();

    public IReadOnlyCollection<IGameObject> Objects => _objectList;
    private readonly List<IGameObject> _objectList = new List<IGameObject>();
    private readonly ConcurrentDictionary<Guid, IGameObject> _objectMap = new ConcurrentDictionary<Guid, IGameObject>();

    public IReadOnlyCollection<ActiveObject> ActiveObjects => _activeObjects;
    private readonly List<ActiveObject> _activeObjects = new List<ActiveObject>();

    public IReadOnlyCollection<BackgroundObject> BackgroundObjects => _backgroundObjects;
    private readonly List<BackgroundObject> _backgroundObjects = new List<BackgroundObject>();

    private string GetName(string prefix)
    {
        ulong nameIndex = Interlocked.Increment(ref _nameCounter);
        return $"{prefix} {nameIndex}";
    }

    public ActiveObject CreateActiveObject(ActiveObject template = null)
    {
        string name = GetName("ActiveObject");
        ActiveObject result = new ActiveObject(Guid.NewGuid(), name);
        if (template is not null)
            result.Assign(template);
        return result;
    }

    public ActiveObject[] CreateActiveObjects(uint count, ActiveObject template = null)
    {
        ActiveObject[] result = new ActiveObject[count];
        for (uint i = 0; i < count; i++)
            result[i] = CreateActiveObject(template);
        return result;
    }

    public BackgroundObject CreateBackgroundObject()
    {
        string name = GetName("BackgroundObject");
        BackgroundObject result = new ColorBackgroundObject(Guid.NewGuid(), name);
        return result;

    }

    public void AddObject(IGameObject obj)
    {
        if (obj is null)
            throw new ArgumentNullException(nameof(obj));
        if (_objectMap.ContainsKey(obj.Id))
            throw new ArgumentException($"The object '{obj}' already added", nameof(obj));

        if (!_objectMap.TryAdd(obj.Id, obj))
            return;

        lock (_objectsLock)
        {
            _objectList.Add(obj);
            if (obj is BackgroundObject backgroundObject)
                _backgroundObjects.Add(backgroundObject);
            else if (obj is ActiveObject activeObject)
                _activeObjects.Add(activeObject);
        }
    }

    public void AddObjects(IGameObject[] objects)
    {
        if (objects is null)
            throw new ArgumentNullException(nameof(objects));
        if (objects.Any(o => _objectMap.ContainsKey(o.Id)))
            throw new ArgumentException($"At least one object is already added", nameof(objects));
        lock (_objectsLock)
        {
            foreach (IGameObject obj in objects)
                _objectMap.AddOrUpdate(obj.Id, obj, (id, old) => obj);
            _objectList.AddRange(objects);
            _backgroundObjects.AddRange(objects.OfType<BackgroundObject>());
            _activeObjects.AddRange(objects.OfType<ActiveObject>());

        }
    }

    public bool RemoveObject(IGameObject obj)
    {
        if (obj is null)
            return false;
        if (!_objectMap.TryRemove(obj.Id, out _))
            return false;
        lock (_objectsLock)
        {
            _objectList.Remove(obj);
            if (obj is ActiveObject activeObject)
                _activeObjects.Remove(activeObject);
            else if (obj is BackgroundObject backgroundObject)
                _backgroundObjects.Remove(backgroundObject);
            obj.Dispose();
        }
        return true;
    }

    public bool RemoveObjects(IGameObject[] objects)
    {
        if (objects is null)
            return false;
        if (objects.All(o => _objectMap.ContainsKey(o.Id)))
            return false;

        lock (_objectsLock)
        {
            foreach (IGameObject obj in objects)
                _objectMap.Remove(obj.Id, out _);
            _objectList.RemoveAll(o => objects.Contains(o));
            _activeObjects.RemoveAll(o => objects.Contains(o));
            _backgroundObjects.RemoveAll(o => objects.Contains(o));
        }

        foreach (IGameObject obj in objects)
            obj.Dispose();

        return true;
    }

    public void ClearObjects()
    {
        IGameObject[] allObjects = _objectList.ToArray();
        RemoveObjects(allObjects);
    }
}
