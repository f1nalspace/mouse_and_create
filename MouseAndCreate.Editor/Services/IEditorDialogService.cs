using MouseAndCreate.Frames;

namespace MouseAndCreate.Editor.Services
{
    public interface IEditorDialogService
    {
        bool? ShowEditFrame(IFrameManager frameMng, FrameViewModel frame);
    }
}
