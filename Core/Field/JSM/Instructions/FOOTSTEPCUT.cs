﻿using System;


namespace OpenVIII.Fields
{
    internal sealed class FOOTSTEPCUT : JsmInstruction
    {
        public FOOTSTEPCUT()
        {
        }

        public FOOTSTEPCUT(Int32 parameter, IStack<IJsmExpression> stack)
            : this()
        {
        }

        public override String ToString()
        {
            return $"{nameof(FOOTSTEPCUT)}()";
        }
    }
}