using System.Collections.Generic;

namespace Prompt
{
    internal class SelectableItem
    {
        public string Name { get; set; }

        public SelectableItem(string name)
        {
            Name = name;
        }
    }

    internal class SelectableItems : SelectableItem
    {
        public IEnumerable<SelectableItem> Children { get; set; }

        public SelectableItems(string name, params SelectableItem[] children) :
            base(name)
        {
            Children = children;
        }
    }
}