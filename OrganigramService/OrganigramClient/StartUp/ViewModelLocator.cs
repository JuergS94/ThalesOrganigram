using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrganigramClient.StartUp
{
    public static class ViewModelLocator
    {
        /// <summary>  
        /// Gets AutoWireViewModel attached property  
        /// </summary>  
        /// <param name="obj"></param>  
        /// <returns></returns>  
        public static bool GetAutoWireViewModel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoWireViewModelProperty);
        }

        /// <summary>  
        /// Sets AutoWireViewModel attached property  
        /// </summary>  
        /// <param name="obj"></param>  
        /// <param name="value"></param>  
        public static void SetAutoWireViewModel(DependencyObject obj, bool value)
        {
            obj?.SetValue(AutoWireViewModelProperty, value);
        }

        /// <summary>  
        /// AutoWireViewModel attached property  
        /// </summary>  
        public static readonly DependencyProperty AutoWireViewModelProperty =
            DependencyProperty.RegisterAttached("AutoWireViewModel",
            typeof(bool), typeof(ViewModelLocator),
            new PropertyMetadata(false, AutoWireViewModelChanged));

        /// <summary>  
        /// Step 5 approach to hookup View with ViewModel  
        /// </summary>  
        /// <param name="d"></param>  
        /// <param name="e"></param>  
        private static void AutoWireViewModelChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
            var viewTypeName = d.GetType().FullName;
            var viewModelTypeName = viewTypeName.Replace("View", "ViewModel");
            if (!viewModelTypeName.EndsWith("ViewModel", StringComparison.InvariantCultureIgnoreCase))
            {
                viewModelTypeName += "ViewModel";
            }
            var viewModelType = Type.GetType(viewModelTypeName);
            var viewModel = Bootstrapper.Resolve(viewModelType);
            ((FrameworkElement)d).DataContext = viewModel;
        }
    }
}
