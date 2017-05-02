using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastonet.Entities;

namespace WinMasto.Models
{
    public class NewStatusParameter
    {
        public Status Status { get; set; }

        public bool IsReply { get; set; }

        public bool IsMention { get; set; }
    }
}
