using System;

namespace TSDFF.DataTypes
{
    /// <summary>
    /// WIP: New Byte format is under developing, use Data instread.
    /// </summary>
    [Obsolete] public class Byte : IData
    {
        object value;
        
        public byte GetValue()
        {
            return (byte)this.value;
        }
        public void SetValue(byte value)
        {
            this.value = value;
        }
    }
}
