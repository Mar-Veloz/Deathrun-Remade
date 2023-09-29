using System;
using DeathrunRemade.Configuration;
using Nautilus.Json;

namespace DeathrunRemade.Objects
{
    /// <summary>
    /// The cache that keeps track of everything happening in an ongoing game. It enables the mod to load properly
    /// after saving and quitting.
    /// </summary>
    internal class SaveData : SaveDataCache
    {
        [NonSerialized]
        public static SaveData Main;
        [NonSerialized]
        public bool Ready;
        
        /// <summary>
        /// Invoked when the save cache has finished initialising and/or loading data from disk.
        /// </summary>
        public static event Action<SaveData> OnSaveDataLoaded;

        public ConfigSave Config;
        public EscapePodSave EscapePod;
        public NitrogenSave Nitrogen;
        public RunStats Stats;
        public TutorialSave Tutorials;
        public WarningSave Warnings;
        
        public override void Load(bool createFileIfNotExist = true)
        {
            base.Load(createFileIfNotExist);
            // The save data loads on game start. If the config data has not been set already, lock it in.
            if (!Config.WasInitialised)
            {
                Config = new ConfigSave(DeathrunInit._Config);
                DeathrunInit._RunHandler.StartNewRun(this);
            }

            // Once the file has completed loading/creation, notify everything waiting on it.
            Ready = true;
            DeathrunInit._Log.Debug("Save data is ready.");
            OnSaveDataLoaded?.Invoke(this);
        }
    }

    [Serializable]
    internal struct EscapePodSave
    {
        public bool isAnchored;
    }

    [Serializable]
    internal struct NitrogenSave
    {
        public float nitrogen;
        public float safeDepth;
    }

    [Serializable]
    internal struct TutorialSave
    {
        public bool seamothVehicleExitCosts;
        public bool exosuitVehicleExitCosts;
    }

    [Serializable]
    internal struct WarningSave
    {
        public float lastAscentWarningTime;
        public float lastCrushDepthWarningTime;
        public float lastBreathWarningTime;
        public float lastDecompressionWarningTime;
        public float lastDecoDamageWarningTime;
    }
}