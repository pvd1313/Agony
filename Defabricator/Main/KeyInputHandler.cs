using Agony.Common;
using UnityEngine;

namespace Agony.Defabricator
{
    partial class Main
    {
        private static class KeyInputHandler
        {
            private const KeyCode ActivationKey = KeyCode.C;

            static KeyInputHandler()
            {
                Updater.updated += OnUpdate;
            }

            public static void Patch() { }

            private static void OnUpdate()
            {
                if (Input.GetKeyDown(ActivationKey))
                {
                    if (!Active) { Activate(); }
                    else { Deactivate(); }
                }
            }
        }
    }  
}