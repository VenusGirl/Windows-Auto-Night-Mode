﻿using System;

namespace AutoDarkModeApp.Handlers
{
    public class SwitchThemeException : Exception
    {
        private readonly static string customMessage = "Theme switching is unsuccessful";
        public SwitchThemeException() : base(customMessage)
        {
            this.Source = "SwitchThemeException";
        }

        public SwitchThemeException(string message, string source) : base($"{customMessage}: {message}")
        {
            this.Source = source;
        }
    }

    public class AddAutoStartException : Exception
    {
        public override string Message => "Auto start task could not been set.";

        public AddAutoStartException()
        {
            this.Source = "AutoStartException";
        }

        public AddAutoStartException(string message, string source) : base(message)
        {
            this.Source = source;
        }
    }

    public class RemoveAutoStartException : Exception
    {
        public override string Message => "Auto start task could not been removed.";

        public RemoveAutoStartException()
        {
            this.Source = "RemoveAutoStartException";
        }

        public RemoveAutoStartException(string message, string source) : base(message)
        {
            this.Source = source;
        }
    }
}
