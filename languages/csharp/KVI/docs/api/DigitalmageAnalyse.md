## Digital Image Analysis

Corresponding project: Analyze

<img src="D:\mysrc\kingpool\Universal Take\csharp\docs\PART1 KVI\examples\md\DigitalmageAnalyse_media/media/image1.png" style="width:6.74653in;height:5.39167in" />The core task of digital image analysis is to extract useful information from images and convert unstructured image data into structured data. Images are usually stored in computers as multidimensional arrays. Pixel values only represent brightness or color and do not carry explicit semantics by themselves, so they cannot be directly used for measurement, decision-making, or control. Therefore, images are typical unstructured or low-level data. The analysis process gradually reduces data dimensions and increases the semantic level through a series of algorithms: the pre-processing stage eliminates noise and corrects illumination and geometric distortion; the segmentation stage divides the image into foreground and background or several regions; the feature extraction stage computes geometric, grayscale, texture, color, and other attributes for each region; the description and classification stage organizes features into vectors, labels, or relationships. The final output is a structured representation, such as object lists, coordinates, area, perimeter, orientation, circularity, count, category, confidence, relationship tables, etc. According to semantic level, it can be divided into pixel-level statistics, region-level attributes, object-level recognition, and scene-level understanding. Structured results can be directly stored in databases, input into control systems, or used for decision-making. For example, cell image analysis outputs cell count, position, area, and circularity; industrial inspection outputs defect location, type, and size; face recognition outputs identity and confidence. Traditional methods rely on manually designed features, while deep learning methods can learn features end-to-end and directly output structured results such as categories, detection boxes, and segmentation masks. Therefore, digital image analysis is a bridge connecting image data and upper-layer applications. Its value lies in transforming "visible" pixels into structured information that can be "calculated, queried, and used for decisions."

## Binary Image Features

The goal of digital image analysis is to extract structured information from images and convert pixel data into quantifiable, comparable, and decision-ready attributes. After binarization, the foreground object consists of pixels with value 255, and its geometric features such as shape, position, orientation, and size can be described by a series of metrics. The Dia class provides a set of static methods for calculating these attributes, including density, circularity, slope, centroid, bounding box, and bounding rectangle. These attributes are commonly used for target classification, shape recognition, position localization, and quality inspection.

The example first loads the triangle.png image and converts it to grayscale, then uses the Poisson minimum error method for binarization, and then calls a series of static methods of the Dia class to calculate geometric properties such as density, circularity, slope, centroid, bounding box, and bounding rectangle of the foreground object in the binary image, and outputs the results to a text box.

string strFile = "..\\samples\\triangle.png";

KImage img = new KImage();

bool b = img.Load(strFile);

Pool.Assert(b);

img.Cast(PixelFormat.Gray);

Dip.MinError(img.Handle, MinErrorMode.Poinssen);

string s = "";

s += \$"The Density is {Dia.Density(img.Handle, (int)BlobPart.Whole)} \r\n";

s += \$"The Circularity is {Dia.Circular(img.Handle, IntPtr.Zero)} \r\n";

s += \$"The Slope is {Dia.Slope(img.Handle, IntPtr.Zero)} \r\n";

s += \$"The Centoid is {Dia.Centroid(img.Handle, IntPtr.Zero).ToString()} \r\n";

RvBox2D box = Dia.GetBoundBox(img.Handle, IntPtr.Zero);

s += \$"The Box2d: cx = {box.cx}, cy = {box.cy}, angle = {box.angle}, width = {box.width}, height = {box.height} \r\n";

s += \$"The Bound Rect is {Dia.GetBoundRect(img.Handle, IntPtr.Zero).ToString()} \r\n";

tbxOutput.Text = s;

**Function Description**

Dia.Density(handle, part): Calculates the density (fill ratio) of the foreground object, i.e., the ratio of the number of foreground pixels to the area of its bounding rectangle. The second parameter is the BlobPart enumeration. In the example, BlobPart.Whole is passed, indicating density is calculated for the entire image foreground. The density value ranges from 0 to 1; closer to 1 means the foreground is more fully filled, and closer to 0 means sparser. This metric reflects the compactness of the object; for example, a circular object has a density of about 0.785, and a triangle about 0.5.

Dia.Circular(handle, maskHandle): Calculates circularity, reflecting how close the object shape is to a circle. Circularity is usually calculated based on the ratio of perimeter to area. An ideal circle has a circularity of 1; the more irregular the shape, the smaller the circularity. This metric is commonly used to distinguish circular targets (such as cells and holes) from elongated or irregular targets.

Dia.Slope(handle, maskHandle): Calculates the slope, representing the tilt angle of the object's principal axis. The slope is usually calculated from the second moment of the object, reflecting the overall orientation of the object. For elongated targets, the slope is the angle between the major axis direction and the horizontal axis; for approximately symmetric targets, the slope may have no clear physical meaning.

Dia.Centroid(handle, maskHandle): Calculates the centroid coordinates, i.e., the geometric center of all foreground pixels of the object. The return value is usually of type RvPoint or similar, representing the position of the centroid in the image coordinate system. The centroid is commonly used for target localization, tracking, and registration.

Dia.GetBoundBox(handle, maskHandle): Calculates the minimum bounding rectangle (also called rotated rectangle or minimum-area rectangle) and returns an RvBox2D structure containing the center coordinates cx, cy, rotation angle angle, width width, and height height. This rectangle rotates with the object direction and can enclose the object with the minimum area, suitable for describing the size and orientation of elongated or tilted targets.

Dia.GetBoundRect(handle, maskHandle): Calculates the axis-aligned bounding rectangle (i.e., bounding rectangle) and returns the position and size of the rectangle. The sides of this rectangle are parallel to the image coordinate axes. It is simple to compute and suitable for quickly locating the object range, but it will contain more blank area for tilted targets.

**About the Mask Parameter**

In the example, all methods except Density pass IntPtr.Zero as the second parameter, indicating that no mask is used and calculations are performed directly on the foreground region of the entire image. In this case, all pixels with value 255 in the image are treated as a single object participating in the statistics. If there are multiple independent foreground objects in the image, not using a mask will treat them as a whole, and the obtained circularity, centroid, bounding box, and other attributes reflect the combined result of all objects rather than the attributes of a single object. Therefore, in single-object scenarios the mask can be omitted; in multi-object scenarios, individual objects should first be separated through BLOB analysis, and then their corresponding masks should be passed in to obtain the independent attributes of each object.

**Key Points for Use**

All methods of the Dia class take handles as parameters, do not modify the input image, and only return calculation results, so no prior cloning is needed. The input image should be a binary image with foreground pixel values of 255 and background 0; if the image is grayscale or multi-channel, the calculation results may not meet expectations. The BlobPart enumeration can also take values such as East, South, West, and North, used to calculate the density distribution of the object in a specific direction. In the RvBox2D returned by GetBoundBox, angle is the rotation angle, and width and height are the long and short sides of the rectangle. The rectangle returned by GetBoundRect has sides parallel to the coordinate axes, and its width and height are usually greater than or equal to the results of GetBoundBox. In practical applications, circularity and density are commonly used for shape classification, centroid and bounding box for localization and tracking, and slope and minimum bounding rectangle for orientation analysis. This group of functions is commonly used in cell analysis, part inspection, target counting, and shape recognition.

## Image Sharpness

Sharpness is a metric that measures the richness of image details and the sharpness of edges. The higher the sharpness, the more drastic the grayscale changes in the image and the more distinct the edges; the lower the sharpness, the blurrier the image and the more details are lost. Sharpness is commonly used for focus evaluation, image quality assessment, denoising effect comparison, and comparison before and after algorithm processing. Unlike simply observing an image, sharpness quantifies image details in numerical form, facilitating objective comparison and automatic decision-making.

The example first loads the colorwave.jpg image and converts it to grayscale, then measures the sharpness of the original image via Dia.GetClearness, then performs one and two blurring operations respectively, re-measuring the sharpness after each operation, and finally outputs the three results to a text box for comparison.

string strFile = "..\\samples\\colorwave.jpg";

KImage img = new KImage();

bool b = img.Load(strFile);

Pool.Assert(b);

img.Cast(PixelFormat.Gray);

KMask mask = CreateMask();

double n = Dia.GetClearness(img.Handle, mask.Handle);

string s = "";

s += \$"Original sharpness is {n} \n";

Dip.Blur(img.Handle, 5, 5);

n = Dia.GetClearness(img.Handle, mask.Handle);

s += \$"The sharpness after blurring is {n} \n";

Dip.Blur(img.Handle, 3, 3);

n = Dia.GetClearness(img.Handle, mask.Handle);

s += \$"The sharpness after 2 times of blurring is {n} \n";

tbxOutput.Text = s;

**Function Description**

Dia.GetClearness(handle, maskHandle): Calculates the sharpness of the image within the specified region.

- handle: Handle of the image to be measured, usually a grayscale image.

- maskHandle: Mask handle, used to specify the region participating in the calculation. In the example, a mask is created via CreateMask() to calculate sharpness only within the valid region of the mask, avoiding interference from background or irrelevant regions. If IntPtr.Zero is passed, the calculation is performed on the entire image.

The return type is double; a larger value indicates a sharper image. The specific algorithm is usually based on image gradient, Laplacian response, or high-frequency energy statistics. Different implementations may differ slightly, but the trend is consistent: more details and sharper edges yield a higher return value.

**Interpreting the Results**

The three measurement results usually show a decreasing trend: the original image has the highest sharpness, which decreases noticeably after one blur, and further decreases after two blurs. The magnitude of the decrease is related to the blur kernel size, the number of blurs, and the richness of details in the original image content. If the image itself has few details (such as large flat areas), the sharpness difference before and after blurring may be small; if the image is rich in details (such as dense textures and edges), the decrease is obvious.

**Key Points for Use**

No cloning is needed. In the example, two consecutive Dip.Blur calls act directly on the same image; the second blur is based on the result of the first, so the total blur effect is the superposition of both, rather than being determined only by the second 3\times3 kernel. If you want to observe the effects of different blur kernels separately, clone the original image separately before processing, rather than performing consecutive operations on the same image. The choice of mask directly affects the measurement result: the mask should cover the region to be evaluated and avoid including large flat background, otherwise the sharpness value will be pulled down and changes will not be obvious. The input image should be a grayscale image; color images must be converted first, otherwise the sharpness calculation may be processed as single-channel or multi-channel, and the result will not meet expectations. This function is commonly used in autofocus, image quality assessment, and comparison of denoising and sharpening algorithm effects.

## Histogram

A histogram is the statistical distribution of the frequency of occurrence of each grayscale level in an image. The horizontal axis is the grayscale value (0–255), and the vertical axis is the number of pixels corresponding to that grayscale value. The histogram reflects the brightness distribution, contrast level, and dynamic range of the image: a concentrated distribution indicates low contrast, a uniform distribution indicates high contrast, and a bias to one side indicates the image is overall dark or bright. For color images, each channel has its own histogram, describing the grayscale distribution of that channel. The histogram is an important basis for image enhancement, threshold selection, exposure evaluation, and color analysis.

The example first loads the lenna.png image and converts it to BGR format, then calls Dia.HistogramEx to obtain the histogram data of the image, then traverses the statistical values channel by channel and grayscale level by grayscale level, and finally concatenates the data and outputs it to a text box, and calls DrawCurve to draw the histogram curve.

string strFile = "..\\samples\\lenna.png";

KImage img = new KImage();

bool b = img.Load(strFile);

Pool.Assert(b);

img.Cast(PixelFormat.BGR);

KMask mask = CreateMask();

string s = "";

KFdox dox = Dia.HistogramEx(img);

if (dox != null)

{

int chns = img.GetChannels();

Int64\[\] rarr = new Int64\[256\];

Int64\[\] garr = new Int64\[256\];

Int64\[\] barr = new Int64\[256\];

for (int c = 0; c \< chns; c++)

{

KFdox sub = dox.GetChild(c);

s += \$"Channel {c + 1}:\r\n";

for (int i = 0; i \< sub.GetCount(); i++)

{

Pool.Assert(i \< 256);

KFdox child = sub.GetChild(i);

Int64 n;

if (child.GetInt64(out n))

{

s += \$"{n} ";

if (c == 0) barr\[i\] = n;

if (c == 1) garr\[i\] = n;

if (c == 2) rarr\[i\] = n;

}

else

{

s += "-1 ";

}

}

DrawCurve(rarr, garr, barr);

}

tbxOutput.Text = s;

}

**Data Structure Description**

Dia.HistogramEx(img) returns a KFdox object, used to organize histogram data in a tree structure. KFdox is a general-purpose data container in the library that supports multi-level child nodes; each node can store integers, floating-point numbers, or other types of data. The hierarchy in the example is as follows:

- Root node dox: represents the histogram of the entire image; its child nodes correspond to each channel.

- Channel node sub = dox.GetChild(c): the histogram of the c-th channel; its child nodes correspond to each grayscale level.

- Grayscale level node child = sub.GetChild(i): the statistical node of the i-th grayscale level, storing the number of pixels at that grayscale level.

**Key Points for Use**

Dia.HistogramEx does not modify the input image and only returns statistical data, so no cloning is needed. Before traversing, check whether dox is null to prevent null reference exceptions. The return value of GetInt64 should be checked to avoid reading invalid values when a node has no data. In the example, mask is created but not used. If you need to limit the statistical region, use a histogram function with a mask parameter instead. DrawCurve is a custom function used to visualize array data; its internal implementation is outside the scope of this tutorial. After processing, the text box outputs 256 statistical values for each channel, and the curve chart intuitively shows the distribution differences among the three channels. This function is commonly used in analysis before image enhancement, threshold selection, exposure evaluation, and color correction.

## Counting Non-zero Pixels

In a binary image, foreground pixels are usually assigned the value 255 and background 0. The number of non-zero pixels is the total number of foreground pixels, which can directly reflect the area, size, or count of the target region. Counting non-zero pixels is one of the most basic measurements in image analysis, commonly used for area measurement, target counting, coverage calculation, and segmentation effect evaluation. Compared with traversing the image point by point, Dia.CountPixels is implemented internally by the library, is more efficient, and supports mask-limited statistical ranges.

The example first loads the triangle.png image and converts it to grayscale, then calls Dip.MaxEntropy for automatic binarization using the maximum entropy method, then counts the number of non-zero pixels in the binary image via Dia.CountPixels, and finally outputs the result to a text box.

string strFile = "..\\samples\\triangle.png";

KImage img = new KImage();

bool b = img.Load(strFile);

Pool.Assert(b);

img.Cast(PixelFormat.Gray);

Dip.MaxEntropy(img.Handle);

long n = Dia.CountPixels(img.Handle, IntPtr.Zero);

string s = "";

s += \$"The count of non-zero pixels is {n} \n";

tbxOutput.Text = s;

**Function Description**

Dia.CountPixels(handle, maskHandle) parameters are as follows:

- handle: Handle of the image to be counted, usually a binary image. If it is a grayscale image, all pixels with a non-zero grayscale value are counted.

- maskHandle: Mask handle, used to limit the statistical region. In the example, IntPtr.Zero is passed, indicating no mask is used and the entire image is counted. If a valid mask is passed, only non-zero pixels within the valid region of the mask (value 1) are counted.

- Return value: long type, representing the total number of non-zero pixels meeting the condition.

**Key Points for Use**

Dia.CountPixels does not modify the input image and only returns the statistical result, so it can be called multiple times on the same image without cloning. The input image should be a binary or grayscale image; if it is a color image, it must first be converted to grayscale and binarized, otherwise the statistical result may include non-zero values from multiple channels and lose the meaning of area. The mask parameter can precisely limit the statistical region, for example, counting only foreground pixels within a certain BLOB or ROI; in this case, a mask should first be created via KMask and its handle passed in. The return type is long, which can represent a large total number of pixels and avoid int overflow. This function is commonly used in target area measurement, coverage analysis, segmentation quality evaluation, and counting statistics.

## Projection Image

Image projection is the process of accumulating pixel values along a certain direction to obtain a one-dimensional array. Horizontal projection accumulates along the horizontal direction, i.e., summing pixels in each row; the length of the resulting array equals the image height, reflecting the energy distribution of the image in the vertical direction. Vertical projection accumulates along the vertical direction, i.e., summing pixels in each column; the length of the resulting array equals the image width, reflecting the energy distribution of the image in the horizontal direction. Projection operations reduce a two-dimensional image to a one-dimensional signal, facilitating analysis of the overall distribution pattern of the image. They are commonly used in character segmentation, target localization, texture analysis, edge detection, and document layout analysis. For example, in a text image, valleys in the horizontal projection usually correspond to blank spaces between lines, and valleys in the vertical projection correspond to gaps between characters, allowing rapid division of text lines and characters.

The example first loads the lenna.png image and converts it to BGR format, then calls Dia.ProjectEx to obtain the projection data of the image in the horizontal and vertical directions, then extracts the projection values channel by channel and stores them in arrays, and finally draws the projection curves of the three channels via DrawCurve, while outputting the raw data to a text box.

string strFile = "..\\samples\\lenna.png";

KImage img = new KImage();

bool b = img.Load(strFile);

Pool.Assert(b);

img.Cast(PixelFormat.BGR);

string s = "";

KFdox dox = Dia.ProjectEx(img, RvDirection.Both, null);

if (dox != null)

{

int chns = img.GetChannels();

Int64\[\] rarr = null;

Int64\[\] garr = null;

Int64\[\] barr = null;

for (int c = 0; c \< chns; c++)

{

KFdox sub = dox.GetChild(c);

int subcnt = sub.GetCount();

if (c == 0)

barr = new Int64\[subcnt\];

else if (c == 1)

garr = new Int64\[subcnt\];

else if (c == 2)

rarr = new Int64\[subcnt\];

s += \$"Channel {c + 1}:\r\n";

for (int i = 0; i \< sub.GetCount(); i++)

{

KFdox child = sub.GetChild(i);

Int64 n;

if (child.GetInt64(out n))

{

s += \$"{n} ";

if (c == 0) barr\[i\] = n;

if (c == 1) garr\[i\] = n;

if (c == 2) rarr\[i\] = n;

}

else

{

s += "-1 ";

}

}

s += "\r\n";

}

DrawCurve(rarr, garr, barr);

}

tbxOutput.Text = s;

**Function Description**

Dia.ProjectEx(img, direction, resultHandle) parameters are as follows:

- img: The image object to be processed, supporting grayscale and color images. For color images, projections are calculated separately by channel.

- direction: Projection direction, specified by the RvDirection enumeration. Horizontal means horizontal projection, Vertical means vertical projection, and Both means calculating both directions simultaneously. In the example, Both is passed, so each channel obtains projection data in two directions.

- resultHandle: KFdox handle, used to store the result data. When null is passed, the function internally creates a new KFdox object and returns it via the return value; if an existing KFdox handle is passed, the result is written into that object and the return value is that handle. In the example, null is passed, so a newly created root node is returned.

The function returns a KFdox object that organizes the projection data in a tree structure. Each child node under the root node corresponds to a color channel; under each channel node are the projection data points of that channel, each storing an integer value representing the cumulative sum of that row or column. For the Both direction, the data points of each channel contain projection results in both horizontal and vertical directions; the total number of data points is usually the sum of the image height and width. KFdox is a general-purpose data container in the library that supports multi-level child nodes, accessed layer by layer via GetChild.

**Key Points for Use**

Dia.ProjectEx does not modify the input image and only returns statistical data, so no cloning is needed. The returned KFdox object should be released in a timely manner after use to avoid memory accumulation. Before traversing, check whether the return value is null to prevent null reference exceptions. The channel order of a color image depends on the pixel format: in the example, it has been converted to BGR, so channel indices 0, 1, and 2 correspond to blue, green, and red respectively, and the arrays barr, garr, and rarr are filled in that order. If the image format is RGB, the correspondence between indices and components is reversed. Projection data can be used to analyze image content distribution: for example, peaks in the horizontal projection correspond to rows with dense targets, and valleys correspond to blank rows; peaks in the vertical projection correspond to columns with dense targets, and valleys correspond to blank columns. Combined with threshold judgment, automatic segmentation of target regions can be achieved. DrawCurve is a custom function used to draw the projection arrays of the three channels as curves, usually with the index as the horizontal axis and the cumulative value as the vertical axis, with the three channels represented in red, green, and blue for intuitive comparison of distribution differences among channels. This function is commonly used in image analysis, character segmentation, target localization, and layout understanding.

## Pixel Intensity Statistics

Grayscale statistics are the most basic quantitative method in image analysis. By calculating indicators such as grayscale sum, average, and variance, the overall brightness level and contrast characteristics of an image can be quickly understood. The grayscale sum reflects the overall energy of the image; the grayscale average reflects overall brightness, with larger values indicating brighter images; the grayscale variance reflects the degree of dispersion of the grayscale distribution, with larger variance indicating more obvious light-dark differences and higher contrast. These statistics are commonly used for exposure evaluation, image quality comparison, threshold reference, and verification of effects before and after algorithm processing.

The example first loads the lenna.png image and converts it to grayscale, then calls Dia.CalcGrayStatsEx to calculate grayscale statistics, then reads the grayscale sum, average, and variance by index via KFdox.GetDoubleAt, and finally outputs the results to a text box.

string strFile = "..\\samples\\lenna.png";

KImage img = new KImage();

bool b = img.Load(strFile);

Pool.Assert(b);

img.Cast(PixelFormat.Gray);

KFdox dox = Dia.CalcGrayStatsEx(img);

string s = "";

double n = 0;

dox.GetDoubleAt(0, out n);

s += \$"Summary of gray: {n}\r\n";

dox.GetDoubleAt(1, out n);

s += \$"Average of gray: {n}\r\n";

dox.GetDoubleAt(2, out n);

s += \$"Variance of gray: {n}\r\n";

tbxOutput.Text = s;

**Function Description**

Dia.CalcGrayStatsEx(img) is used to calculate statistics of a grayscale image.

- img: The image object to be processed; it should be a grayscale image. If it is a color image, it must first be converted to grayscale format.

- Return value: A KFdox object, internally storing multiple statistical results in a fixed order, accessed by index.

KFdox.GetDoubleAt(index, out value) is used to read a floating-point value from a KFdox node by index.

- index: Data index. In the example, 0, 1, and 2 correspond to grayscale sum, average, and variance respectively.

- value: Output parameter; on successful read, stores the result, of type double.

- Return value: Boolean, indicating whether the read was successful (not used in the example).

**Output Description**

- Summary of gray: Grayscale sum, i.e., the accumulation of all pixel grayscale values in the image, reflecting overall energy.

- Average of gray: Grayscale average, equal to the sum divided by the total number of pixels, reflecting overall brightness.

- Variance of gray: Grayscale variance, reflecting the degree to which each pixel's grayscale value deviates from the average, measuring contrast.

**Key Points for Use**

Dia.CalcGrayStatsEx does not modify the input image and only returns statistical data, so no cloning is needed. The returned KFdox object should be released in a timely manner after use to avoid memory accumulation. Before calling GetDoubleAt, ensure the index is valid. In the example, it is assumed that the returned object contains at least 3 data items. In actual use, confirm the meaning of the indices in the documentation, or first check the number of data items via GetCount. Grayscale statistics require the input to be a single-channel image. If it is a color image, it must first be converted via Cast(PixelFormat.Gray); otherwise the statistical result may accumulate across multiple channels and lose physical meaning. A large variance indicates strong light-dark contrast in the image; a small variance indicates the image is overall gray and lacks contrast, which can be used to decide whether equalization or contrast stretching is needed. This function is commonly used in image quality assessment, exposure analysis, pre-processing effect verification, and feature extraction.

## Estimating Canny Operator Contrast

The edge detection effect of the Canny operator depends heavily on two threshold parameters: the low threshold and the high threshold. Too low a threshold introduces noise edges, and too high a threshold loses weak edges. Manually setting thresholds requires repeated experimentation and lacks adaptability to different images. Dia.EstimateCannyContrast is used to automatically estimate a suitable set of low and high contrast thresholds based on the image's own grayscale distribution, which can be directly used as input to the Canny operator, thereby eliminating manual parameter tuning. Different estimation methods yield different thresholds and are suitable for different types of images.

The example first loads the cell.jpg image and converts it to grayscale, then uses three methods to call Dia.EstimateCannyContrast to estimate the minimum and maximum contrast thresholds required by the Canny operator, and finally outputs the results of the three methods to a text box.

string strFile = "..\\samples\\cell.jpg";

KImage img = new KImage();

bool b = img.Load(strFile);

Pool.Assert(b);

img.Cast(PixelFormat.Gray);

string s = "";

double n = 0;

float minContrast = 0;

float maxContrast = 0;

if (Dia.EstimateCannyContrast(img.Handle, IntPtr.Zero, Dia.ECC_MEDINA, 0, ref minContrast, ref maxContrast))

{

s += \$"Median Contrast: MinContrast = {minContrast}, MaxContrast = {maxContrast}\n";

}

else

{

s += "Median Contrast failed\n";

}

if (Dia.EstimateCannyContrast(img.Handle, IntPtr.Zero, Dia.ECC_ADPATIVE, 0.15f, ref minContrast, ref maxContrast))

{

s += \$"Adaptive Contrast: MinContrast = {minContrast}, MaxContrast = {maxContrast}\n";

}

else

{

s += "Adaptive Contrast failed\n";

}

if (Dia.EstimateCannyContrast(img.Handle, IntPtr.Zero, Dia.ECC_HALIKE, 3, ref minContrast, ref maxContrast))

{

s += \$"Halike Contrast: MinContrast = {minContrast}, MaxContrast = {maxContrast}\n";

}

else

{

s += "Halike Contrast failed\n";

}

tbxOutput.Text = s;

**Function Description**

Dia.EstimateCannyContrast(handle, optionHandle, method, param, ref minContrast, ref maxContrast) parameters are as follows:

- handle: Handle of the image to be analyzed, usually a grayscale image.

- optionHandle: Optional handle parameter. In the example, IntPtr.Zero is passed, indicating that this optional parameter is not used and processing is performed by default.

- method: Estimation method, specified by predefined constants in the library:

  - Dia.ECC_MEDINA: Median method. Estimates the contrast range based on the median grayscale value of the image. In the example, param is 0, indicating default parameters are used. This method is simple to compute and suitable for images with relatively uniform grayscale distribution.

  - Dia.ECC_ADPATIVE: Adaptive method. Dynamically adjusts the estimation strategy based on local or global statistical characteristics of the image. In the example, param is 0.15f, a sensitivity coefficient; larger values are more sensitive to contrast changes. Suitable for images with uneven illumination or large contrast differences.

  - Dia.ECC_HALIKE: Histogram similarity method. Estimates the contrast range based on the shape characteristics of the grayscale histogram. In the example, param is 3, a histogram binning or smoothing parameter. Suitable for images with obvious peak-valley characteristics in the histogram.

- param: Method parameter; its meaning varies with method. Specific values should be found in the library documentation.

- minContrast, maxContrast: Output parameters, passed by ref; after successful execution, store the estimated minimum and maximum contrast respectively. These two values can be used as the low and high thresholds of the Canny operator.

- Return value: Boolean; true indicates successful estimation, false indicates failure. On failure, a prompt message should be output to avoid using uninitialized thresholds.

**Comparison of the Three Methods**

- Median method (ECC_MEDINA): Determines the contrast range centered on the grayscale median. It is fast and stable, suitable for most conventional images.

- Adaptive method (ECC_ADPATIVE): Dynamically adjusts according to image content, with better adaptability to illumination changes and local contrast differences. Suitable for complex scenes, but parameters need to be fine-tuned according to image characteristics.

- Histogram similarity method (ECC_HALIKE): Depends on the histogram shape, suitable for images with clear bimodal peaks or obvious valleys in the histogram, such as cell or part images with good foreground-background separation.

**Key Points for Use**

Dia.EstimateCannyContrast does not modify the input image and only returns estimation results, so no cloning is needed. The input image should be a grayscale image; color images must first be converted via Cast(PixelFormat.Gray). minContrast and maxContrast do not need to be reset before each call; the function overwrites their values on success. However, if a call returns false, these two variables may retain the previous result or initial value, so they must only be used after the if condition is true. The three methods can be called in sequence to compare their estimated threshold ranges, or one can be selected according to image characteristics. Usually the low threshold is used for weak edge determination and the high threshold for strong edge determination; the high threshold is generally 2–3 times the low threshold. If the estimation result does not match this ratio, it can be manually adjusted. This function is commonly used in automatic parameter tuning for Canny edge detection, image pre-processing analysis, and batch image processing pipelines.
