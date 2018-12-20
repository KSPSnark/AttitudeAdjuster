namespace AttitudeAdjuster
{
    /// <summary>
    /// Adds an action group for cycling to the next control point.
    /// </summary>
    public class ModuleChangeControlPoint : ModuleCommandExtension
    {
        /// <summary>
        /// Action group for toggling between the specified control point and the part's default.
        /// </summary>
        /// <param name="actionParam"></param>
        [KSPAction("#AttitudeAdjuster_changeControlPointAction")]
        public void DoChangeControlPointAction(KSPActionParam actionParam)
        {
            if (CommandModule == null) return; // not initialized, can't do anything
            CommandModule.ChangeControlPoint();
        }
    }
}
