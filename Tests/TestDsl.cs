using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    public static class TestDsl
    {
        public static T Cast<T>(this object value)
        {
            return (T)value;
        }
    }
}
