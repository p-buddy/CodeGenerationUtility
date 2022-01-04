using System;

namespace pbuddy.CodeGenerationUtility.EditorScripts
{
    /// <summary>
    /// Text describing the 'guards' around a given section of text (either in a template or generated code file) 
    /// </summary>
    public readonly struct SectionIdentifiers
    {
        /// <summary>
        /// Tag / text that signals the beginning of the section
        /// </summary>
        public string SectionOpen { get; }
        
        /// <summary>
        /// Tag / text that signals the close of the section
        /// </summary>
        public string SectionClose { get; }

        public SectionIdentifiers(string sectionOpen, string sectionClose)
        {
            SectionOpen = sectionOpen;
            SectionClose = sectionClose;
        }

        public override string ToString()
        {
            return $"{SectionOpen}{Environment.NewLine}{SectionClose}";
        }
    }
}