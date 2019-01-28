using System;

namespace Security
{
    public struct UInt64 : IFormattable, IComparable, IComparable<UInt64>, IComparable<ulong>, IEquatable<UInt64>, IEquatable<ulong>
    {
        public const ulong MaxValue = ulong.MaxValue;
        public const ulong MinValue = ulong.MinValue;

#if UNITY_EDITOR
        ulong m_debugValue;
#endif

#if NO_SECURITY
        private ulong m_value;
#elif SIMPLE_SECURITY
        private UInt64Chiper m_chiper;
#else
        private UInt64Chiper m_chiper;
        private UInt64Chiper m_chiperCmp;
#endif

        //---------------------------------------------------------------------
        //  Get / Set Secure Value
        //---------------------------------------------------------------------
        #region Get / Set Secure Value

        private ulong GetValue()
        {
#if NO_SECURITY
            return m_value;
#elif UNITY_EDITOR
            ulong value = m_chiper.GetValue();

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

        private void SetValue(ulong value)
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
            ulong v1 = m_chiper.GetValue();
            ulong v2 = m_chiperCmp.GetValue();
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

        public UInt64(ulong value)
        {
#if UNITY_EDITOR
            m_debugValue = value;
#endif

#if NO_SECURITY
            m_value = value;
#elif SIMPLE_SECURITY
            m_chiper = new UInt64Chiper(value);
#else
            m_chiper = new UInt64Chiper(value);
            m_chiperCmp = new UInt64Chiper(value);
#endif
        }

        public ulong Value { get { return GetValue(); } set { SetValue(value); } }

        #endregion Get / Set Secure Value

        //---------------------------------------------------------------------
        //  Operator
        //---------------------------------------------------------------------
        #region Operator

        public static implicit operator ulong(UInt64 sValue)
        {
            return sValue.GetValue();
        }

        /*
        public static implicit operator UInt64(ulong value)
        {
            UInt64 sValue = new UInt64();
            sValue.SetValue(value);
            return sValue;
        }
         */

        public static UInt64 operator ++(UInt64 sValue)
        {
            ulong value = sValue.GetValue();
            value++;
            sValue.SetValue(value);
            return sValue;
        }

        public static UInt64 operator --(UInt64 sValue)
        {
            ulong value = sValue.GetValue();
            value--;
            sValue.SetValue(value);
            return sValue;
        }

        #endregion Operator

        //---------------------------------------------------------------------
        //  Implement IComparable
        //---------------------------------------------------------------------
        #region Implement IComparable
        public int CompareTo(ulong value)
        {
            return GetValue().CompareTo(value);
        }

        public int CompareTo(UInt64 value)
        {
            return GetValue().CompareTo(value.GetValue());
        }

        public int CompareTo(object value)
        {
            return GetValue().CompareTo(((UInt64)value).GetValue());
        }

        #endregion Implement IComparable

        //---------------------------------------------------------------------
        //  Implement IEquatable
        //---------------------------------------------------------------------
        #region Implement IEquatable

        public bool Equals(ulong obj)
        {
            return GetValue().Equals(obj);
        }

        public bool Equals(UInt64 obj)
        {
            return GetValue().Equals(obj.GetValue());
        }

        public override bool Equals(object obj)
        {
            return GetValue().Equals(((UInt64)obj).GetValue());
        }

        public override int GetHashCode()
        {
            return GetValue().GetHashCode();
        }

        #endregion Implement IEquatable

        //---------------------------------------------------------------------
        //  Implement IFormattables
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
