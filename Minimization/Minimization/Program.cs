using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimization
{
    class Program
    {

        struct Field
        {
            public int index;
            public string name;
            public List<string> field;
        }
        struct EqualClass
        {
            public List<Field> classes;
            public List<int> indexes;
        }
        struct TableInfo
        {
            public string type;
            public int length;
            public List<string> y;
            public int UniqueY;
            public int x;
            public List<string[]> states;
        }
        static void Main()
        {
            string pathToFile = Environment.CurrentDirectory + "\\Input_5.txt";
            StreamReader sr = new StreamReader(pathToFile);
            string input = sr.ReadToEnd();
            input = input.Replace("\r", "").Replace("S", "");
            string[] param = input.Split('\n');
            TableInfo table = new TableInfo();
            Initialization(ref table, param);
            List <Field> classes = new List<Field>();
            for(int i = 0; i < table.length; i++)
            {
                Field field = new Field();
                field.field = new List<string>();
                field.name = table.y[i];
                field.index = i + 1;
                for(int j = 0; j < table.x; j++)
                {
                    field.field.Add(table.states[j][i]);
                }
                classes.Add(field);
            }
            List<string> y = GetUniqueY(table.y);
            List<EqualClass> ZeroEqual = new List<EqualClass>();
            ZeroEqual = GetZeroEqual(classes, y);
            List<EqualClass> firstIteration = new List<EqualClass>();
            List<EqualClass> secondIteration = new List<EqualClass>();
            firstIteration = Segregation(ReplaceClasses(ZeroEqual), classes);
            while (firstIteration.Count != secondIteration.Count)
            {
                secondIteration = firstIteration;
                firstIteration = Segregation(firstIteration, classes);
            }
            Console.WriteLine();
            Minimize(ref firstIteration);
            if (table.type == "Mr")
            {
                WriteToFileMr(firstIteration, classes);
            }
            else
            {
                WriteToFileMl(firstIteration, classes);
            }
            Console.WriteLine();
        }

        static void Minimize(ref List<EqualClass> notMinimize)
        {
            foreach(EqualClass equalClass in notMinimize)
            {
                while(equalClass.classes.Count > 1)
                {
                    equalClass.classes.Remove(equalClass.classes[0]);
                }
            }
        }
        static List<EqualClass> Segregation(List<EqualClass> notEqual, List<Field> classes)
        {
            List<EqualClass> segregated = new List<EqualClass>();
            foreach(EqualClass notEqualClass in notEqual)
            {
                List<string> difFields = new List<string>();
                for(int i = 0; i < notEqualClass.classes.Count; i++)
                {
                    string fieldSt = "";
                    foreach(string field in notEqualClass.classes[i].field)
                    {
                        fieldSt += field;
                    }
                    if(!difFields.Contains(fieldSt))
                    {
                        difFields.Add(fieldSt);
                    }
                }
                if(difFields.Count < 2)
                {
                    segregated.Add(notEqualClass);
                }
                else
                {
                    foreach(string name in difFields)
                    {
                        EqualClass equalClass = new EqualClass();
                        equalClass.classes = new List<Field>();
                        for (int i = 0; i < notEqualClass.classes.Count; i++)
                        {
                            string fieldSt = "";
                            foreach (string field in notEqualClass.classes[i].field)
                            {
                                fieldSt += field;
                            }
                            if (name == fieldSt)
                            {
                                equalClass.classes.Add(notEqualClass.classes[i]);
                            }
                        }
                        segregated.Add(equalClass);
                    }
                }
            }
            AddIndexes(ref segregated);
            ChangeTable(ref segregated, classes);
            return ReplaceClasses(segregated);
        }
        static void ChangeTable(ref List<EqualClass> segregated, List<Field> classes)
        {
            foreach(EqualClass segregate in segregated)
            {
                foreach(int key in segregate.indexes)
                {
                    segregate.classes.Remove(segregate.classes[0]);
                    segregate.classes.Add(classes[key-1]);
                }
            }
        }
        static void AddIndexes(ref List<EqualClass> noIndex)
        {
            List<EqualClass> withIndex = new List<EqualClass>();
            for (int j = 0; j < noIndex.Count; j++)
            {
                EqualClass classWithIndex = new EqualClass();
                classWithIndex.indexes = new List<int>();
                classWithIndex.classes = noIndex[j].classes;
                for (int i = 0; i < noIndex[j].classes.Count; i++)
                {
                    classWithIndex.indexes.Add(noIndex[j].classes[i].index);
                }
                withIndex.Add(classWithIndex);
            }
            noIndex = withIndex;
        }
        static List<string> GetUniqueY(List<string> y)
        {
            List<string> UniqueY = new List<string>();
            for(int i = 0; i < y.Count; i++)
            {
                if (!UniqueY.Contains(y[i]))
                {
                    UniqueY.Add(y[i]);
                }
            }
            return UniqueY;
        }
        static List<EqualClass> GetZeroEqual(List<Field> classes, List<string> UniqueY)
        {
            List<EqualClass> zeroEqual = new List<EqualClass>();
            foreach(string y in UniqueY)
            {
                EqualClass equalClass;
                equalClass.classes = new List<Field>();
                equalClass.indexes = new List<int>();
                for (int i = 0; i < classes.Count; i++)
                {
                    if (classes[i].name == y)
                    {
                        equalClass.classes.Add(classes[i]);
                        equalClass.indexes.Add(i+1);
                    }
                }
                zeroEqual.Add(equalClass);
            }
            return zeroEqual;
        }
        static List<EqualClass> ReplaceClasses(List<EqualClass> Equal)
        {
            List<EqualClass> replacedList = new List<EqualClass>();
            foreach (EqualClass equal in Equal)
            {
                EqualClass replacedEqual = new EqualClass();
                replacedEqual.classes = new List<Field>();
                foreach (Field field in equal.classes)
                {
                    Field replacedField = new Field();
                    replacedField.field = new List<string>();
                    replacedField.index = field.index;
                    foreach (string cell in field.field)
                    {
                        string replacedCell = ReplaceCell(cell, Equal);
                        replacedField.field.Add(replacedCell);
                    }
                    replacedEqual.classes.Add(replacedField);
                }
                replacedEqual.indexes = equal.indexes;
                replacedList.Add(replacedEqual);
            }
            return replacedList;
        }
        static string ReplaceCell(string cell, List<EqualClass> Equal)
        {
            for (int i = 0; i < Equal.Count; i++)
            {
                if(Equal[i].indexes.Contains(Int32.Parse(cell)))
                {
                    return (i+1).ToString();
                }
            }
            return "";
        }
        static void Initialization(ref TableInfo table, string[] param)
        {
            table.type = param[0];
            table.length = Int32.Parse(param[1]);
            table.UniqueY = Int32.Parse(param[2]);
            table.x = Int32.Parse(param[3]);
            table.y = new List<string>();
            if (table.type == "Mr")
            {
                SplitMr(ref table, param);
            }
            else
            {
                SplitMl(ref table, param);
            }
        }
        static void SplitMr(ref TableInfo table, string[] param)
        {
            table.y = new List<string>(param[4].Split(' '));

            table.states = new List<string[]>();
            for (int i = 5; i < param.Length; i++)
            {
                table.states.Add(param[i].Split(' '));
            }
        }
        static void SplitMl(ref TableInfo table, string[] param)
        {
            List<string[]> y = new List<string[]>();
            List<string[]> states = new List<string[]>();
            for (int i = 5; i < param.Length; i += 2)
            {
                y.Add(param[i].Split(' '));
                states.Add(param[i-1].Split(' '));
            }
            List<string> concat = new List<string>();
            for (int i = 0; i < y[1].Length; i++)
            {
                List<string> name = new List<string>();
                for(int j = 0; j < y.Count; j++)
                {
                    name.Add(y[j][i]);
                }
                concat.Add(string.Join(" ",name.ToArray()));
            }
            table.y = concat;
            table.states = states;
        }
        static void WriteToFileMr(List<EqualClass> minimized, List<Field> classes)
        {
            List<List<string>> ans = new List<List<string>>();
            for (int i = 0; i < minimized.Count; i++)
            {
                List<string> oneState = new List<string>();
                oneState.Add(classes[minimized[i].indexes[0]-1].name);
                oneState.Add("S" + (i + 1).ToString());
                for(int j = 0; j < classes[minimized[i].indexes[0] - 1].field.Count; j++)
                {
                    for(int k = 0; k < minimized.Count; k++)
                    {
                        if(minimized[k].indexes.Contains(Int32.Parse(classes[minimized[i].indexes[0] - 1].field[j])))
                        {
                            oneState.Add("S" + (k + 1).ToString());
                            break;
                        }
                    }
                }
                ans.Add(oneState);
            }
            WriteToFile(ans);
        }
        static void WriteToFileMl(List<EqualClass> minimized, List<Field> classes)
        {
            List<List<string>> ans = new List<List<string>>();
            for (int i = 0; i < minimized.Count; i++)
            {
                List<string> oneState = new List<string>();
                oneState.Add("S" + (i + 1).ToString());
                string ySt; 
                ySt = classes[minimized[i].indexes[0]-1].name;
                string[] yList = ySt.Split(' ');
                for(int j = 0; j < yList.Length; j++)
                {
                    oneState.Add("S" + minimized[i].classes[0].field[j]);
                    oneState.Add(yList[j]);
                }
                ans.Add(oneState);
            }
            WriteToFile(ans);
        }
        static void WriteToFile(List<List<string>> ans)
        {
            StreamWriter f = new StreamWriter("output.txt");
            for (int i = 0; i < ans[0].Count; i++)
            {
                string output = "";
                for (int j = 0; j < ans.Count; j++)
                {
                    output += ans[j][i] + " ";
                }
                f.WriteLine(output);
            }
            f.Close();
        }
    }
}
