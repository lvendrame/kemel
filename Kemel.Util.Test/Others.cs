using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Util.Test
{
    public class Others
    {
        public static string[] hosts = new string[]{
            "yahoo.com", "gmail.com", "microsoft.com", "outlook.com",
            "uol.com.br", "ig.com.br", "terra.com.br"
        };

        public const string LoremIpsum_p01 = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
        public const string LoremIpsum_p02 = "Curabitur pretium tincidunt lacus. Nulla gravida orci a odio. Nullam varius, turpis et commodo pharetra, est eros bibendum elit, nec luctus magna felis sollicitudin mauris. Integer in mauris eu nibh euismod gravida. Duis ac tellus et risus vulputate vehicula. Donec lobortis risus a elit. Etiam tempor. Ut ullamcorper, ligula eu tempor congue, eros est euismod turpis, id tincidunt sapien risus a quam. Maecenas fermentum consequat mi. Donec fermentum. Pellentesque malesuada nulla a mi. Duis sapien sem, aliquet nec, commodo eget, consequat quis, neque. Aliquam faucibus, elit ut dictum aliquet, felis nisl adipiscing sapien, sed malesuada diam lacus eget erat. Cras mollis scelerisque nunc. Nullam arcu. Aliquam consequat. Curabitur augue lorem, dapibus quis, laoreet et, pretium ac, nisi. Aenean magna nisl, mollis quis, molestie eu, feugiat in, orci. In hac habitasse platea dictumst.";
        public const string LoremIpsum_p03 = "Fusce convallis, mauris imperdiet gravida bibendum, nisl turpis suscipit mauris, sed placerat ipsum urna sed risus. In convallis tellus a mauris. Curabitur non elit ut libero tristique sodales. Mauris a lacus. Donec mattis semper leo. In hac habitasse platea dictumst. Vivamus facilisis diam at odio. Mauris dictum, nisi eget consequat elementum, lacus ligula molestie metus, non feugiat orci magna ac sem. Donec turpis. Donec vitae metus. Morbi tristique neque eu mauris. Quisque gravida ipsum non sapien. Proin turpis lacus, scelerisque vitae, elementum at, lobortis ac, quam. Aliquam dictum eleifend risus. In hac habitasse platea dictumst. Etiam sit amet diam. Suspendisse odio. Suspendisse nunc. In semper bibendum libero.";
        public const string LoremIpsum_p04 = "Proin nonummy, lacus eget pulvinar lacinia, pede felis dignissim leo, vitae tristique magna lacus sit amet eros. Nullam ornare. Praesent odio ligula, dapibus sed, tincidunt eget, dictum ac, nibh. Nam quis lacus. Nunc eleifend molestie velit. Morbi lobortis quam eu velit. Donec euismod vestibulum massa. Donec non lectus. Aliquam commodo lacus sit amet nulla. Cras dignissim elit et augue. Nullam non diam. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. In hac habitasse platea dictumst. Aenean vestibulum. Sed lobortis elit quis lectus. Nunc sed lacus at augue bibendum dapibus.";
        public const string LoremIpsum_p05 = "Aliquam vehicula sem ut pede. Cras purus lectus, egestas eu, vehicula at, imperdiet sed, nibh. Morbi consectetuer luctus felis. Donec vitae nisi. Aliquam tincidunt feugiat elit. Duis sed elit ut turpis ullamcorper feugiat. Praesent pretium, mauris sed fermentum hendrerit, nulla lorem iaculis magna, pulvinar scelerisque urna tellus a justo. Suspendisse pulvinar massa in metus. Duis quis quam. Proin justo. Curabitur ac sapien. Nam erat. Praesent ut quam.";
        public const string LoremIpsum_p06 = "Vivamus commodo, augue et laoreet euismod, sem sapien tempor dolor, ac egestas sem ligula quis lacus. Donec vestibulum tortor ac lacus. Sed posuere vestibulum nisl. Curabitur eleifend fermentum justo. Nullam imperdiet. Integer sit amet mauris imperdiet risus sollicitudin rutrum. Ut vitae turpis. Nulla facilisi. Quisque tortor velit, scelerisque et, facilisis vel, tempor sed, urna. Vivamus nulla elit, vestibulum eget, semper et, scelerisque eget, lacus. Pellentesque viverra purus. Quisque elit. Donec ut dolor.";
        public const string LoremIpsum_p07 = "Duis volutpat elit et erat. In at nulla at nisl condimentum aliquet. Quisque elementum pharetra lacus. Nunc gravida arcu eget nunc. Nulla iaculis egestas magna. Aliquam erat volutpat. Sed pellentesque orci. Etiam lacus lorem, iaculis sit amet, pharetra quis, imperdiet sit amet, lectus. Integer quis elit ac mi aliquam pretium. Nullam mauris orci, porttitor eget, sollicitudin non, vulputate id, risus. Donec varius enim nec sem. Nam aliquam lacinia enim. Quisque eget lorem eu purus dignissim ultricies. Fusce porttitor hendrerit ante. Mauris urna diam, cursus id, mattis eget, tempus sit amet, risus. Curabitur eu felis. Sed eu mi. Nullam lectus mauris, luctus a, mattis ac, tempus non, leo. Cras mi nulla, rhoncus id, laoreet ut, ultricies id, odio.";
        public const string LoremIpsum_p08 = "Donec imperdiet. Vestibulum auctor tortor at orci. Integer semper, nisi eget suscipit eleifend, erat nisl hendrerit justo, eget vestibulum lorem justo ac leo. Integer sem velit, pharetra in, fringilla eu, fermentum id, felis. Vestibulum sed felis. In elit. Praesent et pede vel ante dapibus condimentum. Donec magna. Quisque id risus. Mauris vulputate pellentesque leo. Duis vulputate, ligula at venenatis tincidunt, orci nunc interdum leo, ac egestas elit sem ut lacus. Etiam non diam quis arcu egestas commodo. Curabitur nec massa ac massa gravida condimentum. Aenean id libero. Pellentesque vitae tellus. Fusce lectus est, accumsan ac, bibendum sed, porta eget, augue. Etiam faucibus. Quisque tempus purus eu ante.";
        public const string LoremIpsum_p09 = "Vestibulum sapien nisl, ornare auctor, consectetuer quis, posuere tristique, odio. Fusce ultrices ullamcorper odio. Ut augue nulla, interdum at, adipiscing non, tristique eget, neque. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Ut pede est, condimentum id, scelerisque ac, malesuada non, quam. Proin eu ligula ac sapien suscipit blandit. Suspendisse euismod. Ut accumsan, neque id gravida luctus, arcu pede sodales felis, vel blandit massa arcu eget ligula. Aenean sed turpis. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Donec sem eros, ornare ut, commodo eu, tempor nec, risus. Donec laoreet dapibus ligula. Praesent orci leo, bibendum nec, ornare et, nonummy in, elit. Donec interdum feugiat leo. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Pellentesque feugiat ullamcorper ipsum. Donec convallis tincidunt urna.";
        public const string LoremIpsum_p10 = "Suspendisse et orci et arcu porttitor pellentesque. Sed lacus nunc, fermentum vel, vehicula in, imperdiet eget, urna. Nam consectetuer euismod nunc. Nulla dignissim posuere nulla. Integer iaculis lacinia massa. Nullam sapien augue, condimentum vel, venenatis id, rhoncus pellentesque, sapien. Donec sed ipsum ultrices turpis consectetuer imperdiet. Duis et ipsum ac nisl laoreet commodo. Mauris eu est. Suspendisse id turpis quis orci euismod consequat. Donec tellus mi, luctus sit amet, ultrices a, convallis eu, lorem. Proin faucibus convallis elit. Maecenas rhoncus arcu at arcu. Proin libero. Proin adipiscing. In quis lorem vitae elit consectetuer pretium. Nullam ligula urna, adipiscing nec, iaculis ut, elementum non, turpis. Fusce pulvinar.";

        public static string GenerateEmail()
        {
            return string.Concat(Names.GetFirstName(1).ToLower(), ".", Names.GetLastName(1).ToLower(), "@", hosts[Randonizer.NextInt(hosts.Length)]);
        }

        public static string GenerateUrl()
        {
            return string.Concat("http://www.", hosts[Randonizer.NextInt(hosts.Length)]);
        }

        public static DateTime GenerateDate()
        {
            return DateTime.Today.AddDays(-Randonizer.NextInt(365, 35000));
        }

        public static string GenerateLoremIpsum(int length)
        {
            int start = Randonizer.NextInt(1, 11);
            string lorem = string.Empty;

            StringBuilder stb = new StringBuilder();
            while (stb.Length <= length)
            {
                lorem = GetLoremIpsum(start);
                stb.AppendLine(lorem);

                start++;
                if (start > 10)
                    start = 1;
            }
            return stb.ToString(0, length);
        }

        public static string GenerateLoremIpsumParagraphs(int paragraphs)
        {
            int start = Randonizer.NextInt(1, 11);
            string lorem = string.Empty;

            StringBuilder stb = new StringBuilder();
            for (int i = 0; i < paragraphs; i++)
            {
                lorem = GetLoremIpsum(start);
                stb.AppendLine(lorem);

                start++;
                if (start > 10)
                    start = 1;

            }
            return stb.ToString();
        }

        public static string GenerateLoremIpsumParagraphs(int paragraphs, int maxParagraphLength)
        {
            int start = Randonizer.NextInt(1, 11);
            string lorem = string.Empty;

            StringBuilder stb = new StringBuilder();
            for (int i = 0; i < paragraphs; i++)
            {
                lorem = GetLoremIpsum(start);
                stb.AppendLine(lorem.Length <= maxParagraphLength? lorem : lorem.Substring(0, maxParagraphLength));

                start++;
                if (start > 10)
                    start = 1;

            }
            return stb.ToString();
        }

        public static string GetLoremIpsum(int num)
        {
            switch (num)
            {
                case 1: return LoremIpsum_p01;
                case 2: return LoremIpsum_p02;
                case 3: return LoremIpsum_p03;
                case 4: return LoremIpsum_p04;
                case 5: return LoremIpsum_p05;
                case 6: return LoremIpsum_p06;
                case 7: return LoremIpsum_p07;
                case 8: return LoremIpsum_p08;
                case 9: return LoremIpsum_p09;
                default: return LoremIpsum_p10;
            }
        }

        public static double GenerateNumber(int baseLength)
        {
            return Randonizer.NextDouble(baseLength);
        }
    }
}
