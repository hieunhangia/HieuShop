export const PAGES = {
  HOME: {
    PATH: "/",
    TITLE: "Trang chủ",
  },
  PRODUCTS: {
    PATH: "/products",
    TITLE: "Sản phẩm",
  },
  PRODUCTS_BY_SLUG: {
    PATH: "/:slug/products",
    TITLE: "Sản phẩm",
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
