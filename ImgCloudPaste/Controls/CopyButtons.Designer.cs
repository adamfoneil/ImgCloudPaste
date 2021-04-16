
namespace ImgCloudPaste.Controls
{
    partial class CopyButtons
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopyButtons));
            this.btnCopyMarkdown = new System.Windows.Forms.Button();
            this.btnCopyRaw = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCopyMarkdown
            // 
            this.btnCopyMarkdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyMarkdown.Image = ((System.Drawing.Image)(resources.GetObject("btnCopyMarkdown.Image")));
            this.btnCopyMarkdown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopyMarkdown.Location = new System.Drawing.Point(98, 3);
            this.btnCopyMarkdown.Name = "btnCopyMarkdown";
            this.btnCopyMarkdown.Size = new System.Drawing.Size(87, 25);
            this.btnCopyMarkdown.TabIndex = 4;
            this.btnCopyMarkdown.Text = "Markdown";
            this.btnCopyMarkdown.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCopyMarkdown.UseVisualStyleBackColor = true;
            // 
            // btnCopyRaw
            // 
            this.btnCopyRaw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyRaw.Image = ((System.Drawing.Image)(resources.GetObject("btnCopyRaw.Image")));
            this.btnCopyRaw.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopyRaw.Location = new System.Drawing.Point(5, 3);
            this.btnCopyRaw.Name = "btnCopyRaw";
            this.btnCopyRaw.Size = new System.Drawing.Size(87, 25);
            this.btnCopyRaw.TabIndex = 3;
            this.btnCopyRaw.Text = "Raw Url";
            this.btnCopyRaw.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCopyRaw.UseVisualStyleBackColor = true;
            // 
            // CopyButtons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCopyMarkdown);
            this.Controls.Add(this.btnCopyRaw);
            this.Name = "CopyButtons";
            this.Size = new System.Drawing.Size(188, 29);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCopyMarkdown;
        private System.Windows.Forms.Button btnCopyRaw;
    }
}
