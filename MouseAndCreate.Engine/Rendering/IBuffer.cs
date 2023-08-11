using System;

namespace MouseAndCreate.Rendering
{
    public interface IBuffer : IDisposable
    {
        string Name { get; }

        void UpdateVertices(int offset, int length, float[] data);
        void UpdateElements(int offset, int length, uint[] data);

        void UseVertices();
        void UseElements();
    }
}
