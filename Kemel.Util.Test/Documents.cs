using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Util.Test
{
    public class Documents
    {
        public static string GenerateCPF()
        {
            StringBuilder stb = new StringBuilder();
            for (int i = 0; i < 9; i++)
            {
                stb.Append(Randonizer.NextInt(10));
            }
            string number = stb.ToString();
            return string.Concat(number.Insert(3, ".").Insert(7, "."), "-", GenerateCPFDigits(number));
        }

        public static string GenerateCPFDigits(string cpf)
        {
            int[] digs = new int[]{0,0};
            int start = 11;

            for (int i = 0; i < 9; i++)
            {
                int part = Convert.ToInt32(char.GetNumericValue(cpf[i]));
                digs[1] += part * start;
                start--;
                digs[0] += part * start;
            }

            digs[0] %= 11;
            if (digs[0] < 2)
            {
                digs[0] = 0;
            }
            else
            {
                digs[0] = 11 - digs[0];
            }

            digs[1] += digs[0] * start;

            digs[1] %= 11;
            if (digs[1] < 2)
            {
                digs[1] = 0;
            }
            else
            {
                digs[1] = 11 - digs[1];
            }

            return string.Concat(digs[0], digs[1]);
        }

        public static bool ValidateCPF(string cpf)
        {
            cpf = cpf.Replace(".", string.Empty).Replace("-", string.Empty);
            if (cpf.Length != 11)
                return false;

            string dig = cpf.Substring(9);
            return GenerateCPFDigits(cpf) == dig;
        }


        public static string GenerateCNPJ()
        {
            StringBuilder stb = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                stb.Append(Randonizer.NextInt(10));
            }
            string number = stb.ToString() + "0001";
            return string.Concat(number.Insert(2, ".").Insert(6, ".").Insert(10 ,"/"), "-", GenerateCNPJDigits(number));
        }

        public static string GenerateCNPJDigits(string cnpj)
        {
            int[] digs = new int[] { 0, 0 };
            int start = 6;

            for (int i = 0; i < 12; i++)
            {
                int part = Convert.ToInt32(char.GetNumericValue(cnpj[i]));
                digs[1] += part * start;
                start--;
                if (start == 1)
                    start = 9;
                digs[0] += part * start;
            }

            digs[0] %= 11;
            if (digs[0] < 2)
            {
                digs[0] = 0;
            }
            else
            {
                digs[0] = 11 - digs[0];
            }

            digs[1] += digs[0] * start;

            digs[1] %= 11;
            if (digs[1] < 2)
            {
                digs[1] = 0;
            }
            else
            {
                digs[1] = 11 - digs[1];
            }

            return string.Concat(digs[0], digs[1]);
        }

        public static bool ValidateCNPJ(string cnpj)
        {
            cnpj = cnpj.Replace(".", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty);
            if (cnpj.Length != 14)
                return false;

            string dig = cnpj.Substring(12);
            return GenerateCNPJDigits(cnpj) == dig;
        }
    }
}
