﻿using System;
using System.Collections.Generic;


namespace RayTracer;

/// <summary>
/// Represent simple command line interface parser.
/// </summary>
class CLIParser
{
    private const string namedArgumentPrefix = "--";

    private Dictionary<string, bool> settings = new();
    private int argumentCount = 0;
    private Dictionary<string, int> numericArguments = new();
    private Dictionary<string, string> stringArguments = new();

    /// <summary>
    /// Add optional named argument (--<name> <value>).
    /// </summary>
    /// <param name="name">
    /// Name of the argument. Undefined behaviour when numeric value is used as name.
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

    public string GetString(int number)
        => GetString(number.ToString());

    public string GetString(string name)
        => stringArguments[name];

    public int GetNumeric(int number)
        => GetNumeric(number.ToString());

    public int GetNumeric(string name)
        => numericArguments[name];

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
                    Console.WriteLine("Missing value for named argument!");
                    return false;
                }

                string name = args[i].Substring(namedArgumentPrefix.Length);
                if (!ParseArgument(name, args[i+1]))
                {
                    Console.WriteLine($"Could not parse argument with name \"{name}\"!");
                    return false;
                }
            }
            // parse non-named argument
            else
            {
                if (argumentNumber == argumentCount)
                {
                    Console.WriteLine("Invalud number of arguments!");
                    return false;
                }

                if (!ParseArgument(argumentNumber.ToString(), args[i]))
                {
                    Console.WriteLine($"Could not parse {argumentNumber}. argument!");
                    return false;
                }
                argumentNumber++;
            }
        }

        if (argumentNumber < argumentCount)
        {
            Console.WriteLine("Invalid number of arguments!");
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