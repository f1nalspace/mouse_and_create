using DevExpress.Mvvm;
using DevExpress.Mvvm.UI.Interactivity;
using OpenTK.Wpf;

namespace MouseAndCreate.Editor
{
    public class GLControlServiceBehavior : Behavior<GLWpfControl>
    {
        private WPFControlInputQueryService _service = null;

        protected override void OnAttached()
        {
            base.OnAttached();

            GLWpfControl ctrl = this.AssociatedObject;

            MainViewModel vm = ctrl.DataContext as MainViewModel;

            ISupportServices supportServices = vm as ISupportServices;

            _service = new WPFControlInputQueryService(ctrl);

            supportServices.ServiceContainer.RegisterService(_service);
        }

        protected override void OnDetaching()
        {
            GLWpfControl ctrl = this.AssociatedObject;

            MainViewModel vm = ctrl.DataContext as MainViewModel;

            ISupportServices supportServices = vm as ISupportServices;

            supportServices.ServiceContainer.UnregisterService(_service);

            base.OnDetaching();
        }
    }
}
