using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinMasto.Tools.TemplateSelector
{
    public class StatusTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RegularStatusDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item,
            DependencyObject container)
        {
            return RegularStatusDataTemplate;
        }
    }
}
