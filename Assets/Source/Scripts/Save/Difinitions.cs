using UnityEngine;
using System;

namespace Game.Preferences
{
    [Serializable]
    public struct PreferencesGame
    {
        public Vector2 sensitivity;
        public Vector2Int resolution;
        public FullScreenMode screenMode;
        public AudioGame audio;
        public bool fogEnabled;
        public bool particlesEnabled;
    }

    [Serializable]
    public struct AudioGame
    {
        public float general;
        public float music;
        public float sound;
    }
}