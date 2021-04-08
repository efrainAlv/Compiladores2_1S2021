using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto2.AST
{
    class Consola
    {
            public System.Windows.Forms.RichTextBox richTextBox2 = new System.Windows.Forms.RichTextBox();

            public Consola()
            {
                this.richTextBox2 = new System.Windows.Forms.RichTextBox();

                this.richTextBox2.AcceptsTab = true;
                this.richTextBox2.AccessibleName = "public static";
                this.richTextBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
                this.richTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                this.richTextBox2.BackColor = System.Drawing.SystemColors.WindowText;
                this.richTextBox2.ForeColor = System.Drawing.SystemColors.InactiveBorder;
                this.richTextBox2.Location = new System.Drawing.Point(12, 495);
                this.richTextBox2.Name = "richTextBox2";
                this.richTextBox2.ReadOnly = true;
                this.richTextBox2.Size = new System.Drawing.Size(1058, 120);
                this.richTextBox2.TabIndex = 2;
                this.richTextBox2.Text = "";

            }

            


    }
}
