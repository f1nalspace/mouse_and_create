using DevExpress.Mvvm;
using MouseAndCreate.Editor.Utils;
using MouseAndCreate.Frames;
using System;
using System.Windows.Media;

namespace MouseAndCreate.Editor
{
    public class FrameViewModel : ViewModelBase
    {
        private readonly IFrame _frame;

        public Guid Id => _frame.Id;
        public string Name
        {
            get => _frame.Name;
            set => _frame.Name = value;
        }

        public ImageSource Image { get => GetValue<ImageSource>(); set => SetValue(value); }

        public FrameViewModel(IFrame frame)
        {
            _frame = frame;
            _frame.PropertyChanged += OnFramePropertyChanged;

            Image = ImageUtils.CreateImageSourceFrom(frame.Image);
        }

        private void OnFramePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IFrame.Name):
                    RaisePropertyChanged(nameof(Name));
                    break;
                case nameof(IFrame.Image):
                    Image = ImageUtils.CreateImageSourceFrom(_frame.Image);
                    break;
            }
        }

        public override string ToString() => _frame.ToString();
    }
}
