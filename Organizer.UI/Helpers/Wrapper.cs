using System.Windows;

namespace Organizer.UI.Helpers
{
    public class Wrapper : DependencyObject
    {
        public static readonly DependencyProperty WrappedDataProperty =
             DependencyProperty.Register("WrappedData", typeof(object),
             typeof(Wrapper), new FrameworkPropertyMetadata(null));

        public object WrappedData
        {
            get { return GetValue(WrappedDataProperty); }
            set { SetValue(WrappedDataProperty, value); }
        }
    }
}