using System;

namespace Security
{
    public struct UInt32 : IFormattable, IComparable, IComparable<UInt32>, IComparable<uint>, IEquatable<UInt32>, IEquatable<uint>
    {
        public const uint MaxValue = uint.MaxValue;
        public const uint MinValue = uint.MinValue;

#if UNITY_EDITOR
        uint m_debugValue;
#endif

#if NO_SECURITY
        private uint m_value;
#elif SIMPLE_SECURITY
        private UInt32Chiper m_chiper;
#else
        private UInt32Chiper m_chiper;
        private UInt32Chiper m_chiperCmp;
#endif

        //---------------------------------------------------------------------
        //  Get / Set Secure Value
        //---------------------------------------------------------------------
        #region Get / Set Secure Value

        private uint GetValue()
        {
#if NO_SECURITY
            return m_value;
#elif UNITY_EDITOR
            uint value = m_chiper.GetValue();
            // Self error check
            if (value != m_debugValue)
            {
                string message = string.Format("[{0}] Dec({1}) != DebugV({2})"
                    , GetType().ToString()
                    , value
                    , m_debugValue);
                SecurityListener.OnError(message);
            }
            return value;
#else
            return m_chiper.GetValue();
#endif
        }

        private void SetValue(uint value)
        {
#if NO_SECURITY
            m_value = value;
#elif SIMPLE_SECURITY
            m_chiper.SetValue(value);

            // Save debug value
    #if UNITY_EDITOR
            m_debugValue = value;
    #endif
#else
            uint v1 = m_chiper.GetValue();
            uint v2 = m_chiperCmp.GetValue();
            if (v1 != v2)
            {
                // Detected hacking
                string message = string.Format("[{0}] v1({1}) != v2({2})"
                    , GetType().ToString()
                    , v1
                    , v2);
                SecurityListener.OnHackDetect(message);
            }

            m_chiper.SetValue(value);
            m_chiperCmp.SetValue(value);

            // Save debug value
    #if UNITY_EDITOR
            m_debugValue = value;
    #endif
#endif
        }

        public UInt32(uint value)
        {
#if UNITY_EDITOR
            m_debugValue = value;
#endif

#if NO_SECURITY
            m_value = value;
#elif SIMPLE_SECURITY
            m_chiper = new UInt32Chiper(value);
#else
            m_chiper = new UInt32Chiper(value);
            m_chiperCmp = new UInt32Chiper(value);
#endif
        }

        public uint Value { get { return GetValue(); } set { SetValue(value); } }

        #endregion Get / Set Secure Value

        //---------------------------------------------------------------------
        //  Operator
        //---------------------------------------------------------------------
        #region Operator

        public static implicit operator uint(UInt32 sValue)
        {
            return sValue.GetValue();
        }

        /*
        public static implicit operator UInt32(uint value)
        {
            UInt32 sValue = new UInt32();
            sValue.SetValue(value);
            return sValue;
        }
        */

        public static UInt32 operator ++(UInt32 sValue)
        {
            uint value = sValue.GetValue();
            value++;
            sValue.SetValue(value);
            return sValue;
        }

        public static UInt32 operator --(UInt32 sValue)
        {
            uint value = sValue.GetValue();
            value--;
            sValue.SetValue(value);
            return sValue;
        }

        #endregion Operator

        //---------------------------------------------------------------------
        //  Implement IComparable
        //---------------------------------------------------------------------
        #region Implement IComparable

        public int CompareTo(uint value)
        {
            return GetValue().CompareTo(value);
        }

        public int CompareTo(UInt32 value)
        {
            return GetValue().CompareTo(value.GetValue());
        }

        public int CompareTo(object value)
        {
            return GetValue().CompareTo(((UInt32)value).GetValue());
        }

        #endregion Implement IComparable

        //---------------------------------------------------------------------
        //  Implement IEquatable
        //---------------------------------------------------------------------
        #region Implement IEquatable

        public bool Equals(uint obj)
        {
            return GetValue().Equals(obj);
        }

        public bool Equals(UInt32 obj)
        {
            return GetValue().Equals(obj.GetValue());
        }

        public override bool Equals(object obj)
        {
            return GetValue().Equals(((UInt32)obj).GetValue());
        }

        public override int GetHashCode()
        {
            return GetValue().GetHashCode();
        }

        #endregion Implement IEquatable

        //---------------------------------------------------------------------
        //  Implement IFormattable
        //---------------------------------------------------------------------
        #region Implement IFormattable

        public override string ToString()
        {
            return GetValue().ToString();
        }

        public string ToString(IFormatProvider provider)
        {
            return GetValue().ToString(provider);
        }

        public string ToString(string format)
        {
            return GetValue().ToString(format);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return GetValue().ToString(format, provider);
        }

        #endregion Implement IFormattable
    }
}
