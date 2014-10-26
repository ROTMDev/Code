// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =NumBox.cs=
// = 10/25/2014 =
// =ROTM_MU002=
  
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace ROTM_MU002.Windows
{
    public class NumBox : TextBox
    {
        bool allowSpace = false;
        public int IntValue
        {
            get { return Int32.Parse(this.Text); }
        }

        // Restricts the entry of characters to digits (including hex), the negative sign, 
        // the decimal point, and editing keystrokes (backspace). 
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string groupSeparator = numberFormatInfo.NumberGroupSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;

            // Workaround for groupSeparator equal to non-breaking space 
            if (groupSeparator == (( char )160).ToString())
            { groupSeparator = " "; }

            string keyInput = e.KeyChar.ToString();

            if (Char.IsDigit(e.KeyChar))
            { }
            else if (keyInput.Equals(decimalSeparator) || keyInput.Equals(groupSeparator) ||
                     keyInput.Equals(negativeSign))
            { }
            else if (e.KeyChar == '\b')
            { }
            //    else if ((ModifierKeys & (Keys.Control | Keys.Alt)) != 0) 
            //    { 
            //     // Let the edit control handle control and alt key combinations 
            //    } 
            else if (this.allowSpace && e.KeyChar == ' ')
            { }
            else
            {
                // Consume this invalid key and beep
                e.Handled = true;
                //    MessageBeep();
            }
        }
    }
}
