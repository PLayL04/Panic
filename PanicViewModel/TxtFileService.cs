using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panic
{
    public class TxtFileService : IFileService
    {
        public List<Discipline> Open(string fileName)
        {
            List<Discipline> disciplines = new List<Discipline>();
            using (StreamReader reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] parts = line.Split(',');
                    string str = parts[0];
                    bool flag = bool.Parse(parts[1]);
                    disciplines.Add(new Discipline(str, flag));
                }
            }
            return disciplines;
        }

        public void Save(string fileName, List<Discipline> disciplines)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (var discipline in disciplines)
                {
                    string str = discipline.Title;
                    bool flag = discipline.IsPassed;
                    writer.WriteLine($"{str},{flag}");
                }
            }
        }
    }
}
