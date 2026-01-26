export interface ProductImage {
  id: string;
  imageUrl: string;
  displayOrder: number;
}

export interface ProductVariant {
  id: string;
  price: number;
  availableStock: number;
  imageUrl: string;
  productOptionValues: ProductOptionValue[];
}

export interface ProductOptionValue {
  id: string;
  value: string;
}

export interface ProductOption {
  id: string;
  name: string;
  productOptionValues: ProductOptionValue[];
}

export interface Brand {
  id: string;
  name: string;
  slug: string;
  logoUrl: string;
}

export interface Category {
  id: string;
  name: string;
  slug: string;
  imageUrl: string;
}

export interface ProductDetail {
  id: string;
  name: string;
  slug: string;
  description: string;
  brand?: Brand;
  categories: Category[];
  productOptions: ProductOption[];
  productVariants: ProductVariant[];
  productImages: ProductImage[];
}
