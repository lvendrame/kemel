using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Util.Test
{
    public class Telephone
    {
        public static int[][] DDDs = new int[][] {
            new int[]{68},
            new int[]{82},
            new int[]{96},
            new int[]{92, 97},
            new int[]{71, 73, 74, 75, 77},
            new int[]{85, 88},
            new int[]{61},
            new int[]{27,28},
            new int[]{62,64},
            new int[]{98,99},
            new int[]{65,66},
            new int[]{67},
            new int[]{31, 32, 33, 34, 35, 37, 38},
            new int[]{91,93,94},
            new int[]{83},
            new int[]{41, 42, 43, 44, 45, 46},
            new int[]{81,87},
            new int[]{86,89},
            new int[]{21,22,24},
            new int[]{84},
            new int[]{51,53,54,55},
            new int[]{69},
            new int[]{95},
            new int[]{47,48,49},
            new int[]{11, 12, 13, 14, 15, 16, 17, 18, 19},
            new int[]{79},
            new int[]{63},
        };

        public static int GetAnyDDD()
        {
            int[] vddd = DDDs[Randonizer.NextInt(28)];
            return vddd[Randonizer.NextInt(vddd.Length)];
        }

        public static int GetAnyDDDFromState(States state)
        {
            int[] vddd = DDDs[(int)state];
            return vddd[Randonizer.NextInt(vddd.Length)];
        }

        public static string GenerateTelephoneNumber()
        {
            StringBuilder stb = new StringBuilder(9);
            stb.Append(Randonizer.NextInt(2, 7));
            for (int i = 0; i < 7; i++)
            {
                stb.Append(Randonizer.NextInt(10));
            }
            return stb.Insert(4, "-").ToString();
        }

        public static string GenerateCellphoneNumber(int ddd)
        {
            StringBuilder stb = new StringBuilder(10);
            if (ddd == 11)
                stb.Append(9);

            stb.Append(Randonizer.NextInt(7, 10));
            for (int i = 0; i < 7; i++)
            {
                stb.Append(Randonizer.NextInt(10));
            }
            return stb.Insert(ddd == 11 ? 5 : 4, "-").ToString();
        }

        public static string GenerateCompleteTelephoneNumber()
        {
            return string.Concat("(", GetAnyDDD(), ") ",
                GenerateTelephoneNumber());
        }

        public static string GenerateCompleteTelephoneNumberFromState(States state)
        {
            return string.Concat("(", GetAnyDDDFromState(state), ") ",
                GenerateTelephoneNumber());
        }

        public static string GenerateCompleteTelephoneNumberFromDDD(int ddd)
        {
            return string.Concat("(", ddd, ") ",
                GenerateTelephoneNumber());
        }

        public static string GenerateCompleteCellphoneNumber()
        {
            int ddd = GetAnyDDD();
            return string.Concat("(", ddd, ") ",
                GenerateCellphoneNumber(ddd));
        }

        public static string GenerateCompleteCellphoneNumberFromState(States state)
        {
            int ddd = GetAnyDDDFromState(state);
            return string.Concat("(", ddd, ") ",
                GenerateCellphoneNumber(ddd));
        }

        public static string GenerateCompleteCellphoneNumberFromDDD(int ddd)
        {
            return string.Concat("(", ddd, ") ",
                GenerateCellphoneNumber(ddd));
        }
    }
}
