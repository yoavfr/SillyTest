using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SillyTest
{
    public class Enums
    {
        public void Run()
        {
            AEnum a = AEnum.Two;
            BEnum b = (BEnum)a;
            Console.WriteLine(b);
        }

        public enum AEnum
        {
            One,
            Two,
            Three
        }

        public enum BEnum
        {
            One,
            Two,
            Three
        }
    }
}
