namespace Gamefreak130.Common.Booters
{
    public abstract class Booter
    {
        protected string mXmlResource;

        public Booter(string xmlResource) => mXmlResource = xmlResource;

        public abstract void LoadData();
    }
}
