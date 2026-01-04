using System;
using SixLabors.ImageSharp.Processing;

namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="CanvasImage"/>.
/// </summary>
public static class CanvasImageExtensions
{
    /// <param name="image">The canvas image.</param>
    extension(CanvasImage image)
    {
        /// <summary>
        /// Sets the maximum width of the rendered image.
        /// </summary>
        /// <param name="maxWidth">The maximum width.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public CanvasImage MaxWidth(int? maxWidth)
        {
            ArgumentNullException.ThrowIfNull(image);

            image.MaxWidth = maxWidth;
            return image;
        }

        /// <summary>
        /// Disables the maximum width of the rendered image.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public CanvasImage NoMaxWidth()
        {
            ArgumentNullException.ThrowIfNull(image);

            image.MaxWidth = null;
            return image;
        }

        /// <summary>
        /// Sets the pixel width.
        /// </summary>
        /// <param name="width">The pixel width.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public CanvasImage PixelWidth(int width)
        {
            ArgumentNullException.ThrowIfNull(image);

            image.PixelWidth = width;
            return image;
        }

        /// <summary>
        /// Mutates the underlying image.
        /// </summary>
        /// <param name="action">The action that mutates the underlying image.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public CanvasImage Mutate(Action<IImageProcessingContext> action)
        {
            ArgumentNullException.ThrowIfNull(image);

            ArgumentNullException.ThrowIfNull(action);

            image.Image.Mutate(action);
            return image;
        }

        /// <summary>
        /// Uses a bicubic sampler that implements the bicubic kernel algorithm W(x).
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public CanvasImage BicubicResampler()
        {
            ArgumentNullException.ThrowIfNull(image);

            image.Resampler = KnownResamplers.Bicubic;
            return image;
        }

        /// <summary>
        /// Uses a bilinear sampler. This interpolation algorithm
        /// can be used where perfect image transformation with pixel matching is impossible,
        /// so that one can calculate and assign appropriate intensity values to pixels.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public CanvasImage BilinearResampler()
        {
            ArgumentNullException.ThrowIfNull(image);

            image.Resampler = KnownResamplers.Triangle;
            return image;
        }

        /// <summary>
        /// Uses a Nearest-Neighbour sampler that implements the nearest neighbor algorithm.
        /// This uses a very fast, unscaled filter which will select the closest pixel to
        /// the new pixels position.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public CanvasImage NearestNeighborResampler()
        {
            ArgumentNullException.ThrowIfNull(image);

            image.Resampler = KnownResamplers.NearestNeighbor;
            return image;
        }
    }
}