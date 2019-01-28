using System;

namespace Security
{
    public struct UInt16 : IFormattable, IComparable, IComparable<UInt16>, IComparable<ushort>, IEquatable<UInt16>, IEquatable<ushort>
    {
        public const ushort MaxValue = ushort.MaxValue;
        public const ushort MinValue = ushort.MinValue;

#if UNITY_EDITOR
        ushort m_debugValue;
#endif

#if NO_SECURITY
        private ushort m_value;
#elif SIMPLE_SECURITY
        private UInt16Chiper m_chiper;
#else
        private UInt16Chiper m_chiper;
        private UInt16Chiper m_chiperCmp;
#endif

        //---------------------------------------------------------------------
        //  Get / Set Secure Value
        //---------------------------------------------------------------------
        #region Get / Set Secure Value

        private ushort GetValue()
        {
#if NO_SECURITY
            return m_value;
#elif UNITY_EDITOR
            ushort value = m_chiper.GetValue();
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

        private void SetValue(ushort value)
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
            ushort v1 = m_chiper.GetValue();
            ushort v2 = m_chiperCmp.GetValue();
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

        public UInt16(ushort value)
        {
#if UNITY_EDITOR
            m_debugValue = value;
#endif

#if NO_SECURITY
            m_value = value;
#elif SIMPLE_SECURITY
            m_chiper = new UInt16Chiper(value);
#else
            m_chiper = new UInt16Chiper(value);
            m_chiperCmp = new UInt16Chiper(value);
#endif
        }

        public ushort Value { get { return GetValue(); } set { SetValue(value); } }

        #endregion Get / Set Secure Value

        //---------------------------------------------------------------------
        //  Operator
        //---------------------------------------------------------------------
        #region Operator

        public static implicit operator ushort(UInt16 sValue)
        {
            return sValue.GetValue();
        }

        /*
        public static implicit operator UInt16(ushort value)
        {
            UInt16 sValue = new UInt16();
            sValue.SetValue(value);
            return sValue;
        }*/

        public static UInt16 operator ++(UInt16 sValue)
        {
            ushort value = sValue.GetValue();
            value++;
            sValue.SetValue(value);
            return sValue;
        }

        public static UInt16 operator --(UInt16 sValue)
        {
            ushort value = sValue.GetValue();
            value--;
            sValue.SetValue(value);
            return sValue;
        }

        #endregion Operator

        //---------------------------------------------------------------------
        //  Implement IComparable
        //---------------------------------------------------------------------
        #region Implement IComparable

        public int CompareTo(ushort value)
        {
            return GetValue().CompareTo(value);
        }

        public int CompareTo(UInt16 value)
        {
            return GetValue().CompareTo(value.GetValue());
        }

        public int CompareTo(object value)
        {
            return GetValue().CompareTo(((UInt16)value).GetValue());
        }

        #endregion Implement IComparable

        //---------------------------------------------------------------------
        //  Implement IEquatable
        //---------------------------------------------------------------------
        #region Implement IEquatable

        public bool Equals(ushort obj)
        {
            return GetValue().Equals(obj);
        }

        public bool Equals(UInt16 obj)
        {
            return GetValue().Equals(obj.GetValue());
        }

        public override bool Equals(object obj)
        {
            return GetValue().Equals(((UInt16)obj).GetValue());
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
