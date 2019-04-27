using InputServer;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

using Mouse = System.Windows.Forms.Cursor;

namespace SwitchKBMEmulator
{
    class Controller
    {
        public readonly string[] buttons = { "Y", "B", "A", "X",
                                "Up", "Down", "Left", "Right",
                                "L", "R", "ZL", "ZR",
                                "Minus", "Plus", "L3", "R3",
                                "Home", "Share" };
        public struct None
        {
            public const int ID = 0;
            public static bool pressed = false;
        };
        public class Y
        {
            public const int ID = 1;
            public static Object bind = Key.Y;
            public static bool pressed = false;
        };
        public class B
        {
            public const int ID = 2;
            public static Object bind = Key.Space;
            public static bool pressed = false;
        };
        public class A
        {
            public const int ID = 4;
            public static Object bind = Key.E;
            public static bool pressed = false;
        };
        public class X
        {
            public const int ID = 8;
            public static Object bind = Key.M;
            public static bool pressed = false;
        };
        public class Up
        {
            public const int ID = 0;
            public static Object bind = Key.Up;
            public static bool pressed = false;
        };
        public class Down
        {
            public const int ID = 4;
            public static Object bind = Key.Down;
            public static bool pressed = false;
        };
        public class Left
        {
            public const int ID = 6;
            public static Object bind = Key.Left;
            public static bool pressed = false;
        };
        public class Right
        {
            public const int ID = 2;
            public static Object bind = Key.Right;
            public static bool pressed = false;
        };
        public class L
        {
            public const int ID = 16;
            public static Object bind = Key.Q;
            public static bool pressed = false;
        };
        public class R
        {
            public const int ID = 32;
            public static Object bind = Key.G;
            public static bool pressed = false;
        };
        public class ZL
        {
            public const int ID = 64;
            public static Object bind = Key.LeftShift;
            public static bool pressed = false;
        };
        public class ZR
        {
            public const int ID = 128;
            public static Object bind = MouseButtons.Left;
            public static bool pressed = false;
        };
        public class Minus
        {
            public const int ID = 256;
            public static Object bind = Key.O;
            public static bool pressed = false;
        };
        public class Plus
        {
            public const int ID = 512;
            public static Object bind = Key.P;
            public static bool pressed = false;
        };
        public class L3
        {
            public const int ID = 1024;
            public static Object bind = Key.C;
            public static bool mouseaim = false;
            public static bool movement = true;
            public static bool pressed = false;
        };
        public class R3
        {
            public const int ID = 2048;
            public static Object bind = Key.Z;
            public static bool mouseaim = true;
            public static bool movement = false;
            public static bool pressed = false;
        };
        public class Home
        {
            public const int ID = 4096;
            public static Object bind = Key.PageUp;
            public static bool pressed = false;
        };
        public class Share
        {
            public const int ID = 8192;
            public static Object bind = Key.PageDown;
            public static bool pressed = false;
        };

        public bool firstTime = true;
        private Point center;
        private Point last;
        public MouseTranslation mouseTranslation;

        public Controller()
        {
            mouseTranslation = new MouseTranslation();
        }
        
        //this will need to be rewritten to use raw input instead of hacky WinForms input...
        public void MouseUpdate(SwitchInputSink sink, Panel panel1)
        {
            if (!firstTime)
            {
                Point m_translation = mouseTranslation.Translate(Mouse.Position, center);
                var x = m_translation.X;
                var y = m_translation.Y;

                if (x > 255)
                {
                    Mouse.Position = new Point(Mouse.Position.X - (x - 255), Mouse.Position.Y);
                    x = 255;
                }
                else if (x < 0)
                {
                    Mouse.Position = new Point(Mouse.Position.X - x, Mouse.Position.Y);
                    x = 0;
                }
                if (y > 255)
                {
                    Mouse.Position = new Point(Mouse.Position.X, Mouse.Position.Y - (y - 255));
                    y = 255;
                }
                else if (y < 0)
                {
                    Mouse.Position = new Point(Mouse.Position.X, Mouse.Position.Y - y);
                    y = 0;
                }

                if (last.X != x)
                {
                    Console.WriteLine(x + ", " + y);
                    sink.Update(InputFrame.ParseInputString("RX=" + x));
                }

                if (last.Y != y)
                {
                    Console.WriteLine(x + ", " + y);
                    sink.Update(InputFrame.ParseInputString("RY=" + y));
                }
                Mouse.Position = center;
                last = new Point(x, y);

            }
            else
            {
                firstTime = false;
                Mouse.Position = new Point(panel1.PointToScreen(Point.Empty).X + 128, panel1.PointToScreen(Point.Empty).Y + 128);
                center = Mouse.Position;
            }
        }

        Point circlePosition = new Point(0, 0);
        int circleCount = 1;
        //not really a functional test
        public void CircleTest(SwitchInputSink sink)
        {
            Point m_translation = mouseTranslation.Translate(circlePosition, new Point(0, 0));

            switch(circleCount)
            {
                case 1:
                    circlePosition.X = 0;
                    circlePosition.Y = 1;
                    break;
                case 2:
                    circlePosition.X = 1;
                    circlePosition.Y = 1;
                    break;
                case 3:
                    circlePosition.X = 1;
                    circlePosition.Y = 0;
                    break;
                case 4:
                    circlePosition.X = 1;
                    circlePosition.Y = -1;
                    break;
                case 5:
                    circlePosition.X = 0;
                    circlePosition.Y = -1;
                    break;
                case 6:
                    circlePosition.X = -1;
                    circlePosition.Y = -1;
                    break;
                case 7:
                    circlePosition.X = -1;
                    circlePosition.Y = 0;
                    break;
                case 8:
                    circlePosition.X = -1;
                    circlePosition.Y = 1;
                    circleCount = 1;
                    break;
            }
            circleCount++;

            var x = m_translation.X;
            var y = m_translation.Y;

            if (x > 255)
            {
                x = 255;
            }
            else if (x < 0)
            {
                x = 0;
            }
            if (y > 255)
            {
                y = 255;
            }
            else if (y < 0)
            {
                y = 0;
            }

            Console.WriteLine(x + ", " + y);
            sink.Update(InputFrame.ParseInputString("RX=" + x));
            sink.Update(InputFrame.ParseInputString("RY=" + y));
        }

        public bool KeyBindDown(Object bind)
        {
            if (bind.GetType().ToString() == "System.Windows.Input.Key")
            {
                if (Keyboard.IsKeyDown((Key)bind))
                    return true;
            }
            else if (bind.GetType().ToString() == "System.Windows.Forms.MouseButtons")
            {
                if ((Control.MouseButtons & MouseButtons.Left) == (MouseButtons)bind)
                    return true;
            }
            return false;
        }

        public Object GetBind(string button)
        {
            switch (button)
            {
                case "Y":
                    return Y.bind;
                case "B":
                    return B.bind;
                case "A":
                    return A.bind;
                case "X":
                    return X.bind;
                case "Up":
                    return Up.bind;
                case "Down":
                    return Down.bind;
                case "Left":
                    return Left.bind;
                case "Right":
                    return Right.bind;
                case "L":
                    return L.bind;
                case "R":
                    return R.bind;
                case "ZL":
                    return ZL.bind;
                case "ZR":
                    return ZR.bind;
                case "Minus":
                    return Minus.bind;
                case "Plus":
                    return Plus.bind;
                case "L3":
                    return L3.bind;
                case "R3":
                    return R3.bind;
                case "Home":
                    return Home.bind;
                case "Share":
                    return Share.bind;
            }
            return "WHAT THE FUCK: "+ button;
        }
        
        public void UpdateBind(string button, Object input)
        {
            switch (button)
            {
                case "Y":
                    Y.bind = input;
                    break;
                case "B":
                    B.bind = input;
                    break;
                case "A":
                    A.bind = input;
                    break;
                case "X":
                    X.bind = input;
                    break;
                case "Up":
                    Up.bind = input;
                    break;
                case "Down":
                    Down.bind = input;
                    break;
                case "Left":
                    Left.bind = input;
                    break;
                case "Right":
                    Right.bind = input;
                    break;
                case "L":
                    L.bind = input;
                    break;
                case "R":
                    R.bind = input;
                    break;
                case "ZL":
                    ZL.bind = input;
                    break;
                case "ZR":
                    ZR.bind = input;
                    break;
                case "Minus":
                    Minus.bind = input;
                    break;
                case "Plus":
                    Plus.bind = input;
                    break;
                case "L3":
                    if (input.ToString().Equals("mouseaim"))
                        L3.mouseaim = !L3.mouseaim;
                    else if (input.ToString().Equals("movement"))
                        L3.movement = !L3.movement;
                    else
                        L3.bind = input;
                    break;
                case "R3":
                    if (input.ToString().Equals("mouseaim"))
                        R3.mouseaim = !R3.mouseaim;
                    else if (input.ToString().Equals("movement"))
                        R3.movement = !R3.movement;
                    else
                        R3.bind = input;
                    break;
                case "Home":
                    Home.bind = input;
                    break;
                case "Share":
                    Share.bind = input;
                    break;
            }
        }

        bool l_left, l_right, l_up, l_down, walk;
        int walk_modifier = 0;
        public void KeyBoardUpdate(SwitchInputSink sink)
        {
            //--------------- Y BUTTON ---------------
            if (KeyBindDown(Y.bind))
            {
                if (!Y.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + Y.ID));
                    Y.pressed = true;
                    Console.WriteLine("Y");
                }
            }
            else if (!KeyBindDown(Y.bind) && Y.pressed)
            {
                Y.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + Y.ID));
            }

            //--------------- B BUTTON / JUMP ---------------
            if (KeyBindDown(B.bind))
            {
                if (!B.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + B.ID));
                    B.pressed = true;
                    Console.WriteLine("B");
                }
            }
            else if (!KeyBindDown(B.bind) && B.pressed)
            {
                B.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + B.ID));
            }

            //--------------- A BUTTON ---------------
            if (KeyBindDown(A.bind))
            {
                if (!A.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + A.ID));
                    A.pressed = true;
                    Console.WriteLine("A");
                }
            }
            else if (!KeyBindDown(A.bind) && A.pressed)
            {
                A.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + A.ID));
            }

            //--------------- X BUTTON / MAP ---------------
            if (KeyBindDown(X.bind))
            {
                if (!X.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + X.ID));
                    X.pressed = true;
                    Console.WriteLine("X");
                }
            }
            else if (!KeyBindDown(X.bind) && X.pressed)
            {
                X.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + X.ID));
            }

            //--------------- L BUTTON ---------------
            if (KeyBindDown(L.bind))
            {
                if (!L.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + L.ID));
                    L.pressed = true;
                    Console.WriteLine("L");
                }
            }
            else if (!KeyBindDown(L.bind) && L.pressed)
            {
                L.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + L.ID));
            }

            //--------------- R BUTTON ---------------
            if (KeyBindDown(R.bind))
            {
                if (!R.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + R.ID));
                    R.pressed = true;
                    Console.WriteLine("R");
                }
            }
            else if (!KeyBindDown(R.bind) && R.pressed)
            {
                R.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + R.ID));
            }

            //--------------- ZL BUTTON / SQUID FORM/RUN ---------------
            if (KeyBindDown(ZL.bind))
            {
                if (!ZL.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + ZL.ID));
                    ZL.pressed = true;
                    Console.WriteLine("ZL");
                }
            }
            else if (!KeyBindDown(ZL.bind) && ZL.pressed)
            {
                ZL.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + ZL.ID));
            }

            //--------------- ZR BUTTON / SHOOT ---------------
            if (KeyBindDown(ZR.bind))
            {
                if (!ZR.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + ZR.ID));
                    ZR.pressed = true;
                    Console.WriteLine("ZR");
                }
            }
            else if (!KeyBindDown(ZR.bind) && ZR.pressed)
            {
                ZR.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + ZR.ID));
            }

            //--------------- L3 BUTTON ---------------
            if (KeyBindDown(L3.bind))
            {
                if (!L3.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + L3.ID));
                    L3.pressed = true;
                    Console.WriteLine("L3");
                }
            }
            else if (!KeyBindDown(L3.bind) && L3.pressed)
            {
                L3.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + L3.ID));
            }

            //--------------- R3 BUTTON / SPECIAL WEAPON ---------------
            if (KeyBindDown(R3.bind))
            {
                if (!R3.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + R3.ID));
                    R3.pressed = true;
                    Console.WriteLine("R3");
                }
            }
            else if (!KeyBindDown(R3.bind) && R3.pressed)
            {
                R3.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + R3.ID));
            }

            //--------------- Minus BUTTON ---------------
            if (KeyBindDown(Minus.bind))
            {
                if (!Minus.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + Minus.ID));
                    Minus.pressed = true;
                    Console.WriteLine("MINUS");
                }
            }
            else if (!KeyBindDown(Minus.bind) && Minus.pressed)
            {
                Minus.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + Minus.ID));
            }

            //--------------- Plus BUTTON ---------------
            if (KeyBindDown(Plus.bind))
            {
                if (!Plus.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + Plus.ID));
                    Plus.pressed = true;
                    Console.WriteLine("PLUS");
                }
            }
            else if (!KeyBindDown(Plus.bind) && Plus.pressed)
            {
                Plus.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + Plus.ID));
            }

            //--------------- HOME BUTTON ---------------
            if (KeyBindDown(Home.bind))
            {
                if (!Home.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + Home.ID));
                    Home.pressed = true;
                    Console.WriteLine("HOME");
                }
            }
            else if (!KeyBindDown(Home.bind) && Home.pressed)
            {
                Home.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + Home.ID));
            }

            //--------------- SHARE BUTTON ---------------
            if (KeyBindDown(Share.bind))
            {
                if (!Share.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + Share.ID));
                    Share.pressed = true;
                    Console.WriteLine("SHARE");
                }
            }
            else if (!KeyBindDown(Share.bind) && Share.pressed)
            {
                Share.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + Share.ID));
            }

            //--------------- WALK MODIFIER ---------------
            //if (KeyBindDown(Key.LeftCtrl))
            //{
            //    if (!walk)
            //    {
            //        walk_modifier = 32;

            //        if (KeyBindDown(Key.W) && l_up)
            //        {
            //            sink.Update(InputFrame.ParseInputString("LY=" + (0 + walk_modifier)));
            //        }
            //        else if (KeyBindDown(Key.S) && l_down)
            //        {
            //            sink.Update(InputFrame.ParseInputString("LY=" + (255 - walk_modifier)));
            //        }


            //        if (KeyBindDown(Key.A) && l_left)
            //        {
            //            sink.Update(InputFrame.ParseInputString("LX=" + (0 + walk_modifier)));
            //        }
            //        else if (KeyBindDown(Key.D) && l_right)
            //        {
            //            sink.Update(InputFrame.ParseInputString("LX=" + (255 - walk_modifier)));
            //        }
            //    }
            //    walk = true;
            //}
            //else if (!KeyBindDown(Key.LeftCtrl) && walk)
            //{
            //    walk = false;
            //    walk_modifier = 0;
            //    Console.WriteLine("WALK UP");
            //}

            //--------------- DPAD UP BUTTON ---------------
            if (KeyBindDown(Up.bind))
            {
                if (!Up.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("D=" + Up.ID));
                    Up.pressed = true;
                    Console.WriteLine("UP");
                }
            }
            else if (!KeyBindDown(Up.bind) && Up.pressed)
            {
                Up.pressed = false;
                sink.Update(InputFrame.ParseInputString("D=8"));
            }

            //--------------- DPAD DOWN BUTTON ---------------
            if (KeyBindDown(Down.bind))
            {
                if (!Down.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("D=" + Down.ID));
                    Down.pressed = true;
                    Console.WriteLine("DOWN");
                }
            }
            else if (!KeyBindDown(Down.bind) && Down.pressed)
            {
                Down.pressed = false;
                sink.Update(InputFrame.ParseInputString("D=8"));
            }

            //--------------- DPAD LEFT BUTTON ---------------
            if (KeyBindDown(Left.bind))
            {
                if (!Left.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("D=" + Left.ID));
                    Left.pressed = true;
                    Console.WriteLine("LEFT");
                }
            }
            else if (!KeyBindDown(Left.bind) && Left.pressed)
            {
                Left.pressed = false;
                sink.Update(InputFrame.ParseInputString("D=8"));
            }

            //--------------- DPAD RIGHT BUTTON ---------------
            if (KeyBindDown(Right.bind))
            {
                if (!Right.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("D=" + Right.ID));
                    Right.pressed = true;
                    Console.WriteLine("RIGHT");
                }
            }
            else if (!KeyBindDown(Right.bind) && Right.pressed)
            {
                Right.pressed = false;
                sink.Update(InputFrame.ParseInputString("D=8"));
            }

            //--------------- UP ---------------
            if (KeyBindDown(Key.W) && !l_up)
            {
                if (KeyBindDown(Key.S) || l_down)
                {
                    l_down = true;
                }
                if (KeyBindDown(Key.W))
                {
                    sink.Update(InputFrame.ParseInputString("LY=" + (0 + walk_modifier)));
                    Console.WriteLine("L_UP");
                }
                l_up = true;
            }
            else if (!KeyBindDown(Key.W) && l_up)
            {
                l_up = false;
                if (!KeyBindDown(Key.W))
                    sink.Update(InputFrame.ParseInputString("LY=128"));
                else
                    l_down = false;
            }

            //--------------- DOWN ---------------
            if (KeyBindDown(Key.S) && !l_down)
            {
                if (KeyBindDown(Key.W) || l_up)
                {
                    l_up = true;
                }
                if (KeyBindDown(Key.S))
                {
                    sink.Update(InputFrame.ParseInputString("LY=" + (255 - walk_modifier)));
                    Console.WriteLine("L_DOWN");
                }
                l_down = true;
            }
            else if (!KeyBindDown(Key.S) && l_down)
            {
                l_down = false;
                if (!KeyBindDown(Key.W))
                    sink.Update(InputFrame.ParseInputString("LY=128"));
                else
                    l_up = false;
            }

            //--------------- LEFT ---------------
            if (KeyBindDown(Key.A) && !l_left)
            {
                if (KeyBindDown(Key.D) || l_right)
                {
                    l_right = true;
                }
                if (KeyBindDown(Key.A))
                {
                    sink.Update(InputFrame.ParseInputString("LX=" + (0 + walk_modifier)));
                    Console.WriteLine("L_LEFT");
                }
                l_left = true;
            }
            else if (!KeyBindDown(Key.A) && l_left)
            {
                l_left = false;
                if (!KeyBindDown(Key.D))
                    sink.Update(InputFrame.ParseInputString("LX=128"));
                else
                    l_right = false;
            }

            //--------------- RIGHT ---------------
            if (KeyBindDown(Key.D) && !l_right)
            {
                if (KeyBindDown(Key.A) || l_left)
                {
                    l_left = true;
                }
                if (KeyBindDown(Key.D))
                {
                    sink.Update(InputFrame.ParseInputString("LX=" + (255 - walk_modifier)));
                    Console.WriteLine("L_RIGHT");
                }
                l_right = true;
            }
            else if (!KeyBindDown(Key.D) && l_right)
            {
                l_right = false;

                if (!KeyBindDown(Key.A))
                    sink.Update(InputFrame.ParseInputString("LX=128"));
                else
                    l_left = false;
            }
        }
    }
}
