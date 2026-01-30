namespace Sorter.Utility;

public class SortComparer : IComparer<string>
{
    public int Compare(string a, string b)
    {
        var splitA = a.Split([". "], 2, StringSplitOptions.None);
        var splitB = b.Split([". "], 2, StringSplitOptions.None);

        string stringA = splitA[1];
        string stringB = splitB[1];

        int stringComparison = String.Compare(stringA, stringB, StringComparison.Ordinal);
        if (stringComparison != 0)
        {
            return stringComparison;
        }

        int numberA = int.Parse(splitA[0]);
        int numberB = int.Parse(splitB[0]);
        return numberA.CompareTo(numberB);
    }
}