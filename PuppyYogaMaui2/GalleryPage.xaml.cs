namespace PuppyYogaMaui2;

public class ImageItem
{
    public string ImagePath { get; set; }
}

public partial class GalleryPage : ContentPage
{
    public List<ImageItem> ImageList { get; set; }

    public GalleryPage()
    {
        InitializeComponent();

        ImageList = new List<ImageItem>
        {
            new ImageItem { ImagePath = "gallery_image1.jpg" },
            new ImageItem { ImagePath = "gallery_image2.jpg" },
            new ImageItem { ImagePath = "gallery_image3.jpg" },
            new ImageItem { ImagePath = "gallery_image4.jpg" },
            new ImageItem { ImagePath = "gallery_image5.jpg" },
            new ImageItem { ImagePath = "gallery_image6.jpg" },

        };

        BindingContext = this;
    }
}
