// import { useMemo } from "react"; // Removed unused
import type {
  ProductOption,
  ProductVariant,
} from "../../types/products/productDetail";
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
  // Check if a specific option value is available in combination with *current* selections of *other* options
  const isValueAvailable = (optionId: string, valueId: string) => {
    // 1. We create a logic that checks if the proposed value + other currently selected values form a valid variant.

    // 2. Check if there is ANY variant that matches this test selection.
    //    Note: We must only check against options that are currently selected (keys in testSelection).
    //    However, usually we want to know if *some* variant exists.
    //    Since product options are dependent, valid combinations are those that actully exist in variants.

    // We want to find if there is >= 1 variant where:
    //   - For every option key in `variants`'s values definition...
    //   - IF that option is selected in `testSelection`, it MUST match.
    //   - IF NOT selected yet (we might not have selected all options), we assume it's "possible" (OR better: we filter strictly).

    // Let's rely on standard practice:
    // A value is "available" if there exists at least one variant that includes this value AND matches the OTHER currently selected options.

    return variants.some((variant) => {
      // Does this variant contain the value we are testing?
      const hasValue = variant.productOptionValues.some(
        (v) => v.id === valueId, // This variant has the value for the current option we are looping over
      );
      if (!hasValue) return false;

      // If it has the value, does it conflict with OTHER selections?
      // Loop through all OTHER currently selected options
      for (const [otherOptId, otherValId] of Object.entries(selectedOptions)) {
        // Skip the option we are currently testing (since we overrode it in logic, or just ignore it here)
        if (otherOptId === optionId) continue;

        // For this variant, what is the value for `otherOptId`?
        // (We assume every variant has values for all options - or strict subset if sparse)
        const variantValForOtherOpt = variant.productOptionValues.find((v) => {
          // We need to know which OptionID this value belongs to.
          // The variant's value list is flat. We need to look up component's options prop to map ValueID -> OptionID?
          // OR we can assume `options` prop has the structure.

          // Let's correctly find if this variant matches `otherValId`.
          // Actually, easier: checking if `otherValId` is present in this variant's values list.
          return v.id === otherValId;
        });

        if (!variantValForOtherOpt) {
          // If this variant does NOT have the value selected for another option,
          // then this variant is NOT a match for the current combination.
          return false;
        }
      }

      // If we passed all checks, this variant is a valid candidate.
      // Also check if stock > 0? Maybe UI wants to show "Out of stock" vs "Invalid combo".
      // Requirement often is: "dim invalid/impossible combinations".
      // Let's enforce stock check if desired, but "valid combination" usually means "it exists".
      // If user wants to block OOS items, we add && variant.availableStock > 0
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
                    "relative flex items-center justify-center rounded-md border py-2 px-4 text-sm font-medium uppercase hover:bg-gray-50 focus:outline-none sm:flex-1 sm:py-3 cursor-pointer transition-all",
                    // Selected State
                    isSelected
                      ? "border-transparent bg-blue-600 text-white hover:bg-blue-600 shadow-sm"
                      : "border-gray-200 text-gray-900 dark:border-gray-700 dark:text-gray-200 dark:hover:bg-gray-700 bg-white dark:bg-gray-800",
                    // Unavailable State (dimmed but clickable)
                    // High opacity text (gray-400) for readability, dashed border to signify unavailable
                    // Light mode: Opacity 50 + Gray 100 bg to clearly look "disabled"
                    // Dark mode: Opacity 100 (explicit colors) to ensure readability as per previous request
                    !available &&
                      !isSelected &&
                      "bg-gray-100 text-gray-400 border-dashed border-gray-300 opacity-50 dark:opacity-100 dark:bg-gray-800/50 dark:text-gray-500 dark:border-gray-600",
                    // Selected BUT Unavailable (e.g. invalid combo selected)
                    // Just dim the selected blue state, don't change color.
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
