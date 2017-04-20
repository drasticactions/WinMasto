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
    public class MediaAttachmentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ImageStatusDataTemplate { get; set; }

        public DataTemplate VideoStatusDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item,
            DependencyObject container)
        {
            var status = item as Attachment;
            return status.Url.Contains(".mp4") ? VideoStatusDataTemplate : ImageStatusDataTemplate;
        }
    }
}
