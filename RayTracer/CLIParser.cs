using System.Collections.Generic;


namespace RayTracer;
/// <summary>
/// Represent simple command line interface parser.
/// </summary>
class CLIParser
{
    private const string namedArgumentPrefix = "--";

    private readonly Dictionary<string, bool> settings = new();
    private readonly Dictionary<string, int> numericArguments = new();
    private readonly Dictionary<string, string> stringArguments = new();
    
    private int argumentCount = 0;

    /// <summary>
    /// Add optional named argument (--<name> <value>).
    /// </summary>
    /// <param name="name">
    /// Name of the argument. Undefined behavior when numeric value is used as name.
    /// </param>
    /// <param name="numeric">Determine if argument should be parsed as numeric value.</param>
    public void AddNamedArgument(string name, bool numeric)
        => settings.Add(name, numeric);

    /// <summary>
    /// Add argument. The order of arguments is determined by order of <see cref="AddArgument(bool)"/>
    /// calls.
    /// </summary>
    /// <param name="numeric">Determine if argument should be parsed as numeric value.</param>
    public void AddArgument(bool numeric)
    {
        AddNamedArgument(argumentCount.ToString(), numeric);
        argumentCount++;
    }

    #region Arguments query methods
    public string GetString(int number)
        => GetString(number.ToString());

    public string GetString(string name, string failValue = null)
    {
        if (!stringArguments.ContainsKey(name))
            return failValue;

        return stringArguments[name];
    }

    public int GetNumeric(int number)
        => GetNumeric(number.ToString());

    public int GetNumeric(string name, int failValue = -1)
    {
        if (!numericArguments.ContainsKey(name))
            return failValue;

        return numericArguments[name];
    }

    public bool HasString(int number)
    => HasString(number.ToString());

    public bool HasString(string name)
        => stringArguments.ContainsKey(name);

    public bool HasNumeric(int number)
        => HasNumeric(number.ToString());

    public bool HasNumeric(string name)
        => numericArguments.ContainsKey(name);
    #endregion

    public bool Parse(string[] args)
    {
        // used for tracking non-named arguments
        int argumentNumber = 0;

        for (int i = 0; i < args.Length; i++)
        {
            // parse named argument
            if (args[i].StartsWith(namedArgumentPrefix))
            {
                if (args.Length <= i + 1)
                {
                    Logger.WriteLine("Missing value for named argument!", LogType.Error);
                    return false;
                }

                string name = args[i][namedArgumentPrefix.Length..];
                if (!ParseArgument(name, args[i+1]))
                {
                    Logger.WriteLine($"Could not parse argument with name \"{name}\"!", LogType.Error);
                    return false;
                }
                i++;
            }
            // parse non-named argument
            else
            {
                if (argumentNumber == argumentCount)
                {
                    Logger.WriteLine("Invalid number of arguments!", LogType.Error);
                    return false;
                }

                if (!ParseArgument(argumentNumber.ToString(), args[i]))
                {
                    Logger.WriteLine($"Could not parse {argumentNumber}. argument!", LogType.Error);
                    return false;
                }
                argumentNumber++;
            }
        }

        if (argumentNumber < argumentCount)
        {
            Logger.WriteLine("Invalid number of arguments!", LogType.Error);
            return false;
        }

        return true;
    }

    private bool ParseArgument(string name, string argument)
    {
        // parse numeric argument
        if (settings[name])
        {
            if (!int.TryParse(argument, out int value))
                return false;

            numericArguments.Add(name, value);
        }
        // parse string argument
        else
        {
            stringArguments.Add(name, argument);
        }

        return true;
    }
}
