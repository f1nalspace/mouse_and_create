using DevExpress.Mvvm;
using MouseAndCreate.Frames;
using System;

namespace MouseAndCreate.Editor
{
    public class FrameViewModel : ViewModelBase
    {
        private readonly IFrame _frame;

        public Guid Id => _frame.Id;
        public string Name => _frame.Name;

        public FrameViewModel(IFrame frame)
        {
            _frame = frame;
            _frame.PropertyChanged += OnFramePropertyChanged;
        }

        private void OnFramePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (nameof(IFrame.Name).Equals(e.PropertyName))
                RaisePropertyChanged(nameof(Name));
        }
    }
}
