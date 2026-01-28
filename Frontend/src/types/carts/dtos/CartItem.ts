export interface CartItem {
    id: string;
    productVariant: {
        id: string;
        imageUrl: string;
        price: number;
        productOptionValuesString: string;
        product: {
            id: string;
            name: string;
            slug: string;
        };
    };
    quantity: number;
}
