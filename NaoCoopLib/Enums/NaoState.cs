using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopLib.Enums
{
    public enum NaoState
    {
        Initialized, // robot is initialized
        AtCheckpoint, // robot is at the main location (where the nao mark is)
        AtGrabLocation, // robot is at the grab location
        InLiftPosition, // robot is in grab position
        InWalkPosition, // object lifted, robot is now in walk position
        Terminated // robot walk is terminated
    }

    public enum NaoCommand
    {
        [NaoCommand(Text = "Walk")]
        WalkToCheckpoint,
        [NaoCommand(Text = "Grab")]
        GoToGrabLocation,
        [NaoCommand(Text = "Down")]
        GoToLiftPosition,
        [NaoCommand(Text = "Wait")]
        SynchRobot,
        [NaoCommand(Text = "Up")]
        LiftObject,
        [NaoCommand(Text = "Start")]
        WalkWithObject,
        [NaoCommand(Text = "Stop")]
        Stop
    }

    public class NaoCommandAttribute : Attribute
    {
        public string Text
        {
            get;
            set;
        }

        public static string GetText(NaoCommand command)
        {
            var type = typeof(NaoCommand);
            var memInfo = type.GetMember(command.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(NaoCommandAttribute), false);
            var text = attributes!=null && attributes.Length>0 ? ((NaoCommandAttribute)attributes[0]).Text : command.ToString();
            return text;
        }
    }
}
