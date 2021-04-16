using System;
using System.Windows.Forms;

namespace ImgCloudPaste.Controls
{
    public partial class CopyButtons : UserControl
    {
        public CopyButtons()
        {
            InitializeComponent();
            btnCopyRaw.Click += (sender, args) => RawUrlClicked?.Invoke(sender, args);
            btnCopyMarkdown.Click += (sender, args) => MarkdownUrlClicked?.Invoke(sender, args);
        }

        public event EventHandler RawUrlClicked;
        public event EventHandler MarkdownUrlClicked;

        public new bool Enabled
        {
            get => btnCopyMarkdown.Enabled;
            set
            {
                btnCopyMarkdown.Enabled = value;
                btnCopyRaw.Enabled = value;
            }
        }
    }
}
