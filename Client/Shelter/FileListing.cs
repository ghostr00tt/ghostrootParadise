using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Client.Shelter
{


public class FileListing
{
    public string Execute(string Path = "")
    {
            try
            {
                return string.IsNullOrEmpty(Path.Trim()) ? Host.GetDirectoryListing().ToString() : Host.GetDirectoryListing(Path.Trim()).ToString();
            }
            catch (Exception e) { return e.GetType().FullName + ": " + e.Message + Environment.NewLine + e.StackTrace; }
        }
}

public class Host
{

    public static string GetCurrentDirectory()
    {
        return Directory.GetCurrentDirectory();
    }

  
    public static FileResultList<FileSystemEntryResult> GetDirectoryListing()
    {
        return GetDirectoryListing(GetCurrentDirectory());
    }

   
    public static FileResultList<FileSystemEntryResult> GetDirectoryListing(string Path)
    {
        FileResultList<FileSystemEntryResult> results = new FileResultList<FileSystemEntryResult>();
        foreach (string dir in Directory.GetDirectories(Path))
        {
            results.Add(new FileSystemEntryResult(dir));
        }
        foreach (string file in Directory.GetFiles(Path))
        {
            results.Add(new FileSystemEntryResult(file));
        }
        return results;
    }
}
public sealed class FileSystemEntryResult : FileResultCl
{
    public string Name { get; } = "";
    protected internal override IList<FileResultClProperty> ResultProperties
    {
        get
        {
            return new List<FileResultClProperty>
            {
                new FileResultClProperty
                {
                    Name = "Name",
                    Value = this.Name
                }
            };
        }
    }

    public FileSystemEntryResult(string Name = "")
    {
        this.Name = Name;
    }
}

public sealed class GenericObjectResult : FileResultCl
{
    public object Result { get; }
    protected internal override IList<FileResultClProperty> ResultProperties
    {
        get
        {
            return new List<FileResultClProperty>
                {
                    new FileResultClProperty
                    {
                        Name = this.Result.GetType().Name,
                        Value = this.Result
                    }
                };
        }
    }

    public GenericObjectResult(object Result)
    {
        this.Result = Result;
    }
}

public class FileResultList<T> : IList<T> where T : FileResultCl
{
    private List<T> Results { get; } = new List<T>();

    public int Count => Results.Count;
    public bool IsReadOnly => ((IList<T>)Results).IsReadOnly;


    private const int PROPERTY_SPACE = 3;

    public string FormatList()
    {
        return this.ToString();
    }

    private string FormatTable()
    {
        // TODO
        return "";
    }

   
    /// <returns>string</returns>
    public override string ToString()
    {
        if (this.Results.Count > 0)
        {
            StringBuilder builder1 = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            for (int i = 0; i < this.Results[0].ResultProperties.Count; i++)
            {
                builder1.Append(this.Results[0].ResultProperties[i].Name);
                builder2.Append(new String('-', this.Results[0].ResultProperties[i].Name.Length));
                if (i != this.Results[0].ResultProperties.Count - 1)
                {
                    builder1.Append(new String(' ', PROPERTY_SPACE));
                    builder2.Append(new String(' ', PROPERTY_SPACE));
                }
            }
            builder1.AppendLine();
            builder1.AppendLine(builder2.ToString());
            foreach (FileResultCl result in this.Results)
            {
                for (int i = 0; i < result.ResultProperties.Count; i++)
                {
                    FileResultClProperty property = result.ResultProperties[i];
                    string ValueString = property.Value.ToString();
                    builder1.Append(ValueString);
                    if (i != result.ResultProperties.Count - 1)
                    {
                        builder1.Append(new String(' ', Math.Max(1, property.Name.Length + PROPERTY_SPACE - ValueString.Length)));
                    }
                }
                builder1.AppendLine();
            }
            return builder1.ToString();
        }
        return "";
    }

    public T this[int index] { get => Results[index]; set => Results[index] = value; }

    public IEnumerator<T> GetEnumerator()
    {
        return Results.Cast<T>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Results.Cast<T>().GetEnumerator();
    }

    public int IndexOf(T item)
    {
        return Results.IndexOf(item);
    }

    public void Add(T t)
    {
        Results.Add(t);
    }

    public void AddRange(IEnumerable<T> range)
    {
        Results.AddRange(range);
    }

    public void Insert(int index, T item)
    {
        Results.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        Results.RemoveAt(index);
    }

    public void Clear()
    {
        Results.Clear();
    }

    public bool Contains(T item)
    {
        return Results.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        Results.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return Results.Remove(item);
    }
}


public abstract class FileResultCl
{
    protected internal abstract IList<FileResultClProperty> ResultProperties { get; }
}


public class FileResultClProperty
{
    public string Name { get; set; }
    public object Value { get; set; }
}
}