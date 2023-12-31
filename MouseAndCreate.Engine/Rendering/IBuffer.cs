﻿using MouseAndCreate.Types;
using System;

namespace MouseAndCreate.Rendering
{
    public interface IBuffer : IResource, IDisposable
    {
        void UpdateVertices(int offset, int length, float[] data);
        void UpdateElements(int offset, int length, uint[] data);

        void UseVertices();
        void UseElements();
    }
}
