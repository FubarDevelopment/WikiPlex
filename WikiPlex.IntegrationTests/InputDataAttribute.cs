using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit.Extensions;

namespace WikiPlex.IntegrationTests
{
    public class InputDataAttribute : DataAttribute
    {
        public InputDataAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            string executionDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string baseDirectory = Path.Combine(executionDirectory, "Data\\" + Name);

            return from f in Directory.GetFiles(baseDirectory, "*.wiki")
                   select new object[]
                              {
                                  f.Substring(executionDirectory.Length + 1), 
                                  f.Substring(executionDirectory.Length + 1, f.Length - (executionDirectory.Length + 1 + 5)) + ".html"
                              };
        }
    }
}