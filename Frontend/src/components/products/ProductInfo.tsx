import clsx from "clsx";

interface ProductInfoProps {
  name: string;
  priceRange: string;
  isAvailable: boolean;
}

const ProductInfo = ({ name, priceRange, isAvailable }: ProductInfoProps) => {
  return (
    <div className="flex flex-col space-y-4">
      <div className="space-y-4">
        <h1 className="text-3xl font-bold text-gray-900 leading-tight dark:text-white">
          {name}
        </h1>
      </div>

      <div className="flex items-center justify-between border-b border-gray-100 pb-4 dark:border-gray-700">
        <div className="flex flex-col">
          <span className="text-sm text-gray-500 dark:text-gray-400">Giá</span>
          <span className="text-2xl font-bold text-gray-900 dark:text-white">
            {priceRange}
          </span>
        </div>
        <div
          className={clsx(
            "px-3 py-1 rounded-full text-sm font-medium",
            isAvailable
              ? "bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400"
              : "bg-red-100 text-red-700 dark:bg-red-900/30 dark:text-red-400",
          )}
        >
          {isAvailable ? "Còn hàng" : "Hết hàng"}
        </div>
      </div>
    </div>
  );
};

export default ProductInfo;
