using UnityEngine;

namespace AttitudeAdjuster
{
    [KSPAddon(KSPAddon.Startup.FlightAndEditor, false)]
    class ControlPointMonitor : MonoBehaviour
    {
        public void Awake()
        {
            Logging.Log("Registering events");
            GameEvents.OnControlPointChanged.Add(OnControlPointChanged);
        }

        public void OnDestroy()
        {
            Logging.Log("Unregistering events");
            GameEvents.OnControlPointChanged.Remove(OnControlPointChanged);
        }

        /// <summary>
        /// Here whenever any ModuleCommand's control point changes.
        /// </summary>
        /// <param name="data0"></param>
        /// <param name="data1"></param>
        private void OnControlPointChanged(Part part, ControlPoint controlPoint)
        {
            ModuleCommandExtension.OnControlPointChanged(part, controlPoint);
        }
    }
}

