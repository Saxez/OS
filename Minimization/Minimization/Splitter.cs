using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimization
{
    class Splitter
    {
        public Splitter(TableInfo table)
        {
            m_table = table;
        }
        public void Initialization(string[] param)
        {
            m_table.type = param[0];
            m_table.length = Int32.Parse(param[1]);
            m_table.UniqueY = Int32.Parse(param[2]);
            m_table.x = Int32.Parse(param[3]);
            m_table.y = new List<string>();
            if (m_table.type == "Mr")
            {
                Splitter.SplitMr(ref m_table, param);
            }
            else
            {
                Splitter.SplitMl(ref m_table, param);
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
                states.Add(param[i - 1].Split(' '));
            }
            List<string> concat = new List<string>();
            for (int i = 0; i < y[1].Length; i++)
            {
                List<string> name = new List<string>();
                for (int j = 0; j < y.Count; j++)
                {
                    name.Add(y[j][i]);
                }
                concat.Add(string.Join(" ", name.ToArray()));
            }
            table.y = concat;
            table.states = states;
        }

        static TableInfo m_table;
    }
}
