using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBMSwitchAdapter
{
    public static class Controller
    {
        public static class None
        {
            public const int ID = 0;
            public static bool pressed = false;
        };
        public static class Y
        {
            public const int ID = 1;
            public static bool pressed = false;
        };
        public static class B
        {
            public const int ID = 2;
            public static bool pressed = false;
        };
        public static class A
        {
            public const int ID = 4;
            public static bool pressed = false;
        };
        public static class X
        {
            public const int ID = 8;
            public static bool pressed = false;
        };
        public class L
        {
            public const int ID = 16;
            public bool pressed = false;
        };
        public static class R
        {
            public const int ID = 32;
            public static bool pressed = false;
        };
        public static class ZL
        {
            public const int ID = 64;
            public static bool pressed = false;
        };
        public static class ZR
        {
            public const int ID = 128;
            public static bool pressed = false;
        };
        public class Minus
        {
            public const int ID = 256;
            public static bool pressed = false;
        };
        public static class Plus
        {
            public const int ID = 512;
            public static bool pressed = false;
        };
        public static class L3
        {
            public const int ID = 1024;
            public static bool pressed = false;
        };
        public static class R3
        {
            public const int ID = 2048;
            public static bool pressed = false;
        };
        public static class Home
        {
            public const int ID = 4096;
            public static bool pressed = false;
        };
        public static class Share
        {
            public const int ID = 8192;
            public static bool pressed = false;
        };
    }
}
