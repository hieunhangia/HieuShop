import ReactMarkdown from "react-markdown";

interface ProductDescriptionProps {
  description: string;
}

const ProductDescription = ({ description }: ProductDescriptionProps) => {
  return (
    <div className="prose prose-blue max-w-none dark:prose-invert">
      <h2 className="text-xl font-bold text-gray-900 mb-4 dark:text-white">
        Mô tả sản phẩm
      </h2>
      <div className="text-gray-600 leading-relaxed dark:text-gray-300">
        <ReactMarkdown
          components={{
            h1: ({ node, ...props }) => (
              <h3 className="text-lg font-bold mt-4 mb-2" {...props} />
            ),
            h2: ({ node, ...props }) => (
              <h4 className="text-md font-bold mt-3 mb-2" {...props} />
            ),
            ul: ({ node, ...props }) => (
              <ul className="list-disc pl-5 my-2" {...props} />
            ),
            ol: ({ node, ...props }) => (
              <ol className="list-decimal pl-5 my-2" {...props} />
            ),
            li: ({ node, ...props }) => <li className="mb-1" {...props} />,
          }}
        >
          {description}
        </ReactMarkdown>
      </div>
    </div>
  );
};

export default ProductDescription;
