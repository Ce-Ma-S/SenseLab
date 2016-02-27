using Windows.ApplicationModel.Resources;

namespace CeMaS.Common.Units
{
    public static class Units
    {
        public static readonly Unit Percentage = new Unit("Percentage", "%", resources.GetString("Percentage_Name"));
        public static readonly Unit Hertz = new Unit("Hertz", "Hz", resources.GetString("Hertz_Name"));

        private static ResourceLoader resources = ResourceLoader.GetForViewIndependentUse();
    }
}
