using Cofoundry.Core;
using DeviceDetectorNET;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace VeriDocCertificate.CofoundaryCMS;

public class FeatureContentDisplayModelMapper : IPageBlockTypeDisplayModelMapper<FeatureContentDataModel>
{
    private readonly IContentRepository _contentRepository;
    private IDocumentAssetRouteLibrary _documentAssetRouteLibrary;
    private readonly IWebHostEnvironment _webHostEnvironment;
    IHttpContextAccessor _httpContextAccessor;
    public FeatureContentDisplayModelMapper(IDocumentAssetRouteLibrary documentAssetRouteLibrary,
        IContentRepository contentRepository,
         IWebHostEnvironment webHostEnvironment,
          IHttpContextAccessor httpContextAccessor)
    {
        _documentAssetRouteLibrary = documentAssetRouteLibrary;
        _contentRepository = contentRepository;
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;

    }

    public async Task MapAsync(
        PageBlockTypeDisplayModelMapperContext<FeatureContentDataModel> context,
        PageBlockTypeDisplayModelMapperResult<FeatureContentDataModel> result
        )
    {
          // Find all the image ids to load
        var allImageAssetIds = context
            .Items
            .SelectMany(m => m.DataModel.FeatureLists)
            .Select(m => m.FeatureImageID)
            .Where(i => i > 0)
            .Distinct();

        // Load image data
        var allImages = await _contentRepository
            .WithContext(context.ExecutionContext)
            .DocumentAssets()
            .GetByIdRange(allImageAssetIds)
            .AsRenderDetails()
            .ExecuteAsync();
        // Map display model
        foreach (var items in context.Items)
        {
            var output = new FeatureContentDisplayModel();
            
            

            output.FeatureLists = EnumerableHelper
                .Enumerate(items.DataModel.FeatureLists)
                .Select(m => new FeatureContentListDisplayModel()
                {
                    FeatureImageID = allImages.GetOrDefault(m.FeatureImageID),
                    FeatureTitle = m.FeatureTitle,
                    FeatureDescription = new HtmlString(HtmlFormatter.ConvertLineBreaksToBrTags(m.FeatureDescription))
                })
                .ToList();


            foreach(var item in output.FeatureLists)
            {
                item.FeatureImageUrl = ResizeImage(item.FeatureImageID.Url);
            }

            result.Add(items, output);
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
                inStream.Position = 0;
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
