﻿using System;


namespace OpenVIII.Fields
{
    internal sealed class PUSHANIME : JsmInstruction
    {
        public PUSHANIME()
        {
        }

        public PUSHANIME(Int32 parameter, IStack<IJsmExpression> stack)
            : this()
        {
        }

        public override String ToString()
        {
            return $"{nameof(PUSHANIME)}()";
        }
    }
}