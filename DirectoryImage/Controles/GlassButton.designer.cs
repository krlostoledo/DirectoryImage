﻿
    namespace Glass
    {
        partial class GlassButton
        {
            /// <summary>
            /// Required designer variable.
            /// </summary>
            private System.ComponentModel.IContainer components = null;

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (imageButton != null)
                    {
                        imageButton.Parent.Dispose();
                        imageButton.Parent = null;
                        imageButton.Dispose();
                        imageButton = null;
                    }
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
                base.Dispose(disposing);
            }

            #region Component Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify 
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
                this.components = new System.ComponentModel.Container();
                this.timer = new System.Windows.Forms.Timer(this.components);
                this.SuspendLayout();
                // 
                // timer
                // 
                this.timer.Tick += new System.EventHandler(this.timer_Tick);
                this.ResumeLayout(false);

            }

            #endregion

            private System.Windows.Forms.Timer timer;
        }
    }
