using System;
using System.Collections.Generic;

namespace pbuddy.CodeGenerationUtility.EditorScripts
{
    /// <summary>
    /// 
    /// </summary>
    public static class CodeGenerationHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string[] GetStringArrayFromTemplate(string template)
        {
            return template.Split(new[]
                           {
                               Environment.NewLine
                           },
                           StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="sectionIdentifiers"></param>
        /// <param name="excludeSectionGuards"></param>
        /// <returns></returns>
        public static List<string> GetSection(this List<string> lines,
                                              in SectionIdentifiers sectionIdentifiers,
                                              bool excludeSectionGuards = true)
        {
            List<string> sectionLines = new List<string>(lines.Count);
            bool inSection = false;
            foreach (string line in lines)
            {
                if (line.Contains(sectionIdentifiers.SectionClose))
                {
                    inSection = false;
                    if (!excludeSectionGuards)
                    {
                        sectionLines.Add(line);
                    }
                }
                
                if (inSection)
                {
                    sectionLines.Add(line);
                }
                
                if (line.Contains(sectionIdentifiers.SectionOpen))
                {
                    inSection = true;
                    if (!excludeSectionGuards)
                    {
                        sectionLines.Add(line);
                    }
                }
            }

            return sectionLines;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="sectionIdentifiers"></param>
        public static void RemoveSection(this List<string> lines, in SectionIdentifiers sectionIdentifiers)
        {
            bool insideSectionToDelete = false;
            for (var index = lines.Count - 1; index >= 0; index--)
            {
                if (insideSectionToDelete)
                {
                    insideSectionToDelete = !lines[index].Contains(sectionIdentifiers.SectionOpen);
                    lines.RemoveAt(index);
                    continue;
                }

                if (lines[index].Contains(sectionIdentifiers.SectionClose))
                {
                    lines.RemoveAt(index);
                    insideSectionToDelete = true;
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="sectionIdentifiers"></param>
        public static void RemoveSectionIdentifiers(this List<string> lines, in SectionIdentifiers sectionIdentifiers)
        {
            for (var index = lines.Count - 1; index >= 0; index--)
            {
                if (lines[index].Contains(sectionIdentifiers.SectionClose) || lines[index].Contains(sectionIdentifiers.SectionOpen))
                {
                    lines.RemoveAt(index);
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="sectionIdentifiers"></param>
        /// <param name="replacementString"></param>
        public static void ReplaceSectionIdentifiers(this List<string> lines, in SectionIdentifiers sectionIdentifiers, string replacementString)
        {
            for (var index = lines.Count - 1; index >= 0; index--)
            {
                if (lines[index].Contains(sectionIdentifiers.SectionClose) || lines[index].Contains(sectionIdentifiers.SectionOpen))
                {
                    lines[index] = replacementString;
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="sectionIdentifiers"></param>
        /// <param name="toAdd"></param>
        public static void AddToBeginningOfSection(this List<string> lines, in SectionIdentifiers sectionIdentifiers, params string[] toAdd)
        {
            for (var index = 0; index < lines.Count; index++)
            {
                if (lines[index].Contains(sectionIdentifiers.SectionOpen))
                {
                    for (int toAddIndex = toAdd.Length - 1; toAddIndex >= 0; toAddIndex--)
                    {
                        lines.Insert(index + 1, toAdd[toAddIndex]);
                    }

                    index += toAdd.Length;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="sectionIdentifiers"></param>
        /// <param name="toAdd"></param>
        public static void AddToEndOfSection(this List<string> lines, in SectionIdentifiers sectionIdentifiers, params string[] toAdd)
        {
            bool insideSection = false;
            string spacing = null;

            for (int index = 0; index < lines.Count; index++)
            {
                if (insideSection)
                {
                    if (lines[index].Contains(sectionIdentifiers.SectionClose))
                    {
                        for (int toAddIndex = toAdd.Length - 1; toAddIndex >= 0; toAddIndex--)
                        {
                            lines.Insert(index, $"{spacing}{toAdd[toAddIndex]}");
                        }

                        index += toAdd.Length;
                        insideSection = false;
                    }
                }
                
                if (lines[index].Contains(sectionIdentifiers.SectionOpen))
                {
                    spacing = GetLeadingWhitespace(lines[index]);
                    insideSection = true;
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="replacements"></param>
        public static void ReplaceTemplates(this List<string> lines, params TemplateToReplace[] replacements)
        {
            for (var index = 0; index < lines.Count; index++)
            {
                foreach (TemplateToReplace replacement in replacements)
                {
                    lines[index] = lines[index].Replace(replacement.TemplateString, replacement.ReplacementString);
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        public static void RemoveMultipleEmptyLines(this List<string> lines)
        {
            bool previousWasEmpty = false;
            for (int index = lines.Count - 1; index >= 0; index--)
            {
                bool isEmpty = lines[index] == String.Empty;
                if (isEmpty && previousWasEmpty)
                {
                    lines.RemoveAt(index);
                }
                previousWasEmpty = isEmpty;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetLeadingWhitespace(string str)
        {
            return str.Replace(str.TrimStart(), "");
        }
    }
}