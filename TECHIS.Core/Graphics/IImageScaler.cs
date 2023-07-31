namespace TECHIS.Images
{
    public interface IImageScaler
    {
        //bool SaveImage(byte[]? bytes, string fileName);
        (bool success, byte[] output) ScaleDown(byte[] bytes, string contentType, int width=0, int height=0, int qualityPercentage = 100);
    }
}