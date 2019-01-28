using System;

namespace Security
{
    public struct Single : IFormattable, IComparable, IComparable<Single>, IComparable<float>, IEquatable<Single>, IEquatable<float>
    {
        public const float MaxValue = float.MaxValue;
        public const float MinValue = float.MinValue;

#if UNITY_EDITOR
        float m_debugValue;
#endif

#if NO_SECURITY
        private float m_value;
#elif SIMPLE_SECURITY
        private SingleChiper m_chiper;
#else
        private SingleChiper m_chiper;
        private SingleChiper m_chiperCmp;
#endif

        //---------------------------------------------------------------------
        //  Get / Set Secure Value
        //---------------------------------------------------------------------
        #region Get / Set Secure Value

        private float GetValue()
        {
#if NO_SECURITY
            return m_value;
#elif UNITY_EDITOR
            float value = m_chiper.GetValue();
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

        private void SetValue(float value)
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
            float v1 = m_chiper.GetValue();
            float v2 = m_chiperCmp.GetValue();
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

        public Single(float value)
        {
#if UNITY_EDITOR
            m_debugValue = value;
#endif

#if NO_SECURITY
            m_value = value;
#elif SIMPLE_SECURITY            
            m_chiper = new SingleChiper(value);
#else
            m_chiper = new SingleChiper(value);
            m_chiperCmp = new SingleChiper(value);
#endif
        }

        public float Value { get { return GetValue(); } set { SetValue(value); } }

        #endregion Get / Set Secure Value

        //---------------------------------------------------------------------
        //  Operator
        //---------------------------------------------------------------------
        #region Operator

        public static implicit operator float(Single sValue)
        {
            return sValue.GetValue();
        }

        /*
        public static implicit operator Single(float value)
        {
            Single sValue = new Single();
            sValue.SetValue(value);
            return sValue;
        }
        */

        public static Single operator ++(Single sValue)
        {
            float value = sValue.GetValue();
            value++;
            sValue.SetValue(value);
            return sValue;
        }

        public static Single operator --(Single sValue)
        {
            float value = sValue.GetValue();
            value--;
            sValue.SetValue(value);
            return sValue;
        }

        #endregion Operator

        //---------------------------------------------------------------------
        //  Implement IComparable
        //---------------------------------------------------------------------
        #region Implement IComparable
        public int CompareTo(float value)
        {
            return GetValue().CompareTo(value);
        }

        public int CompareTo(Single value)
        {
            return GetValue().CompareTo(value.GetValue());
        }

        public int CompareTo(object value)
        {
            return GetValue().CompareTo(((Single)value).GetValue());
        }

        #endregion Implement IComparable

        //---------------------------------------------------------------------
        //  Implement IEquatable
        //---------------------------------------------------------------------
        #region Implement IEquatable

        public bool Equals(float obj)
        {
            return GetValue().Equals(obj);
        }

        public bool Equals(Single obj)
        {
            return GetValue().Equals(obj.GetValue());
        }

        public override bool Equals(object obj)
        {
            return GetValue().Equals(((Single)obj).GetValue());
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
