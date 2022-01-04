namespace pbuddy.CodeGenerationUtility.EditorScripts
{
    /// <summary>
    /// ... describe ...
    /// In contrast to <see cref="SectionIdentifiers"/>, this structure represents text that should be 'swapped out' when
    /// generating code. 
    /// </summary>
    public readonly struct TemplateToReplace
    {
        /// <summary>
        /// A string of text in a template that is intended to be replaced dynamically (using the <see cref="ReplacementString"/>)
        /// </summary>
        public string TemplateString { get; }
        
        /// <summary>
        /// The string of text that will replace the <see cref="TemplateString"/>
        /// </summary>
        public string ReplacementString { get; }

        public TemplateToReplace(string templateString, string replacementString)
        {
            TemplateString = templateString;
            ReplacementString = replacementString;
        }
    }
}