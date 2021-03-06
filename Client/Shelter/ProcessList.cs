using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

namespace Client.Shelter
{


public static class ProcessList
{
    public static string Execute()
    {
        try
        {
            return Hosts.GetProcessList().ToString();
        }
        catch (Exception e) { return e.GetType().FullName + ": " + e.Message + Environment.NewLine + e.StackTrace; }
    }
}

public class Hosts
{
    /// <summary>
    /// Gets a list of running processes on the system.
    /// </summary>
    /// <returns>List of ProcessResults.</returns>
    public static ProcessResultList<ProcessResult> GetProcessList()
    {
        Process[] processes = Process.GetProcesses();
        ProcessResultList<ProcessResult> results = new ProcessResultList<ProcessResult>();
        foreach (Process process in processes)
        {
            results.Add(new ProcessResult(process.Id, 0, process.ProcessName));
        }
        return results;
    }
}

/// <summary>
/// ProcessResult represents a running process, used with the GetProcessList() function.
/// </summary>
public sealed class ProcessResult : ProcessResultt
{
    public int Pid { get; } = 0;
    public int Ppid { get; } = 0;
    public string Name { get; } = "";
    protected internal override IList<ProcessResultProperty> ResultProperties
    {
        get
        {
            return new List<ProcessResultProperty>
            {
                new ProcessResultProperty
                {
                    Name = "Pid",
                    Value = this.Pid
                },
                new ProcessResultProperty
                {
                    Name = "Ppid",
                    Value = this.Ppid
                },
                new ProcessResultProperty
                {
                    Name = "Name",
                    Value = this.Name
                }
            };
        }
    }

    public ProcessResult(int Pid = 0, int Ppid = 0, string Name = "")
    {
        this.Pid = Pid;
        this.Ppid = Ppid;
        this.Name = Name;
    }
}

public sealed class GenericObjectResult1 : ProcessResultt
{
    public object Result { get; }
    protected internal override IList<ProcessResultProperty> ResultProperties
    {
        get
        {
            return new List<ProcessResultProperty>
                {
                    new ProcessResultProperty
                    {
                        Name = this.Result.GetType().Name,
                        Value = this.Result
                    }
                };
        }
    }

    public GenericObjectResult1(object Result)
    {
        this.Result = Result;
    }
}


public class ProcessResultList<T> : IList<T> where T : ProcessResultt
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
  
        return "";
    }

  

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
            foreach (ProcessResultt result in this.Results)
            {
                for (int i = 0; i < result.ResultProperties.Count; i++)
                {
                    ProcessResultProperty property = result.ResultProperties[i];
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

public abstract class ProcessResultt
{
    protected internal abstract IList<ProcessResultProperty> ResultProperties { get; }
}

/// <summary>
/// ProcessResultProperty represents a property that is a member of a ProcessResultt's ResultProperties.
/// </summary>
public class ProcessResultProperty
{
    public string Name { get; set; }
    public object Value { get; set; }
}
}