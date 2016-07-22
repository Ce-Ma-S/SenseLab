using CeMaS.Common.Properties;

namespace CeMaS.Common.Units
{
    public static class Units
    {
        public static readonly Unit<string> Percentage = new Unit<string>(nameof(Percentage), Resources.Percentage_Name, "%");
        public static readonly Unit<string> Hertz = new Unit<string>(nameof(Hertz), Resources.Hertz_Name, "Hz");
    }
}
