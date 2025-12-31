
namespace ShiemiApi.Utility;

public class ImageUtility
{
    private readonly Cloudinary? _cloudinary;
    public ImageUtility()
    {
        Account account = new(
            CloudinaryKeyStore.CloudName,
            CloudinaryKeyStore.ApiKey,
            CloudinaryKeyStore.ApiSecret
        );
        _cloudinary = new Cloudinary(account);
        _cloudinary.Api.Secure = true;
    }

    public UploadResult UploadImage(IFormFile file)
    {
        if (file.Length <= 0)
            return null!;

        using Stream stream = file.OpenReadStream();
        ImageUploadParams uploadParams = new()
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Height(500)
            .Width(500)
            .Crop("fill")
            .Gravity("face")
        };
        UploadResult result = _cloudinary!.Upload(uploadParams);

        return result;
    }
    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        DeletionParams deleteParams = new(publicId);
        return await _cloudinary!.DestroyAsync(deleteParams);
    }
}
