﻿namespace Cofoundry.Domain;

/// <summary>
/// Defines if and how the image should be scaled to fit the resized dimensions e.g. is the 
/// image allowed to up-scaled or should padding be added or should the natural image size 
/// be returned. Defaults to DownscaleOnly, which prevents images being upscaled.
/// </summary>
public enum ImageScaleMode
{
    /// <summary>
    /// (Default) Only reduce the size of images - never enlarge. If the original image is smaller
    /// than resize settings, the original image is returned.
    /// </summary>
    DownscaleOnly = 0,

    /// <summary>
    /// Allows both upscaling and downscaling of images.
    /// </summary>
    Both = 2,

    /// <summary>
    /// Downscaling is permitted, but when the requested size is bigger than the original 
    /// image padding is added. 
    /// </summary>
    UpscaleCanvas = 3,
}
