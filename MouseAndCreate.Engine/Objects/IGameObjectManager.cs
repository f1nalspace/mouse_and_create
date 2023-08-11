using System.Collections.Generic;

namespace MouseAndCreate.Objects;

public interface IGameObjectManager
{
    IReadOnlyCollection<IGameObject> Objects { get; }
    IReadOnlyCollection<ActiveObject> ActiveObjects { get; }
    IReadOnlyCollection<BackgroundObject> BackgroundObjects { get; }

    BackgroundObject CreateBackgroundObject();

    ActiveObject CreateActiveObject(ActiveObject template = null);
    ActiveObject[] CreateActiveObjects(uint count, ActiveObject template = null);

    void AddObject(IGameObject obj);
    void AddObjects(IGameObject[] objects);

    bool RemoveObject(IGameObject obj);
    bool RemoveObjects(IGameObject[] objects);

    void ClearObjects();
}
