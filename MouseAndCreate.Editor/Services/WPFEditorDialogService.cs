using MouseAndCreate.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace MouseAndCreate.Editor.Services
{
    class WPFEditorDialogService : IEditorDialogService
    {
        private readonly Window _owner;

        public WPFEditorDialogService(Window owner)
        {
            _owner = owner;
        }

        public bool? ShowEditFrame(IFrameManager frameMng, FrameViewModel frame)
        {
            FrameEditorWindow window = new FrameEditorWindow()
            {
                Owner = _owner,
                DataContext = new FrameEditorViewModel(frameMng, frame),
            };           
            return window.ShowDialog();
        }
    }
}
