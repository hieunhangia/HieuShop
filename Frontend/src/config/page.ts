export const PAGES = {
  HOME: {
    PATH: "/",
    TITLE: "Trang chủ",
  },
  PRODUCTS: {
    ALL: {
      PATH: "/products",
      TITLE: "Sản phẩm",
    },
    BY_SLUG: {
      PATH: "/:slug/products",
      TITLE: "Sản phẩm theo danh mục",
    },
    DETAIL: {
      PATH: "/products/:slug",
      TITLE: "Chi tiết sản phẩm",
    },
  },
  IDENTITY: {
    LOGIN: {
      PATH: "/login",
      TITLE: "Đăng nhập",
    },
    REGISTER: {
      PATH: "/register",
      TITLE: "Đăng ký",
    },
    FORGOT_PASSWORD: {
      PATH: "/forgot-password",
      TITLE: "Quên mật khẩu",
    },
    RESET_PASSWORD: {
      PATH: "/reset-password",
      TITLE: "Đặt lại mật khẩu",
    },
    CONFIRM_EMAIL: {
      PATH: "/confirm-email",
      TITLE: "Xác thực Email",
    },
  },
  ACCOUNT: {
    INFO: {
      PATH: "/account/info",
      TITLE: "Thông tin tài khoản",
    },
    CHANGE_PASSWORD: {
      PATH: "/account/change-password",
      TITLE: "Đổi mật khẩu",
    },
    SET_PASSWORD: {
      PATH: "/account/set-password",
      TITLE: "Thiết lập mật khẩu",
    },
  },
} as const;
