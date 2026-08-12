using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;

using TrickleCharge.DingOS.Core;

using UnityEngine;

namespace TrickleCharge.DingOS.Unity.Modules
{
public class LoggingModule : ICommandModule<Command>
{
    /// <inheritdoc />
    public IEnumerable<Command> GetCommands(IShellEnvironment _)
    {
        yield return DebugLog();
    }

    private enum LogLevel
    {
        Info,
        Warning,
        Error
    }

    public static Command DebugLog()
    {
        Option<LogLevel> levelOption = new("--level", "-l")
        {
            Description = "The logging level",
            Arity = ArgumentArity.ExactlyOne,
            DefaultValueFactory = static _ => LogLevel.Info,
            CustomParser = static result =>
            {
                string? token = result.Tokens.FirstOrDefault()?.Value;

                if (string.IsNullOrEmpty(token)) { return LogLevel.Info; }

                if (TryParseLogLevel(token, out LogLevel level)) { return level; }

                result.AddError($"'{token}' is not a valid log level. Expected: info, warn, error.");
                return LogLevel.Info;
            }
        };

        Argument<string[]> textArgument = new("text")
        {
            Description = "The text to log",
            Arity = ArgumentArity.OneOrMore
        };

        Command logCommand = new("debug-log")
        {
            levelOption,
            textArgument
        };
        logCommand.Description = "Log a debug message";
        logCommand.Aliases.Add("log");

        logCommand.SetAction(parseResult =>
        {
            LogLevel logLevel = parseResult.GetValue(levelOption);
            string[] textTokens = parseResult.GetValue(textArgument) ?? Array.Empty<string>();
            string logText = string.Join(" ", textTokens);

            Action<string> logAction = logLevel switch
            {
                LogLevel.Info    => Debug.Log,
                LogLevel.Warning => Debug.LogWarning,
                LogLevel.Error   => Debug.LogError,
                _                => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
            };

            logAction(logText);
        });

        return logCommand;
    }

    private static bool TryParseLogLevel(string? input, out LogLevel level)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            level = LogLevel.Info;
            return true;
        }

        if (Enum.TryParse(input, ignoreCase: true, out level))
        {
            return true;
        }

        level = input.Trim().ToLowerInvariant() switch
        {
            "i" or "inf" or "info" or "information" => LogLevel.Info,
            "w" or "warn" or "warning"              => LogLevel.Warning,
            "e" or "err" or "error"                 => LogLevel.Error,
            _                                       => (LogLevel)(-1)
        };

        return Enum.IsDefined(typeof(LogLevel), level);
    }
}
}
