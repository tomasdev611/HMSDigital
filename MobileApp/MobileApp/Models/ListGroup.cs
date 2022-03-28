using System.Collections.Generic;

namespace MobileApp.Models
{
    public class ListGroup<T> : List<T> where T : class
    {
        public string Name { get; set; }

        public int Count { get; set; }

        public ListGroup(string groupName, List<T> list, int listCount) : base(list)
        {
            Name = groupName;
            Count = listCount;
        }
    }
}
