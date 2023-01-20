using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMProject.Models
{
    public partial class ImageContent : SectionContent
    {
        public override ContentTypes ContentType { get; set; } = ContentTypes.Image;

        [ObservableProperty]
        private ImageSource showContent;

        public ImageContent()
        {
            showContent = "dotnet_bot.svg";
        }
    }
}
