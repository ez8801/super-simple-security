using System;

namespace Security
{
    public struct Int64 : IFormattable, IComparable, IComparable<Int64>, IComparable<long>, IEquatable<Int64>, IEquatable<long>
    {
        public const long MaxValue = long.MaxValue;
        public const long MinValue = long.MinValue;

#if UNITY_EDITOR
        long m_debugValue;
#endif

#if NO_SECURITY
        private long m_value;
#elif SIMPLE_SECURITY
        private Int64Chiper m_chiper;
#else
        private Int64Chiper m_chiper;
        private Int64Chiper m_chiperCmp;
#endif

        //---------------------------------------------------------------------
        //  Get / Set Secure Value
        //---------------------------------------------------------------------
        #region Get / Set Secure Value

        private long GetValue()
        {
#if NO_SECURITY
            return m_value;
#elif UNITY_EDITOR
            long value = m_chiper.GetValue();
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

        private void SetValue(long value)
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
            long v1 = m_chiper.GetValue();
            long v2 = m_chiperCmp.GetValue();
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

        public Int64(long value)
        {
#if UNITY_EDITOR
            m_debugValue = value;
#endif

#if NO_SECURITY
            m_value = value;
#elif SIMPLE_SECURITY
            m_chiper = new Int64Chiper(value);
#else
            m_chiper = new Int64Chiper(value);
            m_chiperCmp = new Int64Chiper(value);
#endif
        }

        public long Value { get { return GetValue(); } set { SetValue(value); } }

        #endregion Get / Set Secure Value

        //---------------------------------------------------------------------
        //  Operator
        //---------------------------------------------------------------------
        #region Operator

        public static implicit operator long(Int64 sValue)
        {
            return sValue.GetValue();
        }

        /*
        public static implicit operator Int64(long value)
        {
            Int64 sValue = new Int64();
            sValue.SetValue(value);
            return sValue;
        }
        */

        public static Int64 operator ++(Int64 sValue)
        {
            long value = sValue.GetValue();
            value++;
            sValue.SetValue(value);
            return sValue;
        }

        public static Int64 operator --(Int64 sValue)
        {
            long value = sValue.GetValue();
            value--;
            sValue.SetValue(value);
            return sValue;
        }

        #endregion Operator

        //---------------------------------------------------------------------
        //  Implement IComparables
        //---------------------------------------------------------------------
        #region Implement IComparable

        public int CompareTo(long value)
        {
            return GetValue().CompareTo(value);
        }

        public int CompareTo(Int64 value)
        {
            return GetValue().CompareTo(value.GetValue());
        }

        public int CompareTo(object value)
        {
            return GetValue().CompareTo(((Int64)value).GetValue());
        }

        #endregion Implement IComparable

        //---------------------------------------------------------------------
        //  Implement IEquatable
        //---------------------------------------------------------------------
        #region Implement IEquatable

        public bool Equals(long obj)
        {
            return GetValue().Equals(obj);
        }

        public bool Equals(Int64 obj)
        {
            return GetValue().Equals(obj.GetValue());
        }

        public override bool Equals(object obj)
        {
            return GetValue().Equals(((Int64)obj).GetValue());
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
