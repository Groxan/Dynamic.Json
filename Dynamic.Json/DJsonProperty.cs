namespace Dynamic.Json
{
    public sealed class DJsonProperty
    {
        public string Name { get; set; }

        public DJson Value { get; set; }

        internal DJsonProperty(string name, DJson value)
        {
            Name = name;
            Value = value;
        }
    }
}
