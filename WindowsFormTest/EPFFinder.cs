using System.IO;
using System.Threading.Tasks;

namespace WindowsFormTest
{
    class EPFFinder
    {
        public static FileInfo[] EPFFiles
        {
            get
            {
                var files = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("*.epf");
                return files;
            }
        }
    }
}
