using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimization
{
    class InputClasses
    {
        public List<Field> classes;
        public void GetInputClasses(TableInfo table)
        {
            classes = new List<Field>();
            for (int i = 0; i < table.length; i++)
            {
                Field field = new Field();
                field.field = new List<string>();
                field.name = table.y[i];
                field.index = i + 1;
                for (int j = 0; j < table.x; j++)
                {
                    field.field.Add(table.states[j][i]);
                }
                classes.Add(field);
            }
        }
    }
}
