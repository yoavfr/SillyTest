using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SillyTest
{
    [Flags]
    enum Flags
    {
        One,
        Two,
        Three
    }

    public class FlagsClass
    {
        public void Run()
        {
            Flags f = Flags.One | Flags.Two;
            Console.WriteLine(f.HasFlag(Flags.Three));
            Console.WriteLine((f & Flags.Two) == Flags.Two);
            Console.WriteLine(f.HasFlag(Flags.Two));
            f ^= Flags.Two;
            Console.WriteLine(f.HasFlag(Flags.Two));
        }
    }
}
