using System;
using UnityEngine;

namespace AttitudeAdjuster
{
    /// <summary>
    /// A module that allows the user to manually adjust the orientation of a ModuleCommand
    /// control point via a UI slider. To use, add a ModuleAttitudeAdjuster to a part that
    /// also has a ModuleCommand on it, such that the ModuleCommand contains a control point
    /// whose name is the same as adjustableControlPointName.
    /// </summary>
    public class ModuleAttitudeAdjuster : ModuleCommandExtension
    {
        private const float DEFAULT_MIN_PITCH = 0;
        private const float DEFAULT_MAX_PITCH = 90;
        private const float DEFAULT_PITCH_INCREMENT = 1;

        /// <summary>
        /// The minimum allowed value of the pitch field.
        /// </summary>
        [KSPField]
        public float pitchMin = DEFAULT_MIN_PITCH;

        /// <summary>
        /// The maximum allowed value of the pitch field.
        /// </summary>
        [KSPField]
        public float pitchMax = DEFAULT_MAX_PITCH;

        /// <summary>
        /// The adjustment increment of the pitch field.
        /// </summary>
        [KSPField]
        public float pitchIncrement = DEFAULT_PITCH_INCREMENT;

        /// <summary>
        /// Specifies the pitch angle, in degrees, that's applied to the control point.
        /// 0 = no adjustment, 90 = pitch 90 degrees straight up, 180 = full reversal.
        /// </summary>
        [KSPField(guiName = "#AttitudeAdjuster_pitchAngle", guiActive = false, guiActiveEditor = false, isPersistant = true),
         UI_FloatRange(scene = UI_Scene.All, affectSymCounterparts = UI_Scene.All, controlEnabled = true,
            minValue = DEFAULT_MIN_PITCH, maxValue = DEFAULT_MAX_PITCH, stepIncrement = DEFAULT_PITCH_INCREMENT)]
        public float pitch;
        private BaseField PitchField { get { return Fields["pitch"]; } }

        public override void OnStart(StartState state)
        {
            base.OnStart(state);

            // Set up the control range for the module's UI.
            UI_FloatRange uiRange1 = (UI_FloatRange)PitchField.uiControlEditor;
            UI_FloatRange uiRange2 = (UI_FloatRange)PitchField.uiControlFlight;
            uiRange1.minValue = uiRange2.minValue = pitchMin;
            uiRange1.maxValue = uiRange2.maxValue = pitchMax;
            uiRange1.stepIncrement = uiRange2.stepIncrement = pitchIncrement;
        }

        /// <summary>
        /// Called on every frame.
        /// </summary>
        void Update()
        {
            if (CurrentControlPoint == null) return; // nothing to do
            if (-pitch == CurrentControlPoint.orientation.x) return; // nothing to do
            CurrentControlPoint.orientation.x = -pitch;
            CurrentControlPoint.transform.localRotation = Quaternion.AngleAxis(-pitch, Vector3.left);
        }

        /// <summary>
        /// Here when the current control point changes.
        /// </summary>
        /// <param name="currentControlPointName"></param>
        protected override void OnControlPointChanged(string currentControlPointName)
        {
            base.OnControlPointChanged(currentControlPointName);

            // Display the pitch-adjustment control only if the currently selected control point
            // is the pitch-adjustment one.
            bool isAdjuster = currentControlPointName == controlPointName;
            PitchField.guiActiveEditor = isAdjuster;
            PitchField.guiActive = isAdjuster;
        }
    }
}
