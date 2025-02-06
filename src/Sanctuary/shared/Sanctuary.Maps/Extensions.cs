namespace Sanctuary.Maps
{
    public static class StringExtensions
    {
        public static T ToEnum<T>(this string value, T defaultValue) where T: Enum
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
