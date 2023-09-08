using System;

namespace MouseAndCreate.Graphics
{
    public interface IImage : IDisposable
    {
        Guid Id { get; }
        string Name { get; set; }
        int Width { get; }
        int Height { get; }
        int Components { get; }
        ImageFormat Format { get; }
        ReadOnlySpan<byte> Data { get; }
    }
}
