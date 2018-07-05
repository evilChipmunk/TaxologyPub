namespace Shared.Domain
{
    public class State : ValueObject<State>
    {
        public State(string ab, string name)
        {
            Name = name;
            Abbreviation = ab;
        }

        public string Name { get; private set; }

        public string Abbreviation { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Abbreviation, Name);
        }
    }
}