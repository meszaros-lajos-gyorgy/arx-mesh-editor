﻿using System.Runtime.InteropServices;

namespace Assets.Scripts.ArxNative.IO.FTS
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FTS_IO_VERTEX
    {
        public float posY;
        public float posX;
        public float posZ;
        public float texU;
        public float texV;
    }
}
