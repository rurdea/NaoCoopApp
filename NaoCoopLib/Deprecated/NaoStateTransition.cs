using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NaoCoopLib.Enums;

namespace NaoCoopLib
{
    internal class NaoStateTransition
    {
        readonly NaoState _currentState;
        readonly NaoCommand _command;

        public NaoStateTransition(NaoState currentState, NaoCommand command)
        {
            this._currentState = currentState;
            this._command = command;
        }

        public override int GetHashCode()
        {
            return 17 + 31 * _currentState.GetHashCode() + 31 * _command.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            NaoStateTransition other = obj as NaoStateTransition;
            return other != null && this._currentState == other._currentState && this._command == other._command;
        }
    }
}
