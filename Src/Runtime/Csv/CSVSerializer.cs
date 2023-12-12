using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CSVSerializer
{
    public static List<string[]> ParseCSV(string text, char separator = ',')
    {
        List<string[]> lines = new();
        List<string> line = new();
        StringBuilder token = new();
        bool quotes = false;

        for (int i = 0; i < text.Length; i++)
        {
            if (quotes)
            {
                if ((text[i] == '\\' && i + 1 < text.Length && text[i + 1] == '\"') || (text[i] == '\"' && i + 1 < text.Length && text[i + 1] == '\"'))
                {
                    _ = token.Append('\"');
                    i++;
                }
                else if (text[i] == '\\' && i + 1 < text.Length && text[i + 1] == 'n')
                {
                    _ = token.Append('\n');
                    i++;
                }
                else if (text[i] == '\"')
                {
                    line.Add(ValidString(token));
                    token = new StringBuilder();
                    quotes = false;
                    if (i + 1 < text.Length && text[i + 1] == separator)
                    {
                        i++;
                    }
                }
                else
                {
                    _ = token.Append(text[i]);
                }
            }
            else if (text[i] is '\r' or '\n')
            {
                if (i - 1 >= 0 && text[i - 1] == separator)
                {
                    line.Add(ValidString(token));
                    token = new StringBuilder();
                }
                else if (token.Length > 0)
                {
                    line.Add(ValidString(token));
                    token = new StringBuilder();
                }

                if (line.Count > 0)
                {
                    lines.Add(line.ToArray());
                    line.Clear();
                }
            }
            else if (text[i] == separator)
            {
                line.Add(ValidString(token));
                token = new StringBuilder();
            }
            else if (text[i] == '\"')
            {
                quotes = true;
            }
            else
            {
                _ = token.Append(text[i]);
            }
        }

        if (token.Length > 0 || text[^1] == separator)
        {
            line.Add(ValidString(token));
        }
        if (line.Count > 0)
        {
            lines.Add(line.ToArray());
        }
        return lines;
    }
    public static string[] ParseCSVRow(string text)
    {
        List<string> rows = new();
        StringBuilder stringBuilder = new();
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] is '\r' or '\n')
            {
                if (stringBuilder.Length > 0)
                {
                    rows.Add(stringBuilder.ToString());
                    _ = stringBuilder.Clear();
                }
            }
            else if (text[i] == '\"')
            {
                _ = stringBuilder.Append(text[i]);
                while (i + 1 < text.Length)
                {
                    i++;
                    _ = stringBuilder.Append(text[i]);
                    if ((text[i] == '\\' && i + 1 < text.Length && text[i + 1] == '\"') || (text[i] == '\"' && i + 1 < text.Length && text[i + 1] == '\"'))
                    {
                        i++;
                        _ = stringBuilder.Append(text[i]);
                    }
                    else if (text[i] == '\"')
                    {
                        break;
                    }
                }
            }
            else
            {
                _ = stringBuilder.Append(text[i]);
            }
        }

        if (stringBuilder.Length > 0)
        {
            rows.Add(stringBuilder.ToString());
        }
        return rows.ToArray();
    }

    public static string[] ParseCSVCol(string text, char separator = ',')
    {
        List<string> cols = new();
        StringBuilder stringBuilder = new();
        bool quotes = false;

        for (int i = 0; i < text.Length; i++)
        {
            if (quotes)
            {
                if ((text[i] == '\\' && i + 1 < text.Length && text[i + 1] == '\"') || (text[i] == '\"' && i + 1 < text.Length && text[i + 1] == '\"'))
                {
                    _ = stringBuilder.Append('\"');
                    i++;
                }
                else if (text[i] == '\\' && i + 1 < text.Length && text[i + 1] == 'n')
                {
                    _ = stringBuilder.Append('\n');
                    i++;
                }
                else if (text[i] == '\"')
                {
                    cols.Add(ValidString(stringBuilder));
                    _ = stringBuilder.Clear();
                    quotes = false;
                    if (i + 1 < text.Length && text[i + 1] == separator)
                    {
                        i++;
                    }
                }
                else
                {
                    _ = stringBuilder.Append(text[i]);
                }
            }
            else if (text[i] == separator)
            {
                cols.Add(ValidString(stringBuilder));
                _ = stringBuilder.Clear();
            }
            else if (text[i] == '\"')
            {
                quotes = true;
            }
            else
            {
                _ = stringBuilder.Append(text[i]);
            }
        }
        if (stringBuilder.Length > 0 || text[^1] == separator)
        {
            cols.Add(ValidString(stringBuilder));
        }
        return cols.ToArray();
    }

    public static string ValidString(StringBuilder stringBuilder)
    {
        string value = stringBuilder.ToString();
        if (value == "-")
        {
            value = "";
        }
        return value;
    }
}