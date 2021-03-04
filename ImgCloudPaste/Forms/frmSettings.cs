using Azure.Storage.Blobs;
using ImgCloudPaste.Services;
using System;
using System.Windows.Forms;
using WinForms.Library;

namespace ImgCloudPaste.Forms
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        public Settings Settings { get; set; }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            var binder = new ControlBinder<Settings>();
            binder.Add(tbConnectionString, model => model.ConnectionString);
            binder.Add(tbContainerName, model => model.ContainerName);
            binder.Document = Settings;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                var client = new BlobContainerClient(tbConnectionString.Text, tbContainerName.Text);
                await client.CreateIfNotExistsAsync();
                MessageBox.Show("Storage account info is valid.");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}
