using MouseAndCreate.Configurations;
using MouseAndCreate.Play;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MouseAndCreate.Frames
{
    class FrameManager : IFrameManager
    {
        private readonly IGame _game;

        private readonly object _listLock = new object();
        public IReadOnlyCollection<IFrame> Frames => _framesList;
        private readonly List<IFrame> _framesList = new List<IFrame>();
        private readonly ConcurrentDictionary<Guid, IFrame> _frameMap = new ConcurrentDictionary<Guid, IFrame>();

        public FrameManager(IGame game)
        {
            _game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public IFrame CurrentFrame { get => _currentFrame; set => _currentFrame = value; }
        private IFrame _currentFrame = null;

        public IFrame AddFrame()
        {
            Guid id = Guid.NewGuid();
            IFrame result = new Frame(new FrameSetup(_game.Setup), id);
            lock (_listLock)
            {
                _frameMap.TryAdd(result.Id, result);
                _framesList.Add(result);
            }
            return result;
        }

        public bool RemoveFrame(Guid id)
        {
            if (!_frameMap.TryGetValue(id, out IFrame frame))
                return false;
            lock (_listLock)
            {
                _frameMap.Remove(id, out _);
                _framesList.Remove(frame);
                frame.Dispose();
            }
            return true;
        }

        public bool RemoveFrame(IFrame frame) => frame is not null && RemoveFrame(frame.Id);

        public bool ContainsFrame(Guid id) => _frameMap.ContainsKey(id);
        public IFrame GetFrameById(Guid id)
        {
            if (_frameMap.TryGetValue(id, out IFrame result))
                return result;
            return null;
        }

        public void ClearFrames()
        {
            IFrame[] frames = _framesList.ToArray();
            lock (_listLock)
            {
                _frameMap.Clear();
                _framesList.Clear();
            }
            foreach (IFrame frame in frames)
                frame.Dispose();
        }
    }
}
