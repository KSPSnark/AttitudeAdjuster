namespace AttitudeAdjuster
{
    /// <summary>
    /// A module that adds an action item for toggling the control point of a part
    /// to the specified one.
    /// </summary>
    public class ModuleToggleControlPoint : ModuleCommandExtension
    {
        /// <summary>
        /// Action group for toggling between the specified control point and the part's default.
        /// </summary>
        /// <param name="actionParam"></param>
        [KSPAction("#AttitudeAdjuster_toggleAction")]
        public void DoToggleAction(KSPActionParam actionParam)
        {
            if (CommandModule == null) return; // not initialized, can't do anything
            bool isActivating = actionParam.type != KSPActionType.Deactivate;
            CommandModule.SetControlPoint(isActivating ? controlPointName : "_default");
        }
        private BaseAction ToggleAction { get { return Actions["DoToggleAction"];  } }

        public override void OnStart(StartState state)
        {
            base.OnStart(state);

            if (CommandModule == null)
            {
                Logging.Error("No command module found for " + controlPointName + " on " + part.name);
                return;
            }
            ControlPoint controlPoint = CommandModule.GetControlPoint(controlPointName);
            string display = (controlPoint == null) ? "ERROR" : controlPoint.displayName;
            ToggleAction.guiName = KSP.Localization.Localizer.GetStringByTag("#AttitudeAdjuster_toggleAction")
                + " " + display;
        }
    }
}
