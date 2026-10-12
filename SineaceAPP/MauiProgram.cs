using Microsoft.Extensions.Logging;
using The49.Maui.BottomSheet;
#if IOS
using Foundation;
using SineaceAPP.Platforms.iOS;
#endif
using CommunityToolkit.Maui;
using Mopups.Hosting;
using SineaceAPP.Custom.Mapa;


namespace SineaceAPP;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseBottomSheet()
            .UseMauiCommunityToolkit()
            .ConfigureMopups();

        builder.UseMauiMaps();

        builder.ConfigureMauiHandlers(handlers =>
        {
#if IOS
            handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
#endif
        })
            .ConfigureFonts(fonts =>
            {
                //fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                //fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("poppinsbold.otf", "Poppinbold");
                fonts.AddFont("poppinslight.otf", "Poppinlight");
                fonts.AddFont("poppinsmedium.otf", "Poppinsmedium");
                fonts.AddFont("poppinsregular.otf", "Poppinsregular");
                fonts.AddFont("robotomedium.ttf", "robotmedium");
                fonts.AddFont("robotolight.ttf", "robotolight");
                fonts.AddFont("robotobold.ttf", "robotobold");
                fonts.AddFont("robotoblack.ttf", "robotoblack");
                fonts.AddFont("robotothin.ttf", "robotothin");
            })
            .ConfigureGifAnimation();

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
#if IOS
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
    public static MauiAppBuilder ConfigureGifAnimation(this MauiAppBuilder builder)
    {
        builder.ConfigureMauiHandlers(handlers =>
        {
#if IOS
            Microsoft.Maui.Handlers.ImageHandler.Mapper.AppendToMapping("gifimage", async (handler, view) =>
            {
                if (view is GIFImage)
                {
                    var imageStream = await ConvertImageSourceToStreamAsync(((GIFImage)view).ImageName);
                    var nsdata = await GetDataAsync(imageStream);
                    var image = AnimatedImageView.GetAnimatedImageView(nsdata, handler.PlatformView);
                }
            });
#endif

        });
        return builder;
    }
#if IOS
    async static Task<NSData> GetDataAsync(Stream stream)
    {
        return await Task.Run(() => { return NSData.FromStream(stream); });
    }
#endif

    public static async Task<Stream> ConvertImageSourceToStreamAsync(string imageName)
    {
        return await FileSystem.OpenAppPackageFileAsync(imageName);
    }

}
public class GIFImage : Image
{
    public string ImageName { get; set; }
}
