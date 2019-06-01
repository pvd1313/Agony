using UnityEngine.SceneManagement;
using UnityEngine;
using System;

namespace Agony.Common
{
    public static class Updater
    {
        private sealed class MonoUpdater : MonoBehaviour
        {
            private void Update() => updated?.Invoke();
        }

        public static Action updated;

        private static MonoUpdater updater;

        static Updater()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            Initialize();
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mod) => Initialize();

        private static void Initialize()
        {
            if (updater) return;
            var obj = new GameObject("Agony.Common.Updater");
            updater = obj.AddComponent<MonoUpdater>();
        }
    }
}