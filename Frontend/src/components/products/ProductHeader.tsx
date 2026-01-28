import { useMemo } from "react";
import { Link } from "react-router-dom";
import type { Brand, Category } from "../../types/products/dtos/ProductDetail";

interface ProductHeaderProps {
  brand?: Brand;
  categories: Category[];
}

const ProductHeader = ({ brand, categories }: ProductHeaderProps) => {
  // Drag to scroll refs and state
  const scrollRef = useMemo(
    () => ({ current: null as HTMLDivElement | null }),
    [],
  );
  const isDown = useMemo(() => ({ current: false }), []);
  const startX = useMemo(() => ({ current: 0 }), []);
  const scrollLeft = useMemo(() => ({ current: 0 }), []);
  const isDragging = useMemo(() => ({ current: false }), []);

  const handleMouseDown = (e: React.MouseEvent) => {
    if (!scrollRef.current) return;
    isDown.current = true;
    isDragging.current = false;
    scrollRef.current.classList.add("active");
    startX.current = e.pageX - scrollRef.current.offsetLeft;
    scrollLeft.current = scrollRef.current.scrollLeft;
    scrollRef.current.style.cursor = "grabbing";
  };

  const handleMouseLeave = () => {
    isDown.current = false;
    if (scrollRef.current) scrollRef.current.style.cursor = "grab";
  };

  const handleMouseUp = () => {
    isDown.current = false;
    if (scrollRef.current) scrollRef.current.style.cursor = "grab";
    // Reset dragging flag slightly later to allow click event to be blocked if needed
    setTimeout(() => {
      isDragging.current = false;
    }, 0);
  };

  const handleMouseMove = (e: React.MouseEvent) => {
    if (!isDown.current || !scrollRef.current) return;
    e.preventDefault();
    const x = e.pageX - scrollRef.current.offsetLeft;
    const walk = (x - startX.current) * 2; // scroll-fast
    scrollRef.current.scrollLeft = scrollLeft.current - walk;

    if (Math.abs(walk) > 5) {
      isDragging.current = true;
    }
  };

  const handleLinkClick = (e: React.MouseEvent) => {
    if (isDragging.current) {
      e.preventDefault();
      e.stopPropagation();
    }
  };

  return (
    <div className="mb-8 flex flex-col sm:flex-row sm:items-center sm:justify-between gap-6 px-1">
      {brand && (
        <Link
          to={`/brands/${brand.slug}/products`}
          className="group flex items-center gap-4 w-fit p-2 -ml-2 rounded-xl hover:bg-gray-50 dark:hover:bg-gray-800 transition-all duration-300"
        >
          {brand.logoUrl && (
            <img
              src={brand.logoUrl}
              alt={brand.name}
              className="h-16 w-16 object-contain rounded-full border border-gray-200 dark:border-gray-700 bg-white shadow-sm group-hover:shadow-md transition-shadow"
            />
          )}
          <div className="flex flex-col">
            <span className="text-sm text-gray-500 dark:text-gray-400 font-medium tracking-wide uppercase">
              Thương hiệu
            </span>
            <span className="text-xl font-bold text-gray-900 group-hover:text-blue-600 dark:text-white dark:group-hover:text-blue-400 transition-colors whitespace-nowrap">
              {brand.name}
            </span>
          </div>
        </Link>
      )}

      {/* Vertical Divider */}
      <div className="h-8 w-px bg-gray-200 dark:bg-gray-700 hidden sm:block"></div>

      <div className="flex-1 min-w-0 relative group/cat">
        <div className="flex items-center">
          <span className="text-sm text-gray-500 dark:text-gray-400 mr-3 whitespace-nowrap hidden sm:block">
            Danh mục:
          </span>

          <div
            id="category-scroll-container"
            ref={(el) => {
              scrollRef.current = el;
            }}
            onMouseDown={handleMouseDown}
            onMouseLeave={handleMouseLeave}
            onMouseUp={handleMouseUp}
            onMouseMove={handleMouseMove}
            className="flex overflow-x-auto gap-2 scrollbar-hide py-1 pr-2 scroll-smooth [scrollbar-width:none] [-ms-overflow-style:none] [&::-webkit-scrollbar]:hidden cursor-grab active:cursor-grabbing select-none"
          >
            {categories.map((cat) => (
              <Link
                key={cat.id}
                to={`/categories/${cat.slug}/products`}
                onClickCapture={handleLinkClick}
                draggable={false}
                className="flex-shrink-0 inline-flex items-center gap-2 rounded-full bg-white border border-gray-200 pl-1.5 pr-3 py-1 text-sm font-medium text-gray-700 shadow-sm hover:bg-gray-50 hover:border-gray-300 dark:bg-gray-800 dark:border-gray-700 dark:text-gray-200 dark:hover:bg-gray-700 transition-all select-none"
              >
                {cat.imageUrl && (
                  <img
                    src={cat.imageUrl}
                    alt={cat.name}
                    className="h-5 w-5 rounded-full object-cover pointer-events-none"
                  />
                )}
                <span className="whitespace-nowrap pointer-events-none">
                  {cat.name}
                </span>
              </Link>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductHeader;
