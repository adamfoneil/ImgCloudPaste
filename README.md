I often need to paste screenshots in places that don't support image paste natively, such as GitHub readmes and wikis. In the past, I've manually saved images in blob storage, then manually copied links from Azure Storage Explorer. I finally got too tired of that, and made a little WinForms app to automate the blob upload and link retrieval.

![img](https://adamosoftware.blob.core.windows.net/images/BEV8P39JNN.png)

This would be a neat idea for a Blazor Web Assembly app I think, but WinForms is the lower hanging fruit for me, so I went this direction. I did find an existing online service for this [Pasteboard](https://pasteboard.co/), but it wasn't easy enough to get image links, and it doesn't have a markdown option. This is the kind of service I'd like to build in Blazor, but with

My solution requires you to have an Azure storage account. Likewise, as a desktop app, it's really hard to provide an easy deployment. My code signing certificate (which was very difficult to get initially) recently stopped working for some reason. That's another story.

Here are a few noteable things about my solution:

- The core service is [ImageCloudPaste](https://github.com/adamfoneil/ImgCloudPaste/blob/master/ImgCloudPaste/Services/ImageCloudPaste.cs). I was mindful of later wanting to move to Blazor or WPF, so I made sure to put the important logic in one place with minimal dependencies.

- That said, my [Settings](https://github.com/adamfoneil/ImgCloudPaste/blob/master/ImgCloudPaste/Models/Settings.cs) model does depend on my [Json Settings](https://github.com/adamfoneil/JsonSettings) library, which in turn depends on Newtonsoft. I used this because this app uses Azure connection strings. These are pretty sensitive, so I use DPAPI encryption. My Json settings library supports this, but I'm not wild about the dependency footprint of my own library. I would likely want to rework this so the encryption capability is injected.

- The [settings dialog](https://github.com/adamfoneil/ImgCloudPaste/blob/master/ImgCloudPaste/Forms/frmSettings.cs) is here. It's invoked from the main form [here](https://github.com/adamfoneil/ImgCloudPaste/blob/master/ImgCloudPaste/frmMain.cs#L53).

- I had some trouble getting the actual paste functionality to work on the main form. I got it working setting `KeyPreview = true` and handling the [KeyDown](https://github.com/adamfoneil/ImgCloudPaste/blob/master/ImgCloudPaste/frmMain.cs#L26) event. I tried overriding `WndProc` and checking for `WM_PASTE` but I couldn't get that working. I wanted a really flexible paste mechanism that would accept the paste no matter where focus is at the moment, and not rely on a certain key stroke. But like I said, the only thing I could make work was handling the `KeyDown` event.
