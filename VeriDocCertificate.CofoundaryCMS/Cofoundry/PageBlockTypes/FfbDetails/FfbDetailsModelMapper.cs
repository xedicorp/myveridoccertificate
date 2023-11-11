using Cofoundry.Core;
using Cofoundry.Domain;
using Cofoundry.Domain.Internal;
using DeviceDetectorNET;
using Microsoft.AspNetCore.Html;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace Cofoundry.Web;

public class FfbDetailsDisplayModelMapper : IPageBlockTypeDisplayModelMapper<FfbDetailsDataModel>
{   
    private readonly IContentRepository _contentRepository;
    private IDocumentAssetRouteLibrary _documentAssetRouteLibrary;
    private readonly IWebHostEnvironment _webHostEnvironment;
    IHttpContextAccessor _httpContextAccessor;
    public FfbDetailsDisplayModelMapper(IDocumentAssetRouteLibrary documentAssetRouteLibrary, 
        IContentRepository contentRepository,
         IWebHostEnvironment webHostEnvironment,
          IHttpContextAccessor httpContextAccessor)
    {
        _documentAssetRouteLibrary = documentAssetRouteLibrary;
        _contentRepository = contentRepository;
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task MapAsync(PageBlockTypeDisplayModelMapperContext<FfbDetailsDataModel> context, PageBlockTypeDisplayModelMapperResult<FfbDetailsDataModel> result)
    {


        foreach (var item in context.Items)
        {

            var displayModel = new FfbDetailsDisplayModel();
            //section name
            displayModel.FfbTitle = item.DataModel.FfbTitle;

            Int32 FfbImageId = returnintvaluefromnulable(Convert.ToString(item.DataModel.FfbImageID));

            var image = await _contentRepository
             .DocumentAssets()
             .GetById(FfbImageId).AsRenderDetails().ExecuteAsync();

            // var Imageurl = _documentAssetRouteLibrary.DocumentAsset(image);
            var Imageurl =ResizeImage( _documentAssetRouteLibrary.DocumentAsset(image));
            displayModel.FfbImageID = Imageurl;
            //Get Image
            displayModel.MeidaLookup = item.DataModel.MeidaLookup;
            displayModel.VideoLookup = item.DataModel.VideoLookup;
            //Pick Image End
            displayModel.FfbIframe = item.DataModel.FfbIframe;
            //Pick HTML Editor
            displayModel.FfbDescription = new HtmlString(HtmlFormatter.ConvertLineBreaksToBrTags(item.DataModel.FfbDescription));
            //HTML End

            result.Add(item, displayModel);
        }

        
    }
   

    public Int32 returnintvaluefromnulable(String? integervalue)
    {
        if (integervalue == "")
        {
            return 0;
        }
        else
        {
            return Convert.ToInt32(integervalue);
        }

    }
    private string ResizeImage(string orgImagePath)
    {
        if (!string.IsNullOrEmpty(orgImagePath))
        {
            string ext = Path.GetExtension(orgImagePath);
            if (ext == ".svg")
                return orgImagePath;
        }
        else
            return orgImagePath;
        if (string.IsNullOrEmpty(orgImagePath)) return orgImagePath;
        var request = _httpContextAccessor.HttpContext.Request;
        var url = $"{request.Scheme}://{request.Host}" + orgImagePath.Substring(0, orgImagePath.Length - 1);

        string newFileName = orgImagePath;

        var dd = new DeviceDetector(_httpContextAccessor.HttpContext.Request.Headers["user-agent"]);
        dd.Parse();
        var device = dd.GetDeviceName();
        if (device == "smartphone" || device == "tablet" || device == "tv")
        {
            /**
        Possible returns
        --
        desktop
        smartphone
        tablet
        feature phone
        console
        tv
        car browser
        smart display
        camera
        portable media player
        phablet
        **/
            string newFileNameWithoutExtn = Path.GetFileNameWithoutExtension(orgImagePath);
            newFileName = orgImagePath.Replace(newFileNameWithoutExtn, newFileNameWithoutExtn + "_" + device);
            int aspectRatio = 3;
            if (device == "tablet")
                aspectRatio = 2;
            string rootPath = _webHostEnvironment.WebRootPath;
            string folderName = rootPath + Path.GetDirectoryName(orgImagePath);
            string inputFilePath = rootPath + orgImagePath;
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);


            }
            if (!File.Exists(inputFilePath))
            {
                //download original file
                WebClient client = new WebClient();

                byte[] imageData = client.DownloadData(url);
                File.WriteAllBytes(inputFilePath, imageData);
            }
            string extn = Path.GetExtension(inputFilePath);

            string fileNameWithoutExtn = Path.GetFileNameWithoutExtension(inputFilePath);
            string outputFilePath = inputFilePath.Replace(fileNameWithoutExtn, fileNameWithoutExtn + "_" + device);
            if (File.Exists(outputFilePath))
            {
                return newFileName;
            }
            using (Stream inStream = new MemoryStream(File.ReadAllBytes(inputFilePath)))
            {
                using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(inStream))
                {
                    int width = image.Width / aspectRatio;
                    int height = image.Height / aspectRatio;
                    image.Mutate(x => x.Resize(width, height));

                    image.Save(outputFilePath);
                }
            }
            System.GC.Collect();
        }
        return newFileName;
    }
}