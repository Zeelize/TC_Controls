using System;
using System.Drawing;
using System.Windows.Forms;

namespace TronicControls
{
    public partial class ToggleButton : Control
    {
        #region Variables
        private Rectangle _contentRectangle = Rectangle.Empty;
        private int _padx = 0;
        #endregion

        public ToggleButton()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint
                    | ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.ResetClip();
            switch (ToggleStyle)
            {
                case ToggleButtonStyle.Slider:
                    MinimumSize = new Size(40, 20);
                    _contentRectangle = e.ClipRectangle;
                    DrawSliderStyle(e);
                    break;
                case ToggleButtonStyle.Classic:
                    MinimumSize = new Size(20, 20);
                    _contentRectangle = e.ClipRectangle;
                    DrawClassicStyle(e);
                    break;
            }

            base.OnPaint(e);
        }

        #region ClassicStyle
        private void DrawClassicStyle(PaintEventArgs e)
        {
            e.Graphics.ResetClip();
            _contentRectangle = e.ClipRectangle;

            var sf = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            if (ToggleState)
            {
                // Button is switched on
                using (var sb = new SolidBrush(BackColorOn))
                {
                    e.Graphics.FillRectangle(sb, e.ClipRectangle);
                    //ControlPaint.DrawBorder(e.Graphics, _contentRectangle, BorderColor, BorderStyle);

                    ControlPaint.DrawBorder(e.Graphics, _contentRectangle,
                                            SystemColors.ControlDark, BorderSize, ButtonBorderStyle.Outset,
                                            SystemColors.ControlDark, BorderSize, ButtonBorderStyle.Outset,
                                            BorderColor, 1, ButtonBorderStyle.Solid,
                                            BorderColor, 1, ButtonBorderStyle.Solid);

                    e.Graphics.DrawString(TextOn, Font, new SolidBrush(ForeColorOn), _contentRectangle, sf);
                }
            }
            else
            {
                // Button is switched off
                using (var sb = new SolidBrush(BackColorOff))
                {
                    e.Graphics.FillRectangle(sb, e.ClipRectangle);
                    ControlPaint.DrawBorder(e.Graphics, _contentRectangle,
                                            BorderColor, 1, ButtonBorderStyle.Solid,
                                            BorderColor, 1, ButtonBorderStyle.Solid,
                                            SystemColors.ControlLight, BorderSize, ButtonBorderStyle.Outset,
                                            SystemColors.ControlLight, BorderSize, ButtonBorderStyle.Outset);

                    e.Graphics.DrawString(TextOff, Font, new SolidBrush(ForeColorOff), _contentRectangle, sf);
                }
            }
        }
        #endregion

        #region SliderStyle
        // ReSharper disable InconsistentNaming
        private readonly Point[] slidePoints = new Point[4];
        private Point p1, p2, p3, p4;
        // ReSharper restore InconsistentNaming

        private Point[] SliderPoints()
        {
            p1 = new Point(_padx, _contentRectangle.Y);
            p2 = new Point(_padx, _contentRectangle.Bottom - 1);
            p4 = new Point((p1.X + (_contentRectangle.Width / 2)) - 1, _contentRectangle.Y);
            p3 = new Point(p4.X - 1, _contentRectangle.Bottom - 1);

            if (p4.X == _contentRectangle.Right)
                p3 = new Point(p4.X, _contentRectangle.Bottom);

            slidePoints[0] = p1;
            slidePoints[1] = p2;
            slidePoints[2] = p3;
            slidePoints[3] = p4;
            return slidePoints;
        }

        private void DrawSliderStyle(PaintEventArgs e)
        {
            e.Graphics.ResetClip();
            _contentRectangle = e.ClipRectangle;

            if (ToggleState)
                _padx = _contentRectangle.Right - (_contentRectangle.Width / 2);
            else
                _padx = 0;

            using (var sb = new SolidBrush(BackColor))
            {
                e.Graphics.FillRectangle(sb, e.ClipRectangle);
            }

            // Paint border
            ControlPaint.DrawBorder(e.Graphics, _contentRectangle, BorderColor, BorderStyle);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var clr = _padx == 0 ? BackColorOff : BackColorOn;

            using (var sb = new SolidBrush(clr))
            {
                e.Graphics.FillPolygon(sb, SliderPoints());
            }

            var rect = new Rectangle(p1, new Size(p3.X - p1.X, p2.Y - p1.Y));
            var sf = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };

            if (_padx == 0)
            {
                //e.Graphics.DrawString(TextOff, Font, new SolidBrush(ForeColorOff), new PointF(_padx + ((_contentRectangle.Width / 2) / 6), _contentRectangle.Y + (_contentRectangle.Height / 4)));
                e.Graphics.DrawString(TextOff, Font, new SolidBrush(ForeColorOff), rect, sf);
            }
            else
            {
                //e.Graphics.DrawString(TextOn, Font, new SolidBrush(ForeColorOn), new PointF(_padx + ((_contentRectangle.Width / 2) / 4), _contentRectangle.Y + (_contentRectangle.Height / 4)));
                e.Graphics.DrawString(TextOn, Font, new SolidBrush(ForeColorOn), rect, sf);
            }
        }
        #endregion

        #region EventHandlers
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            ToggleState = !ToggleState;

            Invalidate();
        }
        #endregion

        #region Properties
        private Color _foreColorOn = Color.Green;
        private Color _backColorOn = SystemColors.ControlLight;
        private string _textOn = "Z";

        private Color _foreColorOff = Color.Red;
        private Color _backColorOff = SystemColors.ControlLight;
        private string _textOff = "V";

        private bool _toggleState = false;
        private ButtonBorderStyle _border = ButtonBorderStyle.Solid;
        private Color _borderColor = Color.Black;
        private int _borderSize = 2;
        private ToggleButtonStyle _toggleStyle = ToggleButtonStyle.Classic;

        public ButtonBorderStyle BorderStyle
        {
            get { return _border; }
            set { _border = value; Invalidate(); }
        }

        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; Invalidate(); }
        }

        public int BorderSize
        {
            get { return _borderSize; }
            set { _borderSize = value; Invalidate(); }
        }

        public bool ToggleState
        {
            get { return _toggleState; }
            set { _toggleState = value; Invalidate(); }
        }

        public ToggleButtonStyle ToggleStyle
        {
            get { return _toggleStyle; }
            set { _toggleStyle = value; Invalidate(); }
        }

        public string TextOn
        {
            get { return _textOn; }
            set { _textOn = value; Invalidate(); }
        }

        public Color ForeColorOn
        {
            get { return _foreColorOn; }
            set { _foreColorOn = value; Invalidate(); }
        }

        public Color BackColorOn
        {
            get { return _backColorOn; }
            set { _backColorOn = value; Invalidate(); }
        }

        public string TextOff
        {
            get { return _textOff; }
            set { _textOff = value; Invalidate(); }
        }

        public Color ForeColorOff
        {
            get { return _foreColorOff; }
            set { _foreColorOff = value; Invalidate(); }
        }

        public Color BackColorOff
        {
            get { return _backColorOff; }
            set { _backColorOff = value; Invalidate(); }
        }

        public enum ToggleButtonStyle
        {
            Classic,
            Slider
        }
        #endregion
    }
}
