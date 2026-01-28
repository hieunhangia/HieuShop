import { useEffect, useState } from "react";
import type { UserShippingAddressSummary } from "../../types/shipping-addresses/dtos/UserShippingAddressSummary";
import userShippingAddressApi from "../../api/userShippingAddressApi";
import { parseApiError } from "../../utils/error";
import { MapPin, Phone, User, Plus } from "lucide-react";
import { PAGES } from "../../config/page";
import Modal from "../../components/Modal";
import ShippingAddressForm from "./ShippingAddressForm";
import toast from "react-hot-toast";
import MainLayout from "../../layouts/MainLayout";

export default function ShippingAddressPage() {
  const [addresses, setAddresses] = useState<UserShippingAddressSummary[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedAddressId, setSelectedAddressId] = useState<string | null>(
    null,
  );

  useEffect(() => {
    fetchAddresses();
  }, []);

  const fetchAddresses = async () => {
    try {
      setLoading(true);
      const data = await userShippingAddressApi.getUserShippingAddresses();
      setAddresses(data);
    } catch (err) {
      setError(
        parseApiError(err, "Có lỗi xảy ra khi tải danh sách địa chỉ").message,
      );
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = () => {
    setSelectedAddressId(null);
    setIsModalOpen(true);
  };

  const handleEdit = (id: string) => {
    setSelectedAddressId(id);
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setSelectedAddressId(null);
  };

  const handleSuccess = (action?: "create" | "update" | "delete") => {
    handleCloseModal();
    fetchAddresses();
    if (action === "delete") {
      toast.success("Xóa địa chỉ thành công");
    } else if (action === "update") {
      toast.success("Cập nhật địa chỉ thành công");
    } else {
      toast.success("Thêm địa chỉ thành công");
    }
  };

  return (
    <MainLayout>
      <div className="min-h-screen bg-gray-50 dark:bg-gray-900 pt-20 pb-12 px-4 sm:px-6 lg:px-8 transition-colors duration-300">
        <div className="max-w-4xl mx-auto">
          <div className="flex justify-between items-center mb-8">
            <h1 className="text-3xl font-bold text-gray-900 dark:text-white">
              {PAGES.USER.SHIPPING_ADDRESS.TITLE}
            </h1>
            <button
              onClick={handleCreate}
              className="flex items-center px-4 py-2 bg-brand-600 hover:bg-brand-700 text-white rounded-lg transition-colors shadow-sm cursor-pointer"
            >
              <Plus size={20} className="mr-2" />
              Thêm địa chỉ mới
            </button>
          </div>

          {error && (
            <div className="bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 text-red-600 dark:text-red-400 p-4 rounded-lg mb-6">
              {error}
            </div>
          )}

          {loading ? (
            <div className="grid gap-4 md:grid-cols-2">
              {[1, 2, 3, 4].map((i) => (
                <div
                  key={i}
                  className="bg-white dark:bg-gray-800 p-6 rounded-xl shadow-sm border border-gray-100 dark:border-gray-800 animate-pulse"
                >
                  <div className="h-4 bg-gray-200 dark:bg-gray-700 rounded w-1/3 mb-4"></div>
                  <div className="space-y-2">
                    <div className="h-3 bg-gray-200 dark:bg-gray-700 rounded w-1/2"></div>
                    <div className="h-3 bg-gray-200 dark:bg-gray-700 rounded w-full"></div>
                  </div>
                </div>
              ))}
            </div>
          ) : addresses.length === 0 ? (
            <div className="text-center py-12 bg-white dark:bg-gray-800 rounded-xl shadow-sm border border-gray-100 dark:border-gray-800">
              <MapPin
                size={48}
                className="mx-auto text-gray-400 dark:text-gray-500 mb-4"
              />
              <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-1">
                Chưa có địa chỉ nào
              </h3>
              <p className="text-gray-500 dark:text-gray-400">
                Bạn chưa thêm địa chỉ giao hàng nào.
              </p>
            </div>
          ) : (
            <div className="grid gap-4 md:grid-cols-2">
              {addresses.map((address) => (
                <div
                  key={address.id}
                  onClick={() => handleEdit(address.id)}
                  className="bg-white dark:bg-gray-800 p-6 rounded-xl shadow-sm border border-gray-100 dark:border-gray-800 hover:shadow-md transition-shadow group cursor-pointer relative overflow-hidden"
                >
                  <div className="absolute top-0 left-0 w-1 h-full bg-brand-500 opacity-0 group-hover:opacity-100 transition-opacity"></div>
                  <div className="flex items-start justify-between">
                    <div className="space-y-3">
                      <div className="flex items-center text-gray-900 dark:text-white font-medium">
                        <User className="w-4 h-4 mr-2 text-gray-400" />
                        {address.recipientName}
                      </div>
                      <div className="flex items-center text-gray-600 dark:text-gray-300">
                        <Phone className="w-4 h-4 mr-2 text-gray-400" />
                        {address.recipientPhone}
                      </div>
                      <div className="flex items-start text-gray-600 dark:text-gray-300">
                        <MapPin className="w-4 h-4 mr-2 text-gray-400 mt-1 flex-shrink-0" />
                        <span className="leading-relaxed">
                          {address.addressString}
                        </span>
                      </div>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}

          <Modal
            isOpen={isModalOpen}
            onClose={handleCloseModal}
            title={selectedAddressId ? "Cập nhật địa chỉ" : "Thêm địa chỉ mới"}
          >
            <ShippingAddressForm
              initialAddressId={selectedAddressId}
              onSuccess={handleSuccess}
              onCancel={handleCloseModal}
            />
          </Modal>
        </div>
      </div>
    </MainLayout>
  );
}
