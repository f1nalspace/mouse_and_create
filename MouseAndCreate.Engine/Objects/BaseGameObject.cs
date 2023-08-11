using OpenTK.Mathematics;
using System;

namespace MouseAndCreate.Objects;

public abstract class BaseGameObject : IGameObject
{
    public Guid Id { get; }

    public string Name { get; set; }

    public Vector2 Pos { get => _pos; set => SetTranslationValue(ref _pos, value); }
    private Vector2 _pos = Vector2.Zero;

    public Vector2 Scale { get => _scale; set => SetTranslationValue(ref _scale, value); }
    private Vector2 _scale = Vector2.One;

    public Matrix2 Rotation { get => _rotation; set => SetTranslationValue(ref _rotation, value); }
    private Matrix2 _rotation = Matrix2.Identity;
    private bool _disposed;

    public event TranslationChangedEventHandler TranslationChanged;

    private void SetTranslationValue<T>(ref T storage, T value)
    {
        storage = value;
        TranslationChanged?.Invoke(this);
    }

    protected BaseGameObject(Guid? id = null, string name = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), "Name not allowed to be empty");
        Id = id ?? Guid.NewGuid();
        Name = name;
    }

    public override string ToString() => $"{Name} [{Id}]";

    public virtual void Assign(IGameObject other)
    {
        if (other is null)
            return;

        Name = other.Name;

        Pos = other.Pos;
        Scale = other.Scale;
        Rotation = other.Rotation;
    }

    public abstract IGameObject Clone();

    public bool Equals(IGameObject other)
    {
        if (other is null)
            return false;
        return Id.Equals(other.Id);
    }
    public override bool Equals(object obj) => obj is IGameObject gameObject && Equals(gameObject);
    public override int GetHashCode() => Id.GetHashCode();

    protected virtual void DisposeManaged() { }
    protected virtual void DisposeUnmanaged() { }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
                DisposeManaged();
            DisposeUnmanaged();
            _disposed = true;
        }
    }

    ~BaseGameObject() => Dispose(disposing: false);

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
