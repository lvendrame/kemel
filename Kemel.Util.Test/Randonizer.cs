using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Util.Test
{
    public class Randonizer
    {

        private static Random rdInstance = null;

        private static Random Instance {
            get {
                if (rdInstance == null)
                    rdInstance = new Random(DateTime.Now.Millisecond);

                return rdInstance;
            }
        }

        public static int NextInt()
        {
            return Instance.Next();
        }
        public static int NextInt(int max)
        {
            return Instance.Next(max);
        }
        public static int NextInt(int min, int max)
        {
            return Instance.Next(min, max);
        }

        internal static double NextDouble(int baseLength)
        {
            return Instance.NextDouble() * (double)baseLength;
        }
    }
}
