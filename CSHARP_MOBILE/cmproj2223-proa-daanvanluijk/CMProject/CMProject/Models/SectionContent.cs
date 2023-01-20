using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMProject.Models
{
    public abstract partial class SectionContent : ObservableObject
    {
        public enum ContentTypes
        {
            Text,
            Image,
        }

        abstract public ContentTypes ContentType { get; set; }

        [ObservableProperty]
        private object showContent;
    }
}
