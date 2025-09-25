using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W2_assignment_template
{
    public class DataManager
    {
        public string[]? FileContentes { get; set; }
        private string _fileName = "input.csv";

        public DataManager()
        {
            FileContentes = System.IO.File.ReadAllLines(_fileName);
        }

       public void Read()
        {
            FileContentes = File.ReadAllLines(_fileName);
        }

        public void Write(Character character)
        {
         
            string csvName = character.Name.Contains(",") ? $"\"{character.Name}\"" : character.Name;
            string equipment = string.Join("|", character.Equipment);
            string newLine = $"{csvName},{character.Profession},{character.Level},{character.HitPoints},{equipment}";

            using (StreamWriter write = new StreamWriter(_fileName, true))
            {
                write.WriteLine(newLine);
            }
        }
    }
}
