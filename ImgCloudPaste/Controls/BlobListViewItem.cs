using Azure.Storage.Blobs.Models;
using System.Windows.Forms;

namespace ImgCloudPaste.Controls
{
    internal class BlobListViewItem : ListViewItem
    {
        public BlobListViewItem(BlobItem blobItem) : base(blobItem.Name)
        {
            Blob = blobItem;
            SubItems.Add(new ListViewSubItem(this, blobItem.Name));
            SubItems.Add(new ListViewSubItem(this, blobItem.Properties.CreatedOn.ToString()));
        }

        public BlobItem Blob { get; }
    }
}
