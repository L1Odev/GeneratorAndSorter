namespace Sorter.Models;

public struct Line : IComparable<Line>
{
    private const string Value = ". ";
    public int FileIndex { get; set; }
    public string Text { get; set; }
    public int Number { get; set; }
    public string OriginalLine { get; }
    
    public Line(int fileIndex, string line)
    {
        FileIndex = fileIndex;
        OriginalLine = line;
        int separatorIndex = line.IndexOf(Value, StringComparison.Ordinal);
        Number = int.Parse(line.AsSpan(0, separatorIndex));
        Text = line.AsSpan(separatorIndex + 1, line.Length - separatorIndex - 1).ToString();
    }
    
    public int CompareTo(Line other)
    {
        int cmp = string.Compare(Text, other.Text, StringComparison.Ordinal);
        return cmp != 0 ? cmp : Number.CompareTo(other.Number);
    }
}