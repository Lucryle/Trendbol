namespace Trendbol
{
    public class Category
    {
        public int CategoryID{get; set;}
        public string Name{get; set;}
        public string Description{get; set;}

        public Category(int categoryID, string name, string description)
        {
            CategoryID = categoryID;
            Name = name;
            Description = description;
        }

        // TODO
    }
}
