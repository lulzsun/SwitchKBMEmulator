using InputServer;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

using Mouse = System.Windows.Forms.Cursor;

namespace SwitchKBMEmulator
{
    public class Controller
    {
        public class None
        {
            public const int ID = 0;
            public static bool pressed = false;
        };
        public class Y
        {
            public const int ID = 1;
            public static bool pressed = false;
        };
        public class B
        {
            public const int ID = 2;
            public static bool pressed = false;
        };
        public class A
        {
            public const int ID = 4;
            public static bool pressed = false;
        };
        public class X
        {
            public const int ID = 8;
            public static bool pressed = false;
        };
        public class L
        {
            public const int ID = 16;
            public bool pressed = false;
        };
        public class R
        {
            public const int ID = 32;
            public static bool pressed = false;
        };
        public class ZL
        {
            public const int ID = 64;
            public static bool pressed = false;
        };
        public class ZR
        {
            public const int ID = 128;
            public static bool pressed = false;
        };
        public class Minus
        {
            public const int ID = 256;
            public static bool pressed = false;
        };
        public class Plus
        {
            public const int ID = 512;
            public static bool pressed = false;
        };
        public class L3
        {
            public const int ID = 1024;
            public static bool pressed = false;
        };
        public class R3
        {
            public const int ID = 2048;
            public static bool pressed = false;
        };
        public class Home
        {
            public const int ID = 4096;
            public static bool pressed = false;
        };
        public class Share
        {
            public const int ID = 8192;
            public static bool pressed = false;
        };

        public bool firstTime = true;
        private Point center;
        private Point last;
        private MouseTranslation mouseTranslation;
        
        //this will need to be rewritten to use raw input instead of hacky WinForms input...
        public void MouseUpdate(SwitchInputSink sink, Panel panel1)
        {
            if (!firstTime)
            {
                Point m_translation = mouseTranslation.Translate(Mouse.Position, center);
                var x = m_translation.X;
                var y = m_translation.Y;

                //var x = (int)(sens * Math.Pow((Mouse.Position.X - center.X), exp)) + 128;
                //var y = (int)(sens * Math.Pow((Mouse.Position.Y - center.Y), exp)) + 128;

                //var x = (int)((Mouse.Position.X - center.X)*sens) + 128;
                //var y = (int)((Mouse.Position.Y - center.Y)*sens) + 128; //deadzone is +-20

                //Console.WriteLine(x + ", " + y);

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
                mouseTranslation = new MouseTranslation();
                Mouse.Position = new Point(panel1.PointToScreen(Point.Empty).X + 128, panel1.PointToScreen(Point.Empty).Y + 128);
                center = Mouse.Position;
            }
        }


        bool l_left, l_right, l_up, l_down, walk;
        int walk_modifier = 0;
        public void KeyBoardUpdate(SwitchInputSink sink)
        {
            //--------------- ZR BUTTON / SHOOT ---------------
            if ((Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left)
            {
                if (!Controller.ZR.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + Controller.ZR.ID));
                    Controller.ZR.pressed = true;
                    Console.WriteLine("ZR");
                }
            }
            else if (((Control.MouseButtons & MouseButtons.Left) != MouseButtons.Left) && Controller.ZR.pressed)
            {
                Controller.ZR.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + Controller.ZR.ID));
            }

            //--------------- A BUTTON ---------------
            if (Keyboard.IsKeyDown(Key.E))
            {
                if (!Controller.A.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + Controller.A.ID));
                    Controller.A.pressed = true;
                    Console.WriteLine("A");
                }
            }
            else if (Keyboard.IsKeyUp(Key.E) && Controller.A.pressed)
            {
                Controller.A.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + Controller.A.ID));
            }

            //--------------- B BUTTON / JUMP ---------------
            if (Keyboard.IsKeyDown(Key.Space))
            {
                if (!Controller.B.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + Controller.B.ID));
                    Controller.B.pressed = true;
                    Console.WriteLine("B");
                }
            }
            else if (Keyboard.IsKeyUp(Key.Space) && Controller.B.pressed)
            {
                Controller.B.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + Controller.B.ID));
            }

            //--------------- X BUTTON / MAP ---------------
            if (Keyboard.IsKeyDown(Key.M))
            {
                if (!Controller.X.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + Controller.X.ID));
                    Controller.X.pressed = true;
                    Console.WriteLine("X");
                }
            }
            else if (Keyboard.IsKeyUp(Key.M) && Controller.X.pressed)
            {
                Controller.X.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + Controller.X.ID));
            }

            //--------------- ZL BUTTON / SQUID FORM/RUN ---------------
            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                if (!Controller.ZL.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + Controller.ZL.ID));
                    Controller.ZL.pressed = true;
                    Console.WriteLine("ZL");
                }
            }
            else if (Keyboard.IsKeyUp(Key.LeftShift) && Controller.ZL.pressed)
            {
                Controller.ZL.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + Controller.ZL.ID));
            }

            //--------------- R BUTTON / SUB WEAPON ---------------
            if (Keyboard.IsKeyDown(Key.G))
            {
                if (!Controller.R.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + Controller.R.ID));
                    Controller.R.pressed = true;
                    Console.WriteLine("R");
                }
            }
            else if (Keyboard.IsKeyUp(Key.G) && Controller.R.pressed)
            {
                Controller.R.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + Controller.R.ID));
            }

            //--------------- R3 BUTTON / SPECIAL WEAPON ---------------
            if (Keyboard.IsKeyDown(Key.Z))
            {
                if (!Controller.R3.pressed)
                {
                    sink.Update(InputFrame.ParseInputString("P=" + Controller.R3.ID));
                    Controller.R3.pressed = true;
                    Console.WriteLine("R3");
                }
            }
            else if (Keyboard.IsKeyUp(Key.Z) && Controller.R3.pressed)
            {
                Controller.R3.pressed = false;
                sink.Update(InputFrame.ParseInputString("R=" + Controller.R3.ID));
            }

            //--------------- WALK MODIFIER ---------------
            //if (Keyboard.IsKeyDown(Key.LeftCtrl))
            //{
            //    if (!walk)
            //    {
            //        walk_modifier = 32;

            //        if (Keyboard.IsKeyDown(Key.W) && l_up)
            //        {
            //            sink.Update(InputFrame.ParseInputString("LY=" + (0 + walk_modifier)));
            //        }
            //        else if (Keyboard.IsKeyDown(Key.S) && l_down)
            //        {
            //            sink.Update(InputFrame.ParseInputString("LY=" + (255 - walk_modifier)));
            //        }


            //        if (Keyboard.IsKeyDown(Key.A) && l_left)
            //        {
            //            sink.Update(InputFrame.ParseInputString("LX=" + (0 + walk_modifier)));
            //        }
            //        else if (Keyboard.IsKeyDown(Key.D) && l_right)
            //        {
            //            sink.Update(InputFrame.ParseInputString("LX=" + (255 - walk_modifier)));
            //        }
            //    }
            //    walk = true;
            //}
            //else if (Keyboard.IsKeyUp(Key.LeftCtrl) && walk)
            //{
            //    walk = false;
            //    walk_modifier = 0;
            //    Console.WriteLine("WALK UP");
            //}

            //--------------- UP ---------------
            if (Keyboard.IsKeyDown(Key.W) && !l_up)
            {
                if (Keyboard.IsKeyDown(Key.S) || l_down)
                {
                    l_down = true;
                }
                if (Keyboard.IsKeyDown(Key.W))
                {
                    sink.Update(InputFrame.ParseInputString("LY=" + (0 + walk_modifier)));
                    Console.WriteLine("L_UP");
                }
                l_up = true;
            }
            else if (Keyboard.IsKeyUp(Key.W) && l_up)
            {
                l_up = false;
                if (Keyboard.IsKeyUp(Key.W))
                    sink.Update(InputFrame.ParseInputString("LY=128"));
                else
                    l_down = false;
            }

            //--------------- DOWN ---------------
            if (Keyboard.IsKeyDown(Key.S) && !l_down)
            {
                if (Keyboard.IsKeyDown(Key.W) || l_up)
                {
                    l_up = true;
                }
                if (Keyboard.IsKeyDown(Key.S))
                {
                    sink.Update(InputFrame.ParseInputString("LY=" + (255 - walk_modifier)));
                    Console.WriteLine("L_DOWN");
                }
                l_down = true;
            }
            else if (Keyboard.IsKeyUp(Key.S) && l_down)
            {
                l_down = false;
                if (Keyboard.IsKeyUp(Key.W))
                    sink.Update(InputFrame.ParseInputString("LY=128"));
                else
                    l_up = false;
            }

            //--------------- LEFT ---------------
            if (Keyboard.IsKeyDown(Key.A) && !l_left)
            {
                if (Keyboard.IsKeyDown(Key.D) || l_right)
                {
                    l_right = true;
                }
                if (Keyboard.IsKeyDown(Key.A))
                {
                    sink.Update(InputFrame.ParseInputString("LX=" + (0 + walk_modifier)));
                    Console.WriteLine("L_LEFT");
                }
                l_left = true;
            }
            else if (Keyboard.IsKeyUp(Key.A) && l_left)
            {
                l_left = false;
                if (Keyboard.IsKeyUp(Key.D))
                    sink.Update(InputFrame.ParseInputString("LX=128"));
                else
                    l_right = false;
            }

            //--------------- RIGHT ---------------
            if (Keyboard.IsKeyDown(Key.D) && !l_right)
            {
                if (Keyboard.IsKeyDown(Key.A) || l_left)
                {
                    l_left = true;
                }
                if (Keyboard.IsKeyDown(Key.D))
                {
                    sink.Update(InputFrame.ParseInputString("LX=" + (255 - walk_modifier)));
                    Console.WriteLine("L_RIGHT");
                }
                l_right = true;
            }
            else if (Keyboard.IsKeyUp(Key.D) && l_right)
            {
                l_right = false;

                if (Keyboard.IsKeyUp(Key.A))
                    sink.Update(InputFrame.ParseInputString("LX=128"));
                else
                    l_left = false;
            }
        }
    }
}
