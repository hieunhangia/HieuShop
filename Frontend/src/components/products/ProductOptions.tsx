// import { useMemo } from "react"; // Removed unused
import type {
  ProductOption,
  ProductVariant,
} from "../../types/products/dtos/ProductDetail";
import clsx from "clsx";

interface ProductOptionsProps {
  options: ProductOption[];
  variants: ProductVariant[];
  selectedOptions: Record<string, string>;
  onOptionSelect: (optionId: string, valueId: string) => void;
}

const ProductOptions = ({
  options,
  variants,
  selectedOptions,
  onOptionSelect,
}: ProductOptionsProps) => {
  const isValueAvailable = (optionId: string, valueId: string) => {
    return variants.some((variant) => {
      const hasValue = variant.productOptionValues.some(
        (v) => v.id === valueId,
      );
      if (!hasValue) return false;

      for (const [otherOptId, otherValId] of Object.entries(selectedOptions)) {
        if (otherOptId === optionId) continue;

        const variantValForOtherOpt = variant.productOptionValues.find((v) => {
          return v.id === otherValId;
        });

        if (!variantValForOtherOpt) {
          return false;
        }
      }
      return variant.availableStock > 0;
    });
  };
  return (
    <div className="space-y-6">
      {options.map((option) => (
        <div key={option.id} className="space-y-3">
          <h3 className="text-sm font-medium text-gray-900 dark:text-gray-200">
            {option.name}
          </h3>
          <div className="flex flex-wrap gap-3">
            {option.productOptionValues.map((value) => {
              const isSelected = selectedOptions[option.id] === value.id;
              const available = isValueAvailable(option.id, value.id);
              return (
                <button
                  key={value.id}
                  onClick={() => onOptionSelect(option.id, value.id)}
                  className={clsx(
                    "relative flex items-center justify-center rounded-md border py-2 px-4 text-sm font-medium uppercase focus:outline-none sm:flex-1 sm:py-3 cursor-pointer transition-all",
                    isSelected
                      ? "border-transparent bg-blue-600 text-white hover:bg-blue-500 shadow-sm"
                      : "border-gray-200 text-gray-900 dark:border-gray-700 dark:text-gray-200 dark:hover:bg-gray-700 bg-white hover:bg-gray-200 dark:bg-gray-800",
                    !available &&
                      !isSelected &&
                      "bg-gray-100 text-gray-400 border-dashed border-gray-300 opacity-50 dark:opacity-100 dark:bg-gray-800/50 dark:text-gray-500 dark:border-gray-600",
                    isSelected && !available && "opacity-50 cursor-not-allowed",
                  )}
                >
                  {value.value}
                </button>
              );
            })}
          </div>
        </div>
      ))}
    </div>
  );
};

export default ProductOptions;
