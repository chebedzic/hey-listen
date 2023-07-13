using System.Collections.Generic;

public static class CompareLists
{
    public static bool AreEqual<T>(List<T> list1, List<T> list2)
    {
        // Check if both lists have the same number of elements
        if (list1.Count != list2.Count)
        {
            return false;
        }

        // Compare each element of the lists
        for (int i = 0; i < list1.Count; i++)
        {
            // If any elements are not equal, return false
            if (!list1[i].Equals(list2[i]))
            {
                return false;
            }
        }

        // All elements are equal
        return true;
    }
}