using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.DeathInformationList
{
    public class GenericTest<T>
    {
        public void AddIT(T item)
        { }
    }

    public class Test
    {
        public static void Main()
        {
            GenericTest<string> textClass = new GenericTest<string>();
            textClass.AddIT("Hello world");

            GenericTest<int> numClass = new GenericTest<int>();
            numClass.AddIT(22);

            GenericTest<object> objClass = new GenericTest<object>();
            objClass.AddIT(new object());
        }
    }
}
