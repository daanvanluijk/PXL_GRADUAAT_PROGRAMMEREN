using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMProject.Models
{
    public partial class Paragraph : SectionContent
    {
        public override ContentTypes ContentType { get; set; } = ContentTypes.Text;

        [ObservableProperty]
        private string showContent;

        public Paragraph() : this("test")
        {
        }

        public Paragraph(string text)
        {
            showContent = text;
        }
    }
}
