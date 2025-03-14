﻿using AutoDarkModeSvc.Interfaces;
using System;
using AutoDarkModeConfig;
using AutoDarkModeConfig.Interfaces;

namespace AutoDarkModeSvc.SwitchComponents
{
    abstract class BaseComponent<T> : ISwitchComponent
    {
        protected NLog.Logger Logger { get; private set; }
        protected ISwitchComponentSettings<T> Settings { get; set; }
        protected ISwitchComponentSettings<T> SettingsBefore { get; set; }
        public bool Initialized { get; private set; }
        public BaseComponent()
        {
            Logger = NLog.LogManager.GetLogger(GetType().ToString());
        }
        public virtual int PriorityToLight { get; }
        public virtual int PriorityToDark { get; }
        public bool ForceSwitch { get; set; }
        public bool Enabled
        {
            get { return Settings.Enabled; }
        }
        public void Switch(Theme newTheme)
        {
            Logger.Debug($"switch invoked for {GetType().Name}");
            ForceSwitch = false;
            if (Enabled)
            {
                if (!Initialized)
                {
                    try
                    {
                        EnableHook();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, $"error while running enable hook");
                    }
                }
                if (ComponentNeedsUpdate(newTheme))
                {
                    try
                    {
                        HandleSwitch(newTheme);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, $"uncaught exception in component, source: {ex.Source}, message: ");
                    }
                }
            }
            else if (Initialized)
            {
                try
                {
                    DisableHook();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"error while running disable hook");
                }
            }
        }

        public virtual void UpdateSettingsState(object newSettings)
        {
            if (newSettings is ISwitchComponentSettings<T> temp)
            {
                SettingsBefore = Settings;
                Settings = temp;
            }
            else
            {
                Logger.Error($"could not convert generic settings object to ${typeof(T)}, no settings update performed.");
            }
        }
        public virtual void EnableHook()
        {
            Initialized = true;
        }
        public virtual void DisableHook()
        {
            Initialized = false;
        }
        /// <summary>
        /// True when the component should be compatible with the ThemeHandler switching mode
        /// </summary>
        public abstract bool ThemeHandlerCompatibility { get; }

        /// <summary>
        /// Entrypoint, called when a component needs to be updated
        /// </summary>
        /// <param name="newTheme">the new theme to apply</param>
        protected abstract void HandleSwitch(Theme newTheme);
        /// <summary>
        /// Determines whether the component needs to be triggered to update to the correct system state
        /// </summary>
        /// <returns>true if the component needs to be executed; false otherwise</returns>
        public abstract bool ComponentNeedsUpdate(Theme newTheme);
    }
}
