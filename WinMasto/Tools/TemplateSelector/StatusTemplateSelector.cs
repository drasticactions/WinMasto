using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Mastonet.Entities;

namespace WinMasto.Tools.TemplateSelector
{
    public class StatusTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RegularStatusDataTemplate { get; set; }

        public DataTemplate ReblogStatusDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item,
            DependencyObject container)
        {
            var status = item as Status;
            if (status == null) return RegularStatusDataTemplate;

            if (status.Reblog != null) return ReblogStatusDataTemplate;

            return RegularStatusDataTemplate;
        }
    }
}
