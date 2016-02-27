using System;

namespace CeMaS.Common
{
    public interface IAlive
    {
        bool IsAlive { get; }
        event EventHandler IsAliveChanged;
    }
}
