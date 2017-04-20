using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Mastonet.Entities;

namespace WinMasto.Models
{
    public class PhotoFileInfo
    {
        public StorageFile File { get; set; }

        public StorageItemThumbnail Thumbnail { get; set; }

        public Attachment Attachment { get; set; }
    }
}
