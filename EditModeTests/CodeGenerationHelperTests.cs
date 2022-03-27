using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

using pbuddy.CodeGenerationUtility.EditorScripts;
using pbuddy.TestsAsDocumentationUtility.EditorScripts;

namespace pbuddy.CodeGenerationUtility.EditModeTests
{
    public class CodeGenerationHelperTests /*: TestsAsDocumentationBase*/
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
        [Demonstrates(typeof(CodeGenerationHelper), nameof(CodeGenerationHelper.GetLinesArrayFromTemplate))]
        public void GetLinesArrayTest()
        {
            // Given
            string line0 = "Hello,";
            string line1 = "This is an example.";
            string line2 = "goodbye";
            
            string template = @$"
{line0}

{line1}

{line2}";
            
            // When
            string[] lines = CodeGenerationHelper.GetLinesArrayFromTemplate(template);
            
            // Then
            Assert.AreEqual(lines[0], line0);
            Assert.AreEqual(lines[1], line1);
            Assert.AreEqual(lines[2], line2);
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
