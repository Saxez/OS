using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimization
{
    class Printer
    {
        public void SelectOutType(string type, List<EqualClass> minimized, InputClasses allClasses)
        {
            if (type == "Mr")
            {
                WriteToFileMr(minimized, allClasses.classes);
            }
            else
            {
                WriteToFileMl(minimized, allClasses.classes);
            }
        }
        static void WriteToFileMr(List<EqualClass> minimized, List<Field> classes)
        {
            List<List<string>> ans = new List<List<string>>();
            for (int i = 0; i < minimized.Count; i++)
            {
                List<string> oneState = new List<string>();
                oneState.Add(classes[minimized[i].indexes[0] - 1].name);
                oneState.Add("S" + (i + 1).ToString());
                for (int j = 0; j < classes[minimized[i].indexes[0] - 1].field.Count; j++)
                {
                    for (int k = 0; k < minimized.Count; k++)
                    {
                        if (minimized[k].indexes.Contains(Int32.Parse(classes[minimized[i].indexes[0] - 1].field[j])))
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
                ySt = classes[minimized[i].indexes[0] - 1].name;
                string[] yList = ySt.Split(' ');
                for (int j = 0; j < yList.Length; j++)
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
