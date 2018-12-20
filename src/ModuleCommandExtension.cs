using System;

namespace AttitudeAdjuster
{
    /// <summary>
    /// Base class for PartModules that need to do stuff with control points on ModuleCommand.
    /// </summary>
    public class ModuleCommandExtension : PartModule
    {
        private ModuleCommand commandModule = null;
        private ControlPoint controlPoint = null;

        /// <summary>
        /// Specifies the name of a control point on a ModuleCommand on this part,
        /// whose orientation this module will control.
        /// </summary>
        [KSPField]
        public string controlPointName;

        public override void OnStart(StartState state)
        {
            base.OnStart(state);

            if (part == null) return;

            // Initialize the control point. If there's a problem (i.e. because the config is wrong and
            // something doesn't match up), log an error and "go dead" (stay in an uninitialized state
            // so that nothing will happen).
            try
            {
                FindControlPoint(part, controlPointName, out commandModule, out controlPoint);
            }
            catch (ArgumentException e)
            {
                Logging.Error(this.moduleName + " is inactive for " + part.name + ": " + e.Message);
                return;
            }
            Logging.Log("Initialized for " + part.name);
            OnControlPointChanged(commandModule.ActiveControlPointName);
        }

        /// <summary>
        /// Find the specified control point.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="name"></param>
        /// <param name="commandModule"></param>
        /// <param name="controlPoint"></param>
        private static void FindControlPoint(Part part, string name, out ModuleCommand commandModule, out ControlPoint controlPoint)
        {
            commandModule = null;
            controlPoint = null;
            ModuleCommand foundModule = null;
            for (int i = 0; i < part.Modules.Count; ++i)
            {
                foundModule = part.Modules[i] as ModuleCommand;
                if (foundModule != null) break;
            }
            if (foundModule == null)
            {
                throw new ArgumentException("no ModuleCommand found");
            }
            if ((name == null) || (name.Equals(string.Empty)))
            {
                commandModule = foundModule;
                return;
            }
            ControlPoint foundControlPoint;
            if (!foundModule.controlPoints.TryGetValue(name, out foundControlPoint))
            {
                string message = "no control point named '" + name + "' found; there are "
                    + foundModule.controlPoints.Count + " control points defined";
                if (foundModule.controlPoints.Count > 0)
                {
                    message += " (names:";
                    foreach (string key in foundModule.controlPoints.Keys)
                    {
                        message += " '" + key + "'";
                    }
                    message += ")";
                }
                throw new ArgumentException(message);
            }
            // Success!
            commandModule = foundModule;
            controlPoint = foundControlPoint;
        }

        /// <summary>
        /// Called once, on first update.
        /// </summary>
        private void Initialize()
        {

        }

        protected ModuleCommand CommandModule
        {
            get { return commandModule; }
        }

        protected ControlPoint CurrentControlPoint
        {
            get { return controlPoint; }
        }


        /// <summary>
        /// Here when any ModuleCommand anywhere changes its control point.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="controlPoint"></param>
        public static void OnControlPointChanged(Part part, ControlPoint controlPoint)
        {
            Logging.Log("Control point for " + part.name + " changed to " + controlPoint.displayName);

            // Pass the notification along to any modules deriving from this one.
            for (int i = 0; i < part.Modules.Count; ++i)
            {
                ModuleCommandExtension module = part.Modules[i] as ModuleCommandExtension;
                if (module != null)
                {
                    module.OnControlPointChanged(controlPoint.name);
                }
            }
        }

        /// <summary>
        /// Here when the current control point changes.
        /// </summary>
        /// <param name="currentControlPointName"></param>
        protected virtual void OnControlPointChanged(string currentControlPointName)
        {
            // do nothing
        }
    }
}
