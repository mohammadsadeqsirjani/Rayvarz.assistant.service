using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rayvarz.invHServ.Library.Tools.Security
{
    public enum Mode
    {
        NumbersOnly,
        AlphebetsOnly,
        Mix
    }
    public class OTPGenerator
    {
        public static string Create(int length = 5, Mode mode = Mode.NumbersOnly)
        {
            const string alfs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string nums = "0123456789";
            string chars = "";
            switch (mode)
            {
                case Mode.AlphebetsOnly:
                    chars = alfs;
                    break;
                case Mode.NumbersOnly:
                    chars = nums;
                    break;
                default:
                    chars = alfs + nums;
                    break;
            }
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
