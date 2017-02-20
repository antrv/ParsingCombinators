namespace AR.Parsing
{
    public interface IParserInput<out T>
    {
        int Position { get; }
        T Current { get; }
        bool Eof { get; }
        IParserInput<T> Next { get; }
    }
}