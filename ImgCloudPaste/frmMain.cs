using ImgCloudPaste.Controls;
using ImgCloudPaste.Forms;
using ImgCloudPaste.Models;
using ImgCloudPaste.Services;
using JsonSettings.Library;
using System;
using System.Linq;
using System.Windows.Forms;

namespace ImgCloudPaste
{
    public partial class frmMain : Form
    {
        private Settings _settings;
        private ImageCloudPaste _cloudPaste;
        private ImageCloudPaste.Result _currentImage;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            _settings = SettingsBase.Load<Settings>();
            _cloudPaste = new ImageCloudPaste(_settings);
            ListBlobs();
        }

        private void ListBlobs()
        {
            lvBlobs.Items.Clear();
            lvBlobs.Groups.Clear();

            var blobGroups = _cloudPaste.GetBlobs()
                .OrderByDescending(blob => blob.Properties.CreatedOn)
                .Take(100)
                .GroupBy(blob => blob.Properties.CreatedOn.Value.DateTime);
                
            // grouping doesn't work for some reason
            foreach (var grp in blobGroups)
            {
                int groupIndex = lvBlobs.Groups.Add(new ListViewGroup(grp.Key.ToString("ddd M/d"), HorizontalAlignment.Left));
                var items = grp.Select(blob => new BlobListViewItem(blob) { Group = lvBlobs.Groups[groupIndex] }).ToArray();
                lvBlobs.Items.AddRange(items);
            }            
        }

        private async void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V && e.Control)
            {
                try
                {
                    var image = Clipboard.GetImage();
                    pictureBox1.Image = image;
                    _currentImage = await _cloudPaste.AddAsync(image);
                    copyButtons1.Enabled = true;
                    tabControl1.SelectedIndex = 0;
                }
                catch (Exception exc)
                {
                    copyButtons1.Enabled = false;
                    MessageBox.Show($"Error uploading image: {exc.Message}");  
                }
            }
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

        private async void lvBlobs_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                var blobItem = e.Item as BlobListViewItem;
                if (blobItem != null)
                {
                    var result = await _cloudPaste.GetBlobImageAsync(blobItem.Blob);
                    pbBlob.Image = result.image;
                    _currentImage = result.result;
                    lblBlobName.Text = blobItem.Blob.Name;
                }
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetImage(pbBlob.Image);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                pbBlob.Image = Clipboard.GetImage();
                _currentImage = await _cloudPaste.UpdateAsync(lblBlobName.Text, pbBlob.Image);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void WithCurrentImage(Action<ImageCloudPaste.Result> action)
        {
            if (_currentImage != null)
            {
                action.Invoke(_currentImage);
            }
            else
            {
                MessageBox.Show("No current image");
            }
        }

        private void copyButtons1_RawUrlClicked(object sender, EventArgs e)
        {
            WithCurrentImage((img) => Clipboard.SetText(img.RawUrl));
        }

        private void copyButtons1_MarkdownUrlClicked(object sender, EventArgs e)
        {
            WithCurrentImage((img) => Clipboard.SetText(img.MarkdownUrl));
        }
    }
}
