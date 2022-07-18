using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;


namespace SyntaxVisitors
{
    public class TeacherContolConverter : WalkingVisitorNew
    {
        public static TeacherContolConverter New
        {
            get { return new TeacherContolConverter(); }
        }

        public string Name { get => "TeacherContolConverter"; }
        public syntax_tree_node Convert(syntax_tree_node root)
        {
            try
            {
                var program = root as program_module;
                if (program == null) // если это не главная программа, то не преобразовывать
                    return root;
                var FileName = program.file_name;
                var fi = new System.IO.FileInfo(FileName);
                var SourceFileDirectory = fi.DirectoryName;

                var FullLightPTName = System.IO.Path.Combine(SourceFileDirectory, "lightpt.dat");
                if (!System.IO.File.Exists(FullLightPTName))
                    return root;

                if (program.used_units == null)
                    program.used_units = new uses_list();
                program.used_units.Add(new unit_or_namespace("LightPT", null));
                program.used_units.Add(new unit_or_namespace("Tasks", null));
            }
            catch
            {

            }
            return root;
        }
    }

}
