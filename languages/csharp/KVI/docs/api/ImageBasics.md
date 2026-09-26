##  Basic Operations of Digital Images

Corresponding project: ImageBasics

<img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image1.png" style="width:6.74375in;height:5.34792in" />

In the RVB Digital Image Library, the KImage class represents digital images and is an important data type. This document describes in detail the process of basic digital image operations.

## Image Data Format and Memory Layout

KImage supports the following four pixel formats:

- **BGR**: Each pixel consists of 3 bytes, storing the blue, green, and red components in order, each in the range 0–255. No alpha channel.

- **BGRA**: Each pixel consists of 4 bytes, adding an alpha (transparency) channel to BGR, in the order blue, green, red, alpha. Suitable for images requiring transparency.

- **GRAY**: Each pixel occupies only 1 byte, storing a grayscale value (0 for black, 255 for white). No color information.

- **BIN**: Binary image, each pixel occupies 1 byte, with values 0 or 255 representing black and white respectively.

In memory, image data is stored contiguously row by row, from the top‑left corner to the bottom‑right corner. The number of bytes per pixel varies by format: BGR is 3 bytes, BGRA is 4 bytes, and GRAY and BIN are 1 byte.

KImage uses **1‑byte alignment**. For BGR, BGRA, GRAY, and BIN formats, there is no extra padding between rows. The number of bytes per row equals image width $\times$ bytes per pixel, and the total data size equals height $\times$ width $\times$ bytes per pixel.

For example, a 160$\times$120 BGR image has a total data size of 160 $\times$ 120 $\times$ 3 = 57,600 bytes.

### Creating a Digital Image

To create an image, simply call the KImage constructor with the pixel format, width, and height. For example:

m_image = new KImage(PixelFormat.BGR, 160, 120);

This creates a 160‑pixel‑wide, 120‑pixel‑high image with BGR format, where each pixel consists of 3 bytes representing blue, green, and red components.

After creation, the image can be displayed in a PictureBox control using ShowImageInPictureBox. Additionally, the helper function FillRandom fills the image with random data. Its implementation is as follows:

UInt64 size = (UInt64)(image.GetHeight() \* image.GetWidth() \* image.GetBytesPerPixel());

byte\[\] data = new byte\[size\];

Random rnd = new Random();

for (UInt64 i = 0; i \< size; ++i)

data\[i\] = (byte)(rnd.Next() % 256);

image.FloodEx(data);

This function first obtains the image dimensions via GetHeight and GetWidth, then gets the bytes per pixel via GetBytesPerPixel (3 for BGR). Multiplying these gives the total image data size. Since KImage uses 1‑byte alignment with no extra row padding, stride alignment is not a concern. A byte array of the same size is created, filled with random values from 0 to 255 using Random, and finally FloodEx copies the entire array into the image buffer at once.

After calling FillRandom(m_image), call ShowImageInPictureBox again to display the image, showing a colour noise effect.

The before‑and‑after images are shown below:

<img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image2.png" style="width:2.44792in;height:1.92708in" /> <img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image3.png" style="width:2.41667in;height:1.96875in" />

Default image – solid grey (default) Random‑value image – colour snow

### 2.Cloning an Image

Using the same creation method as above, a new image is generated. Then Flood(255) fills all pixels with white (all three channels set to 255). Next, Clone creates an independent copy imNew. The original image is then filled with black using Flood(0). The original and the copy do not affect each other. Finally, ShowImageInPictureBox displays the original in picSrc and the new image in picDest, proving that Clone preserved the white‑filled state. Flood operates directly on the buffer with high efficiency; Clone is used when you need to preserve an intermediate state, avoiding later modifications to the original data.

Example code:

m_image = new KImage(PixelFormat.BGR, 160, 120);

m_image.Flood(255);

KImage imNew = m_image.Clone();

m_image.Flood(0);

//show default image

ShowImageInPictureBox(m_image, picSrc);

ShowImageInPictureBox(imNew, picDest);

The original and new images are displayed as:

<img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image4.png" style="width:2.45833in;height:1.92708in" /> <img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image5.png" style="width:2.45833in;height:1.91667in" />

Cloned new image filled with 0 Original image

### Changing Image Dimensions

Calling SetSize(200, 200) changes the image size to 200$\times$200; the pixel format remains unchanged, but the existing image data is reset. Then FillRandom fills the entire image with random data, and finally ShowImageInPictureBox displays it in picDest, showing a 200$\times$200 colour noise effect. This example demonstrates that SetSize can dynamically resize the image, after which the data must be refilled to show valid content.

Example code:

m_image = new KImage(PixelFormat.BGR, 160, 120);

m_image.Flood(255);

*//show default image*

ShowImageInPictureBox(m_image, picSrc);

m_image.SetSize(200, 200);

FillRandom(m_image);

ShowImageInPictureBox(m_image, picDest);

The same image at different sizes:

<img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image6.png" style="width:2.5in;height:1.97917in" /> <img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image7.png" style="width:3.02153in;height:3.03056in" />

Before resizing After resizing and filling with random values

### Changing Pixel Format

Using the same creation method as above, a BGR image is created and filled with random data via FillRandom, then displayed in picSrc as a colour noise image. Then Cast(PixelFormat.Gray) converts the image from BGR to grayscale. During conversion, each pixel's brightness (grayscale) is computed from its BGR components; colour information is lost but the image structure is preserved. Finally, the image is displayed in picDest, showing the grayscale version of the noise image.

This example shows that Cast can convert between different pixel formats, useful when you need to turn a colour image into grayscale for further processing.

Example code:

m_image = new KImage(PixelFormat.BGR, 160, 120);

FillRandom(m_image);

ShowImageInPictureBox(m_image, picSrc);

m_image.Cast(PixelFormat.Gray);

ShowImageInPictureBox(m_image, picDest);

Images in different pixel formats:

<img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image8.png" style="width:2.45833in;height:1.90625in" /> <img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image9.png" style="width:2.53125in;height:2.16667in" />

Original BGR colour image Grayscale image

## Viewing Basic Image Properties

After creating a new image object, use the Load method to load image content from a file. This method takes a file path as a parameter and returns true on success, false on failure. Once loaded, the image data is stored in the KImage internal buffer and can be displayed and have its properties extracted simultaneously.

Call GetWidth, GetHeight, GetPixelFormat, GetDepth, and GetChannels to obtain the image's dimensions, pixel format, bit depth, and number of channels, and output them to a text box.

Load supports common image formats such as JPEG, PNG, and BMP, and is the main entry point for KImage to obtain image data from external files.

Example code:

string strFile = "..\\samples\\colorwave.jpg";

m_image = new KImage();

if (m_image.Load(strFile))

{

ShowImageInPictureBox(m_image, picSrc);

string s;

s = \$"Width: {m_image.GetWidth()}\r\n";

tbxOutput.Text += s;

s = \$"Height: {m_image.GetHeight()}\r\n";

tbxOutput.Text += s;

s = \$"Pixel Format: {GetPixelFormatDesc((int)m_image.GetPixelFormat())}\r\n";

tbxOutput.Text += s;

s = \$"Pixel Depth: {m_image.GetDepth()}\r\n";

tbxOutput.Text += s;

s = \$"Channels: {m_image.GetChannels()}\r\n";

tbxOutput.Text += s;

}

else

{

tbxOutput.Text += "image loading failed\r\n";

}

Image property output:

<img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image10.png" style="width:1.73958in;height:1.3125in" />

## Cloning a Shadow Image

When Clone is called with the parameter true, it performs a shadow clone (shallow copy). The new KImage object shares the same data buffer as the original image, rather than copying the data. This means that modifying pixel data in either object will affect the other. The advantage of a shadow clone is that it consumes no extra memory and is fast to create, suitable for scenarios where multiple objects need to reference the same data but have independent properties (e.g., different display parameters). However, the lifecycle rule must be strictly followed: all shadow images must be destroyed before the original image – i.e., ensure the original image remains valid while any shadow image is in use.

In the example, first Flood(255) fills the image with white, then a shadow clone imNew is created. Subsequently, FillRandom fills the original image with random data; because they share the same buffer, imNew will also display random noise instead of retaining white.

Example code:

m_image = new KImage(PixelFormat.BGR, 160, 120);

m_image.Flood(255);

*// Note: In shadow clone mode, all shadow images must be destroyed before the original image.*

*// Otherwise, using the shadow image may cause a program crash.*

KImage imNew = m_image.Clone(true);

FillRandom(m_image);

ShowImageInPictureBox(m_image, picSrc);

ShowImageInPictureBox(imNew, picDest);

If a fully independent copy is needed, call Clone(false) or simply Clone() (which defaults to a deep copy), in which case the data is completely copied and neither affects the other.

The original and shadow images are displayed as:

<img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image11.png" style="width:2.47917in;height:1.95833in" /> <img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image12.png" style="width:2.51042in;height:1.91667in" />

原图像 影子图像

### Extracting a Single Channel Image

After creating a new image and loading a BGR colour image file via Load, individual colour channels can be extracted using the Split method. In the example, the loaded waterdrop.png is a BGRA image. A grayscale image imgray of the same size is created as a container for one channel. The call Split(imgray, null, null) – where the three parameters correspond to red, green, and blue output targets in that order – passes imgray as the first parameter to receive the red channel; the latter two are null to ignore the green and blue channels. This extracts the red channel image and displays it. The parameter order of Split is fixed as red, green, blue (and optionally alpha). By selectively passing target images, single‑channel extraction is achieved. This operation is commonly used in image feature analysis and pre‑processing for channel separation.

Example code:

string strFile = "..\\samples\\waterdrop.png";

m_image = new KImage();

if (m_image.Load(strFile))

{

ShowImageInPictureBox(m_image, picSrc);

KImage imgray = new KImage(PixelFormat.Gray, m_image.GetWidth(), m_image.GetHeight());

m_image.Split(imgray, null, null);

ShowImageInPictureBox(imgray, picDest);

}

Original and red‑channel images:

<img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image13.png" style="width:2.49028in;height:1.92708in" /> <img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image14.png" style="width:2.10417in;height:1.88611in" />

Original (colour image) Red channel (grayscale)

### 8.Merging Single‑Channel Images

Merge is the inverse operation of Split, used to combine three independent grayscale images into a colour image. In the example, three grayscale images imRed, imGreen, imBlue of size 120\times100 are created, along with a BGR image m_image of the same size. FillRandom(imRed) fills only the red‑channel grayscale image with random data, while the green and blue channels remain at their default (all black). Then m_image.Merge(imRed, imGreen, imBlue) combines the three grayscale images into a BGR colour image, with parameters corresponding to red, green, and blue input sources in order. Since only the red channel contains random data, the merged result appears as a red noise effect. The parameter order of Merge is fixed as red, green, blue, matching the output order of Split. Together they enable channel separation and recombination, often used in multi‑channel image processing and pseudo‑colour synthesis.

Example code:

KImage imRed = new KImage(PixelFormat.Gray, 120, 100);

KImage imGreen = new KImage(PixelFormat.Gray, 120, 100);

KImage imBlue = new KImage(PixelFormat.Gray, 120, 100);

m_image = new KImage(PixelFormat.BGR, 120, 100);

FillRandom(imRed);

ShowImageInPictureBox(imRed, picSrc);

m_image.Merge(imRed, imGreen, imBlue);

ShowImageInPictureBox(m_image, picDest);

The red‑channel grayscale image and the merged BGR image:

<img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image15.png" style="width:2.47917in;height:1.94792in" /> <img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image16.png" style="width:2.52153in;height:2.07292in" />

Random‑filled grayscale image Merged colour image

### 9.Loading an Image from Memory

Besides loading directly from a file path via Load, images can also be loaded from a byte array in memory, which is more flexible in scenarios like network transmission or embedded resources. In the example, File.ReadAllBytes first reads the entire image file into memory, obtaining a byte array fileData. Then a KImage instance is created and its LoadImageInMemory method is called with the byte array as the parameter to decode the image. On success, ShowImageInPictureBox displays the image; on failure, an error message is output in the text box. LoadImageInMemory supports common formats like JPEG, PNG, and BMP. The difference from Load is that the data source is memory rather than disk, which is suitable for image data already resident in memory or for performance‑sensitive scenarios that avoid repeated disk I/O.

Example code:

byte\[\] fileData = File.ReadAllBytes("..\\samples\\lenna.png");

if (null != fileData)

{

m_image = new KImage();

if (!m_image.LoadImageInMemory(fileData))

{

tbxOutput.Text += "image loading failed\r\n";

}

else

{

ShowImageInPictureBox(m_image, picDest);

}

}

Decoded image:

<img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image17.png" style="width:3.10764in;height:3.00208in" />

### 10. Exporting an Image to Memory as Encoded Data

The ExportImageToMemory method encodes a KImage and exports it to a memory buffer, suitable for passing image data to other components or for network transmission. In the example, the image is first loaded and converted to BGR format. Then a MemoryStream is created to receive the data. The call to ExportImageToMemory takes the encoding format (KImage.IMF_JPG), a quality parameter (0 for default), and a pre‑allocated byte array data. Internally, the image is encoded as JPEG and the data is written into the array; the method returns the actual number of bytes written. The valid data is then written to the MemoryStream, and using the .NET standard library, Image.FromStream reconstructs a System.Drawing.Image object, which is finally set as the background image of picDest. This demonstrates interoperability between KImage and the .NET standard image processing library. ExportImageToMemory supports multiple encoding formats such as JPEG, PNG, and BMP; the quality parameter ranges from 0 to 100 (only effective for lossy formats like JPEG). The ref array parameter must be pre‑allocated with enough space by the caller, and the return value indicates the actual data length so that the valid portion can be extracted.

Example code:

string strFile = "..\\samples\\lenna.png";

m_image = new KImage();

bool ret = m_image.Load(strFile);

Pool.Assert(ret);

ShowImageInPictureBox(m_image, picSrc);

m_image.Cast(PixelFormat.BGR);

MemoryStream ms = new MemoryStream();

UInt64 size = (UInt64)(m_image.GetHeight() \* CalcBitmapPicth(m_image.GetWidth(), m_image.GetBytesPerPixel() \* 8));

byte\[\] data = new byte\[size\];

uint sze = m_image.ExportImageToMemory(KImage.IMF_JPG, 0, ref data);

Pool.Assert(size \> 0);

ms.Write(data, 0, (int)sze);

Image im = Image.FromStream(ms);

picDest.BackgroundImage = im;

Image decoded by the Image class:

<img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\ImageBasics_media/media/image18.png" style="width:2.46875in;height:2.15625in" />
