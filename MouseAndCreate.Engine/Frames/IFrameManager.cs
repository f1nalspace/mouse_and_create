using System;
using System.Collections.Generic;

namespace MouseAndCreate.Frames
{
    public interface IFrameManager
    {
        IReadOnlyCollection<IFrame> Frames { get; }

        IFrame AddFrame();
        bool RemoveFrame(IFrame frame);
        bool RemoveFrame(Guid id);

        IFrame GetFrameById(Guid id);
        bool ContainsFrame(Guid id);

        void ClearFrames();
    }
}
