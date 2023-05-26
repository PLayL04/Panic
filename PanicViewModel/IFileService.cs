using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panic
{
    public interface IFileService
    {
        List<Discipline> Open(string fileName);
        void Save(string fileName, List<Discipline> disciplines);
    }
}
