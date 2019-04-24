using System;

namespace SwitchKBMEmulator
{
    class MouseTranslation
    {
        //Sensitivity
        System.Windows.Point mc_sensitivity = new System.Windows.Point(0.5, 0.5);

        //Sensitivity with Sigmoid
        System.Windows.Point mc_sensitivity2 = new System.Windows.Point(0.5, 0.5);

        //Mouse Delta Start Threshold
        System.Windows.Point mc_mouse_delta_start_threshold = new System.Windows.Point(0.0, 0.0);

        //Delta Sensitivity Sigmoid Constant, -1.0 < k < 1.0
        System.Windows.Point mc_delta_sensitivity_sigmoid_constant = new System.Windows.Point(-0.5, -0.5);

        //Initial Delta
        System.Windows.Point mc_delta_initial = new System.Windows.Point(128 * 0.5, 128 * 0.5);

        //Delta-Stopping Threshold, 0.0 <= k <= 1.0
        System.Windows.Point mc_delta_stop_threshold = new System.Windows.Point(128 * 0.49, 128 * 0.49);

        //Delta Damping Origin
        System.Windows.Point mc_delta_damping_origin = new System.Windows.Point(128 * 0.4, 128 * 0.4);

        //Delta Damping, 0.0 <= k <= 1.0
        System.Windows.Point mc_delta_damping = new System.Windows.Point(0.0, 0.0);

        //Delta Damping with Sigmoid, 0.0 <= k <= 1.0
        System.Windows.Point mc_delta_damping2 = new System.Windows.Point(0.1, 0.1);

        //Delta Damping with Sigmoid Constant, -1.0 < k < 1.0
        System.Windows.Point mc_delta_damping_sigmoid_constant = new System.Windows.Point(0.5, 0.5);

        //Maximum Delta
        System.Windows.Point mc_delta_max = new System.Windows.Point(128, 128);

        //system.threadExecutionInterval = 1000.0 / mc_framerate

        //Mouse Camera Init
        //bool mc_enabled = false; // Toggle
        System.Windows.Point mc_delta = new System.Windows.Point(); // Mouse Camera Delta

        //System.Windows.Point mc_screen_half = new System.Windows.Point(); // Screen Half
        System.Windows.Point mc_mouse_delta = new System.Windows.Point(); // Mouse Delta
        System.Windows.Point mc_mouse = new System.Windows.Point(); // Mouse Position

        public MouseTranslation()
        {
            //Damping Initialization
            mc_delta_damping.X = 0;
            mc_delta_damping.X = 1.0 - mc_delta_damping.X;
            mc_delta_damping.Y = 1.0 - mc_delta_damping.Y;
            mc_delta_damping2.X = 1.0 - mc_delta_damping2.X;
            mc_delta_damping2.Y = 1.0 - mc_delta_damping2.Y;
        }

        private double Sigmoid_Tunable(double k, double x)
        {
            return (x - x * k) / (k - Math.Abs(x) * 2.0 * k + 1.0);
        }

        public System.Drawing.Point Translate(System.Drawing.Point mouse, System.Drawing.Point center)
        {
            // Mouse Delta
            mc_mouse = new System.Windows.Point(mouse.X, mouse.Y);
            mc_mouse_delta = new System.Windows.Point(mc_mouse.X - center.X, mc_mouse.Y - center.Y);

            // Initial Delta
            if (mc_delta.X == 0.0 && Math.Abs(mc_mouse_delta.X) > mc_mouse_delta_start_threshold.X)
                mc_delta.X = mc_delta_initial.X * Math.Abs(mc_mouse_delta.X) / mc_mouse_delta.X;
            if (mc_delta.Y == 0.0 && Math.Abs(mc_mouse_delta.Y) > mc_mouse_delta_start_threshold.Y)
                mc_delta.Y = mc_delta_initial.Y * Math.Abs(mc_mouse_delta.Y) / mc_mouse_delta.Y;

            // Mouse Camera Delta
            if (mc_delta.X != 0.0)
                mc_delta.X += mc_mouse_delta.X * (mc_sensitivity.X + mc_sensitivity2.X * Math.Abs(Sigmoid_Tunable(mc_delta_sensitivity_sigmoid_constant.X, mc_delta.X / mc_delta_max.X)));
            if (mc_delta.Y != 0.0)
                mc_delta.Y += mc_mouse_delta.Y * (mc_sensitivity.Y + mc_sensitivity2.Y * Math.Abs(Sigmoid_Tunable(mc_delta_sensitivity_sigmoid_constant.Y, mc_delta.Y / mc_delta_max.Y)));

            // Delta Limit
            if (Math.Abs(mc_delta.X) > mc_delta_max.X)
                mc_delta.X = Math.Abs(mc_delta.X) / mc_delta.X * mc_delta_max.X;
            if (Math.Abs(mc_delta.Y) > mc_delta_max.Y)
                mc_delta.Y = Math.Abs(mc_delta.Y) / mc_delta.Y * mc_delta_max.Y;

            // Mouse Camera Delta to Axis
            //Console.WriteLine((int)(mc_delta.X+128) + ", " + (int)(mc_delta.Y + 128));
            System.Drawing.Point translation = new System.Drawing.Point((int)(mc_delta.X + 128), (int)(mc_delta.Y + 128));

            // Delta Damping
            if (mc_delta.X != 0) {
                var dt_x = mc_delta_damping_origin.X * Math.Abs(mc_delta.X) / mc_delta.X;
                mc_delta.X = (mc_delta.X - dt_x) * (mc_delta_damping.X + mc_delta_damping2.X * Math.Abs(Sigmoid_Tunable(mc_delta_damping_sigmoid_constant.X, mc_delta.X / mc_delta_max.X))) * 0.5 + dt_x;
            }
            if (mc_delta.Y != 0) {
                var dt_y = mc_delta_damping_origin.Y * Math.Abs(mc_delta.Y) / mc_delta.Y;
                mc_delta.Y = (mc_delta.Y - dt_y) * (mc_delta_damping.Y + mc_delta_damping2.Y * Math.Abs(Sigmoid_Tunable(mc_delta_damping_sigmoid_constant.Y, mc_delta.Y / mc_delta_max.Y))) * 0.5 + dt_y;
            }
            // Delta Stopping
            if (Math.Abs(mc_delta.X) < mc_delta_stop_threshold.X)
                mc_delta.X = 0.0;
            if (Math.Abs(mc_delta.Y) < mc_delta_stop_threshold.Y)
                mc_delta.Y = 0.0;
            return translation;
        }
    }
}
