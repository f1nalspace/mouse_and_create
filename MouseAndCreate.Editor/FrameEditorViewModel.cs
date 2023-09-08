using DevExpress.Mvvm;
using MouseAndCreate.Frames;
using System;

namespace MouseAndCreate.Editor
{
    public class FrameEditorViewModel : ViewModelBase
    {
        public string Title => $"Edit Frame '{_frame.Name}'";

        public Guid Id => _frame.Id;

        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value, () => NameChanged(value));
        }
        private string _name;

        private readonly IFrameManager _frameMng;
        private readonly FrameViewModel _frame;
        private readonly string _initialName;

        public DelegateCommand ApplyCommand { get; }

        public FrameEditorViewModel(IFrameManager frameMng, FrameViewModel frame)
        {
            _frameMng = frameMng ?? throw new ArgumentNullException(nameof(frameMng));
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));
            _initialName = _name = frame.Name;
            ApplyCommand = new DelegateCommand(Apply, CanApply);
        }

        private bool CanApply() => !string.Equals(_initialName, Name, StringComparison.InvariantCultureIgnoreCase) && !_frameMng.ContainsFrame(Name);
        private void Apply()
        {
            _frame.Name = Name;
        }

        private void NameChanged(string newName)
        {
            ApplyCommand.RaiseCanExecuteChanged();
        }
    }
}
