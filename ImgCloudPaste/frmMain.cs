using ImgCloudPaste.Forms;
using ImgCloudPaste.Services;
using JsonSettings.Library;
using System;
using System.Windows.Forms;

namespace ImgCloudPaste
{
    public partial class frmMain : Form
    {
        private Settings _settings;
        private ImageCloudPaste _cloudPaste;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            _settings = SettingsBase.Load<Settings>();
            _cloudPaste = new ImageCloudPaste(_settings);
        }

        private async void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V && e.Control)
            {
                try
                {
                    var image = Clipboard.GetImage();
                    pictureBox1.Image = image;
                    await _cloudPaste.UploadAndGetUrlsAsync(image);
                    btnCopyMarkdown.Visible = true;
                    btnCopyRaw.Visible = true;
                    tabControl1.SelectedIndex = 0;
                }
                catch (Exception exc)
                {
                    btnCopyMarkdown.Visible = false;
                    btnCopyRaw.Visible = false;
                    MessageBox.Show($"Error uploading image: {exc.Message}");  
                }
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(_cloudPaste.MarkdownUrl);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            frmSettings dlg = new frmSettings();
            dlg.Settings = _settings.Clone() as Settings;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _settings = dlg.Settings;
                _settings.Save();
            }
        }

        private void btnCopyRaw_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_cloudPaste.RawUrl);
        }

        private void btnCopyMarkdown_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_cloudPaste.MarkdownUrl);
        }
    }
}
