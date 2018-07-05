namespace Shared.Service
{
    public class HeaderAccept
    {
        public string Type;

        private HeaderAccept(string type)
        {
            Type = type;
        }

        public static HeaderAccept None = new HeaderAccept("None");

        public static HeaderAccept Json = new HeaderAccept("application/json");

        public static HeaderAccept XML = new HeaderAccept("application/xml");

        public static HeaderAccept TextJson = new HeaderAccept("text/json");

        public static HeaderAccept TextXML = new HeaderAccept("text/xml");

        public static HeaderAccept TextPlain = new HeaderAccept("text/plain");

        public override string ToString()
        {
            return Type;
        }
    }
}