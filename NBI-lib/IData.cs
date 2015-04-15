using System;

namespace TSDFF
{
    /// <summary>
    /// WIP: New Data format is under developing.
    /// </summary>
    [Obsolete] public interface IData
    {
       virtual object value { get; }
       virtual string name { get; }
       public void SetValue();
       object GetValue();

       // virtual IData(byte value, string Name);

    }
}
