using System;

namespace TECHIS.Core
{
    public static class MimeTypeUtil
    {
        /// <summary>
        /// Returns the file extension that corresponds to the specified content type
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string GetExtensionFromContentType(string contentType)
        {
            string val;
            switch (contentType)
            {
                case "image/gif":
                    val = "gif";
                    break;
                case "image/jpeg":
                    val = "jpg";
                    break;
                case "image/pjpeg":
                    val = "pjpeg";
                    break;
                case "image/png":
                    val = "png";
                    break;
                case "image/svg+xml":
                    val = "svg";
                    break;
                case "image/tiff":
                    val = "tiff";
                    break;
                case "video/webm":
                    val = "webm";
                    break;
                case "video/mpeg":
                    val = "mpeg";
                    break;
                case "audio/mpeg":
                    val = "mpga";
                    break;
                case "video/mp4":
                    val = "mp4";
                    break;
                case "audio/mp4":
                    val = "mp4a";
                    break;
                case "video/ogg":
                    val = "ogv";
                    break;
                case "video/3gpp":
                    val = "3gp";
                    break;
                case "video/3gpp2":
                    val = "3gpp2";
                    break;
                case "video/x-msvideo":
                    val = "avi";
                    break;
                case "video/quicktime":
                    val = "mov";
                    break;
                case "audio/ogg":
                    val = "oga";
                    break;
                default:
                    return string.Empty;
                    
            }

            return val;
        }

        // Create a new method similar to GetExtensionFromContentType that returns the content type that corresponds to the specified file extension
        public static string GetContentTypeFromExtension(string extension)
        {
            string val;
            switch (extension)
            {
                case "gif":
                    val = "image/gif";
                    break;
                case "jpg":
                    val = "image/jpeg";
                    break;
                case "pjpeg":
                    val = "image/pjpeg";
                    break;
                case "png":
                    val = "image/png";
                    break;
                case "svg":
                    val = "image/svg+xml";
                    break;
                case "tiff":
                    val = "image/tiff";
                    break;
                case "webm":
                    val = "video/webm";
                    break;
                case "mpeg":
                    val = "video/mpeg";
                    break;
                case "mpga":
                    val = "audio/mpeg";
                    break;
                case "mp4":
                    val = "video/mp4";
                    break;
                case "mp4a":
                    val = "audio/mp4";
                    break;
                case "ogv":
                    val = "video/ogg";
                    break;
                case "3gp":
                    val = "video/3gpp";
                    break;
                case "3gpp2":
                    val = "video/3gpp2";
                    break;
                case "avi":
                    val = "video/x-msvideo";
                    break;
                case "mov":
                    val = "video/quicktime";
                    break;
                case "oga":
                    val = "audio/ogg";
                    break;
                default:
                    return string.Empty;

            }

            return val;
        }
    }
}
