using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TronicControls
{
    public partial class ToggleButtonGroup : Panel
    {
        public void ValidateButtons(ToggleButton sender)
        {
            foreach(var control in Controls)
            {
                if (control.GetType() != typeof(ToggleButton) || control == sender) continue;
                var button = (ToggleButton)control;
                button.ToggleState = false;
                button.Invalidate();
            }
        }
    }
}
