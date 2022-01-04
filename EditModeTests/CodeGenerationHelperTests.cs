using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

using pbuddy.CodeGenerationUtility.EditorScripts;

namespace pbuddy.CodeGenerationUtility.EditModeTests
{
    public class CodeGenerationHelperTests
    {
        private static readonly SectionIdentifiers SectionA = new SectionIdentifiers("openA", "closeA");
        private static readonly SectionIdentifiers SectionB = new SectionIdentifiers("openB", "closeB");
        private static readonly SectionIdentifiers SectionC = new SectionIdentifiers("openC", "closeC");

        private static readonly string Template = @$"
{SectionA}
{SectionB}
{SectionC}
";

        private static readonly ReadOnlyCollection<string> TemplateLines = CodeGenerationHelper
                                                                           .GetLinesArrayFromTemplate(Template)
                                                                           .ToList()
                                                                           .AsReadOnly();
        
        [Test]
        public void GetLinesArrayTest()
        {
            string[] lines = CodeGenerationHelper.GetLinesArrayFromTemplate(Template);
            
            Assert.AreEqual(lines[0], SectionA.SectionOpen);
            Assert.AreEqual(lines[1], SectionA.SectionClose);

            Assert.AreEqual(lines[2], SectionB.SectionOpen);
            Assert.AreEqual(lines[3], SectionB.SectionClose);
            
            Assert.AreEqual(lines[4], SectionC.SectionOpen);
            Assert.AreEqual(lines[5], SectionC.SectionClose);
        }
        
        [Test]
        public void GetSectionIncludingGuardsTest()
        {
            List<string> sectionLines = TemplateLines.ToList().GetSection(SectionA, false);
            Assert.AreEqual(sectionLines.Count, 2);
            Assert.AreEqual(sectionLines[0], SectionA.SectionOpen);
            Assert.AreEqual(sectionLines[1], SectionA.SectionClose);
        }
        
        [Test]
        public void GetSectionExcludingGuardsTest()
        {
            string dummyText = "dummy";
            List<string> emptySection = TemplateLines.ToList();
            List<string> nonEmptySection = TemplateLines.ToList();
            nonEmptySection.Insert(1, dummyText);

            List<string> emptySectionLines = emptySection.GetSection(SectionA); // using extension method and default parameter
            List<string> nonEmptySectionLines = CodeGenerationHelper.GetSection(nonEmptySection, SectionA, true); 
            
            Assert.AreEqual(emptySectionLines.Count, 0);
            Assert.AreEqual(nonEmptySectionLines.Count, 1);
            Assert.AreEqual(nonEmptySectionLines[0], dummyText);
        }
    }
}
