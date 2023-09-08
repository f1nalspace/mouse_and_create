using DevExpress.Mvvm;
using MouseAndCreate.Configurations;
using MouseAndCreate.Graphics;
using MouseAndCreate.Objects;
using OpenTK.Mathematics;
using System;

namespace MouseAndCreate.Frames
{
    public class Frame : BindableBase, IFrame
    {
        public const int ImageWidth = 128;
        public const int ImageHeight = 128;

        public Guid Id { get; }
        public string Name { get => GetValue<string>(); set => SetValue(value); }
        public IImage Image { get => GetValue<IImage>(); set => SetValue(value); }

        public IGameObjectManager ObjectMng => _gameObjectManager;

        public FrameSetup Setup { get; }

        private readonly GameObjectManager _gameObjectManager = new GameObjectManager();

        public Frame(FrameSetup setup, Guid? id = null, string name = null)
        {
            Setup = setup ?? throw new ArgumentNullException(nameof(setup));
            Id = id ?? Guid.NewGuid();
            Name = name;
            Image = ImageGenerator.CreateSolidImage(ImageWidth, ImageHeight, new Color4(1.0f, 1.0f, 1.0f, 1.0f));
        }

        public override string ToString() => string.IsNullOrWhiteSpace(Name) ? "Unnamed" : Name;

        private bool _disposed = false;
        public void Dispose()
        {
            if (!_disposed)
            {
                _gameObjectManager.ClearObjects();
                _disposed = true;
            }
        }

        public bool Equals(IFrame other) => other is not null && Id.Equals(other.Id);
        public override bool Equals(object obj) => obj is Frame frame && Equals(frame);
        public override int GetHashCode() => Id.GetHashCode();
    }
}
