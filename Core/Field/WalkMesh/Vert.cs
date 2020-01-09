﻿namespace OpenVIII.Fields
{
    public partial class WalkMesh
    {
        private struct Vert
        {
            public short x, y, z, res;

            public override string ToString() => $"{nameof(x)} {x},{nameof(y)} {y},{nameof(z)} {z},{nameof(res)} {res}";
        }
    }
}