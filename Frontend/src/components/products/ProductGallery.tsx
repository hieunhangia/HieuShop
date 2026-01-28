import { useState, useEffect } from "react";
import type { ProductImage } from "../../types/products/dtos/ProductDetail";
import { clsx } from "clsx";
import { ChevronLeft, ChevronRight } from "lucide-react";

// Extended interface to include optional caption/variant info for gallery display
export interface GalleryImage extends ProductImage {
  caption?: string;
  variantId?: string;
}

interface ProductGalleryProps {
  images: GalleryImage[];
  activeImage?: GalleryImage | null;
  onImageSelect?: (image: GalleryImage) => void;
}

const ProductGallery = ({
  images,
  activeImage,
  onImageSelect,
}: ProductGalleryProps) => {
  /* Logic fix: store the entire image object, not just URL, so we can access caption */
  const [selectedImage, setSelectedImage] = useState<GalleryImage | null>(
    images.length > 0 ? images[0] : null,
  );

  useEffect(() => {
    if (activeImage) {
      setSelectedImage(activeImage);
    }
  }, [activeImage]);

  useEffect(() => {
    if (images.length > 0 && !selectedImage) {
      setSelectedImage(images[0]);
    }
  }, [images, selectedImage]);

  const handleImageClick = (img: GalleryImage) => {
    setSelectedImage(img);
    if (onImageSelect) {
      onImageSelect(img);
    }
  };

  const handlePrev = (e: React.MouseEvent) => {
    e.stopPropagation();
    if (!selectedImage) return;
    const currentIndex = images.findIndex((img) => img.id === selectedImage.id);
    const prevIndex = (currentIndex - 1 + images.length) % images.length;
    handleImageClick(images[prevIndex]);
  };

  const handleNext = (e: React.MouseEvent) => {
    e.stopPropagation();
    if (!selectedImage) return;
    const currentIndex = images.findIndex((img) => img.id === selectedImage.id);
    const nextIndex = (currentIndex + 1) % images.length;
    handleImageClick(images[nextIndex]);
  };

  if (!images.length || !selectedImage)
    return (
      <div className="aspect-square bg-gray-200 rounded-lg animate-pulse" />
    );

  return (
    <div className="flex flex-col gap-4">
      {/* Main Image */}
      <div className="aspect-square w-full relative overflow-hidden rounded-xl border border-gray-100 bg-white dark:bg-gray-800 dark:border-gray-700 group">
        <img
          src={selectedImage.imageUrl}
          alt="Product details"
          className="h-full w-full object-cover transition-none"
        />
        {/* Caption Overlay for Variant Details */}
        {selectedImage.caption && (
          <div className="absolute bottom-4 left-4 right-4 bg-black/70 backdrop-blur-sm text-white text-xs font-medium py-2 px-3 rounded-lg text-center shadow-lg transition-all border border-white/10">
            {selectedImage.caption}
          </div>
        )}

        {/* Navigation Arrows */}
        {images.length > 1 && (
          <>
            <button
              onClick={handlePrev}
              className="absolute left-4 top-1/2 -translate-y-1/2 p-2 rounded-full bg-white/80 text-gray-800 hover:bg-white shadow-md backdrop-blur-sm transition-all opacity-0 group-hover:opacity-100 active:scale-95 disabled:opacity-0"
              aria-label="Previous image"
            >
              <ChevronLeft size={24} />
            </button>
            <button
              onClick={handleNext}
              className="absolute right-4 top-1/2 -translate-y-1/2 p-2 rounded-full bg-white/80 text-gray-800 hover:bg-white shadow-md backdrop-blur-sm transition-all opacity-0 group-hover:opacity-100 active:scale-95 disabled:opacity-0"
              aria-label="Next image"
            >
              <ChevronRight size={24} />
            </button>
          </>
        )}
      </div>

      {/* Thumbnails */}
      <div className="flex gap-3 overflow-x-auto pb-2 scrollbar-thin scrollbar-thumb-gray-200 dark:scrollbar-thumb-gray-700 snap-x">
        {images.map((img) => (
          <button
            key={img.id}
            onClick={() => handleImageClick(img)}
            className={clsx(
              "relative aspect-square flex-shrink-0 w-20 sm:w-24 cursor-pointer overflow-hidden rounded-lg border-2 bg-white transition-all dark:bg-gray-800 snap-start",
              selectedImage.id === img.id
                ? "border-blue-600 ring-2 ring-blue-100 dark:ring-blue-900"
                : "border-gray-200 hover:border-gray-300 dark:border-gray-700 dark:hover:border-gray-600",
            )}
          >
            <img
              src={img.imageUrl}
              alt="Thumbnail"
              className="h-full w-full object-contain p-1"
            />
          </button>
        ))}
      </div>
    </div>
  );
};

export default ProductGallery;
