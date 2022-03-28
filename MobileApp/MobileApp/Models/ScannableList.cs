using MobileApp.DataBaseAttributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MobileApp.Models
{
    public class ScannableList : ObservableCollection<ScanItem>
    {
        public string Name { get; set; }

        public ScannableList(List<ScanItem> list, string name) : base(list)
        {
            Name = name;
        }

    }
}
