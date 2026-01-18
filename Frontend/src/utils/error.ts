export interface ApiError {
    title?: string;
    message: string;
}

export const parseApiError = (error: any, fallbackMessage: string): ApiError => {
    if (!error?.response?.data) {
        return { message: fallbackMessage };
    }

    const { title, detail, errors } = error.response.data;
    const parts: string[] = [];

    if (detail) parts.push(detail);

    if (errors && typeof errors === 'object') {
        const validationMessages = Object.values(errors)
            .flat()
            .map((msg) => String(msg))
            .join('\n');
        if (validationMessages) {
            parts.push(validationMessages);
        }
    }

    if (parts.length > 0) {
        return {
            title: title || undefined,
            message: parts.join('\n')
        };
    }

    return { message: fallbackMessage };
};
