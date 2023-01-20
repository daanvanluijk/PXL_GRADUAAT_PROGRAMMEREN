using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMProject.Models
{
    public partial class Section : ObservableObject
    {
        [ObservableProperty]
        private SectionContent _content;

        [ObservableProperty]
        private bool currentAndHighlightedSectionNrMatch = false;

        public Section()
        {
            
        }
    }
}
