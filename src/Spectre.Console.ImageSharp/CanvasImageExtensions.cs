using System;
using SixLabors.ImageSharp.Processing;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="CanvasImage"/>.
    /// </summary>
    public static class CanvasImageExtensions
    {
        /// <summary>
        /// Sets the maximum width of the rendered image.
        /// </summary>
        /// <param name="image">The canvas image.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static CanvasImage MaxWidth(this CanvasImage image, int? maxWidth)
        {
            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            image.MaxWidth = maxWidth;
            return image;
        }

        /// <summary>
        /// Disables the maximum width of the rendered image.
        /// </summary>
        /// <param name="image">The canvas image.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static CanvasImage NoMaxWidth(this CanvasImage image)
        {
            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            image.MaxWidth = null;
            return image;
        }

        /// <summary>
        /// Sets the pixel width.
        /// </summary>
        /// <param name="image">The canvas image.</param>
        /// <param name="width">The pixel width.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static CanvasImage PixelWidth(this CanvasImage image, int width)
        {
            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            image.PixelWidth = width;
            return image;
        }

        /// <summary>
        /// Mutates the underlying image.
        /// </summary>
        /// <param name="image">The canvas image.</param>
        /// <param name="action">The action that mutates the underlying image.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static CanvasImage Mutate(this CanvasImage image, Action<IImageProcessingContext> action)
        {
            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            image.Image.Mutate(action);
            return image;
        }

        /// <summary>
        /// Uses a bicubic sampler that implements the bicubic kernel algorithm W(x).
        /// </summary>
        /// <param name="image">The canvas image.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static CanvasImage BicubicResampler(this CanvasImage image)
        {
            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            image.Resampler = KnownResamplers.Bicubic;
            return image;
        }

        /// <summary>
        /// Uses a bilinear sampler. This interpolation algorithm
        /// can be used where perfect image transformation with pixel matching is impossible,
        /// so that one can calculate and assign appropriate intensity values to pixels.
        /// </summary>
        /// <param name="image">The canvas image.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static CanvasImage BilinearResampler(this CanvasImage image)
        {
            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            image.Resampler = KnownResamplers.Triangle;
            return image;
        }

        /// <summary>
        /// Uses a Nearest-Neighbour sampler that implements the nearest neighbor algorithm.
        /// This uses a very fast, unscaled filter which will select the closest pixel to
        /// the new pixels position.
        /// </summary>
        /// <param name="image">The canvas image.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static CanvasImage NearestNeighborResampler(this CanvasImage image)
        {
            if (image is null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            image.Resampler = KnownResamplers.NearestNeighbor;
            return image;
        }
    }
}
