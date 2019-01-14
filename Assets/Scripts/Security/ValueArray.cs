using System;

namespace Security
{
    public struct ValueArray<T>
    {
        T m_v0;
        T m_v1;
        T m_v2;

        public T this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return m_v0;
                    case 1: return m_v1;
                    case 2: return m_v2;
                    default:
                        Console.WriteLine(string.Format("ValueArray invaild index({0})", index));
                        return m_v0;
                }
            }

            set
            {
                switch (index)
                {
                    case 0: m_v0 = value; break;
                    case 1: m_v1 = value; break;
                    case 2: m_v2 = value; break;
                    default:
                        Console.WriteLine(string.Format("ValueArray invaild index({0})", index));
                        m_v0 = value;
                        break;
                }
            }
        }
    }
}