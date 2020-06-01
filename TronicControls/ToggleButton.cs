using System;
using System.IO;
using System.Resources;
using System.Drawing;
using System.Windows.Forms;

namespace TronicControls
{
    public partial class ToggleButton : Control
    {
        public ToggleButton()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint
                    | ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            switch (ToggleStyle)
            {
                case ToggleButtonStyle.Slider:
                    DrawSliderStyle(e);
                    break;
                case ToggleButtonStyle.Classic:
                    DrawClassicStyle(e);
                    break;
            }            
        }

        #region ClassicStyle
        private void DrawClassicStyle(PaintEventArgs e)
        {
            var sf = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            if (ToggleState)
            {
                // Button is switched on
                if (ImageOn != null)
                {
                    e.Graphics.DrawImage(ImageOn, ClientRectangle);
                } else
                {
                    using (var sb = new SolidBrush(BackColorOn)) e.Graphics.FillRectangle(sb, ClientRectangle);                                            
                }
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                                            SystemColors.ControlDark, BorderSize, ButtonBorderStyle.Outset,
                                            SystemColors.ControlDark, BorderSize, ButtonBorderStyle.Outset,
                                            BorderColor, 1, ButtonBorderStyle.Solid,
                                            BorderColor, 1, ButtonBorderStyle.Solid);

                using (var sbFore = new SolidBrush(ForeColorOn)) e.Graphics.DrawString(TextOn, Font, sbFore, ClientRectangle, sf);                                
            }
            else
            {
                // Button is switched off
                if (ImageOff != null)
                {
                    e.Graphics.DrawImage(ImageOff, ClientRectangle);
                } else
                {
                    using (var sb = new SolidBrush(BackColorOff)) e.Graphics.FillRectangle(sb, ClientRectangle);                    
                }
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                                            BorderColor, 1, ButtonBorderStyle.Solid,
                                            BorderColor, 1, ButtonBorderStyle.Solid,
                                            SystemColors.ControlLight, BorderSize, ButtonBorderStyle.Outset,
                                            SystemColors.ControlLight, BorderSize, ButtonBorderStyle.Outset);

                using (var sbFore = new SolidBrush(ForeColorOff)) e.Graphics.DrawString(TextOff, Font, sbFore, ClientRectangle, sf);                                
            }
        }
        #endregion

        #region SliderStyle
        private Point _p1, _p2, _p3, _p4;

        private Point[] SliderPoints(int padX, Rectangle rect)
        {
            Point[] slidePoints = new Point[4];            

            _p1 = new Point(padX, rect.Y);
            _p2 = new Point(padX, rect.Bottom - 1);
            _p4 = new Point((_p1.X + (rect.Width / 2)) - 1, rect.Y);
            _p3 = new Point(_p4.X - 1, rect.Bottom - 1);

            if (_p4.X == rect.Right)
                _p3 = new Point(_p4.X, rect.Bottom);

            slidePoints[0] = _p1;
            slidePoints[1] = _p2;
            slidePoints[2] = _p3;
            slidePoints[3] = _p4;
            return slidePoints;
        }

        private void DrawSliderStyle(PaintEventArgs e)
        {
            var padX = 0;
            if (ToggleState) padX = ClientRectangle.Right - (ClientRectangle.Width / 2);            

            using (var sb = new SolidBrush(BackColor))
            {
                e.Graphics.FillRectangle(sb, ClientRectangle);
            }

            // Paint border
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, BorderColor, BorderStyle);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var clr = padX == 0 ? BackColorOff : BackColorOn;

            using (var sb = new SolidBrush(clr))
            {
                e.Graphics.FillPolygon(sb, SliderPoints(padX, ClientRectangle));
            }

            var rect = new Rectangle(_p1, new Size(_p3.X - _p1.X, _p2.Y - _p1.Y));
            var sf = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };

            if (padX == 0)
            {
                using (var sbFore = new SolidBrush(ForeColorOff)) e.Graphics.DrawString(TextOff, Font, sbFore, rect, sf);                
            }
            else
            {
                using (var sbFore = new SolidBrush(ForeColorOn)) e.Graphics.DrawString(TextOn, Font, sbFore, rect, sf);
            }
        }
        #endregion

        #region EventHandlers
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (!ToggleState && Parent.GetType() == typeof(ToggleButtonGroup))
            {
                var group = (ToggleButtonGroup)Parent;
                group.ValidateButtons(this);
            }
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

        private Image _imageOff = null;
        private Image _imageOn = null;

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

        public Image ImageOn
        {
            get { return _imageOn; }
            set { _imageOn = value; Invalidate(); }
        }        

        public Image ImageOff
        {
            get { return _imageOff; }
            set { _imageOff = value; Invalidate(); }
        }

        /// <summary>
        /// Vypln pokud konvertujes do Vizwebu a pouzivas ImageOn
        /// </summary>
        public string ImageOnName { get; set; } = null;
        /// <summary>
        /// Vypln pokud konvertujes do Vizwebu a pouzivas ImageOff
        /// </summary>
        public string ImageOffName { get; set; } = null;

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
