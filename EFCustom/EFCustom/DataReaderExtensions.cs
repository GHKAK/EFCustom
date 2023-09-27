using System.Data;

namespace EFCustom; 

public static class DataReaderExtensions {
    public static bool TryGetOrdinal(this IDataReader reader, string columnName, out int ordinalIndex) {
        try {
            ordinalIndex = reader.GetOrdinal(columnName);
            return true;
        } catch (IndexOutOfRangeException) {
            ordinalIndex = -1;
            return false;
        }
    }

    public static T GetValue<T>(this IDataReader reader, string columnName) {
        if (reader.TryGetOrdinal(columnName, out int ordinalIndex)) {
            return (T)reader.GetValue(ordinalIndex);
        }

        return default;
    }
}