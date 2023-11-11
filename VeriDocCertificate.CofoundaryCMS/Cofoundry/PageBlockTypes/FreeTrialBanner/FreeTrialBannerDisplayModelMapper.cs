 
using Cofoundry.Core;
using Cofoundry.Domain;
using Cofoundry.Domain.Internal;
using Microsoft.AspNetCore.Html;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using static System.Net.Mime.MediaTypeNames;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using DeviceDetectorNET;
using System.Net;
using AngleSharp.Io;

namespace Cofoundry.Web;

public class FreeTrialBannerDisplayModelMapper : IPageBlockTypeDisplayModelMapper<FreeTrialBannerDataModel>
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IContentRepository _contentRepository;
    private IDocumentAssetRouteLibrary _documentAssetRouteLibrary;
    IHttpContextAccessor _httpContextAccessor;
    public FreeTrialBannerDisplayModelMapper(IDocumentAssetRouteLibrary documentAssetRouteLibrary, 
        IContentRepository contentRepository, IWebHostEnvironment webHostEnvironment,
          IHttpContextAccessor httpContextAccessor)
    {
        _documentAssetRouteLibrary = documentAssetRouteLibrary;
        _contentRepository = contentRepository;
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task MapAsync(PageBlockTypeDisplayModelMapperContext<FreeTrialBannerDataModel> context, PageBlockTypeDisplayModelMapperResult<FreeTrialBannerDataModel> result)
    {


        foreach (var item in context.Items)
        {
            var displayModel = new FreeTrialBannerDisplayModel();

            var image = await _contentRepository
            .DocumentAssets()
            .GetById(item.DataModel.BannerImageID)
            .AsRenderDetails()
            .ExecuteAsync();

            var BannerImageurl = _documentAssetRouteLibrary.DocumentAsset(image);

           // displayModel.BannerImageID = BannerImageurl;

            displayModel.BannerImageID= ResizeImage(BannerImageurl);//according to device
            displayModel.BannerDescription = new HtmlString(HtmlFormatter.ConvertLineBreaksToBrTags(item.DataModel.BannerDescription));
          

            result.Add(item, displayModel);
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
        var url =  $"{request.Scheme}://{request.Host}" +  orgImagePath.Substring(0,orgImagePath.Length-1) ;
        
        string newFileName =  orgImagePath ;
      
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
            string folderName = rootPath +  Path.GetDirectoryName(orgImagePath);
            string inputFilePath = rootPath + orgImagePath;
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
               

            }
           if(!File.Exists(inputFilePath))
            {
                //download original file
                WebClient client = new WebClient();
              
                byte[] imageData = client.DownloadData(url);
                File.WriteAllBytes(inputFilePath, imageData);
            }
            string extn = Path.GetExtension(inputFilePath);  
            
            string fileNameWithoutExtn = Path.GetFileNameWithoutExtension(inputFilePath);          
            string outputFilePath = inputFilePath.Replace(fileNameWithoutExtn, fileNameWithoutExtn + "_"+ device);
            if(File.Exists(outputFilePath))
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