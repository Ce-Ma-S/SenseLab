namespace CeMaS.Common.Units
{
    public static class SI
    {
        public const string IdPrefix = "SI.";

        public static string GetId(string id)
        {
            return IdPrefix + id;
        }
    }
}
