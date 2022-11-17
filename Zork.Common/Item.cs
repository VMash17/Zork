namespace Zork.Common
{
    public class Item
    {
        public string Name { get; }

        public string Description { get; }

        public Item(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public override string ToString() => Name;
    }
}
